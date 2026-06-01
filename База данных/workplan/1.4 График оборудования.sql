SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';
SET @target_area_id = 1;

WITH RECURSIVE calendar AS (
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < @end_date
),

-- 1. СЕТКА СМЕН ОБОРУДОВАНИЯ (с историей настроек)
EquipmentShifts AS (
    SELECT 
        c.day_date, eq.id AS eq_id, eq.name AS eq_name, eq.sort_order AS eq_sort,
        sd.id AS shift_id, sd.name AS shift_name, sd.shift_number,
        COALESCE((SELECT is_cancelled FROM equipment_daily_plan edp 
                  WHERE edp.equipment_id = eq.id AND edp.plan_date = c.day_date AND edp.shift_id = sd.id), 0) as is_cancelled,
        COALESCE((SELECT staffing_mode FROM equipment_staffing_history WHERE equipment_id = eq.id AND valid_from <= c.day_date ORDER BY valid_from DESC LIMIT 1), eq.staffing_mode) AS active_mode,
        EXISTS (SELECT 1 FROM equipment_daily_plan edp2 WHERE edp2.equipment_id = eq.id AND edp2.plan_date = c.day_date AND edp2.shift_id = sd.id) AS has_plan_entry
    FROM calendar c
    CROSS JOIN equipment eq
    JOIN schedule_templates st ON COALESCE((SELECT template_id FROM equipment_schedule_history WHERE equipment_id = eq.id AND valid_from <= c.day_date ORDER BY valid_from DESC LIMIT 1), eq.template_id) = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (((DATEDIFF(c.day_date, st.base_date) % sc.cycle_length) + sc.cycle_length) % sc.cycle_length + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
    WHERE eq.work_area_id = @target_area_id
),

-- 2. ВСЕ СВЯЗИ СОТРУДНИКОВ (План + Правки)
EmployeeLinks AS (
    SELECT 
        c.day_date, e.id AS emp_id, e.full_name,
        eqa.equipment_id AS eq_id, sd_p.id AS shift_id,
        'PLAN' AS l_type, NULL AS l_status, sd_p.shift_number AS s_num
    FROM calendar c
    CROSS JOIN employees e
    JOIN employee_schedule_assignments esa ON esa.employee_id = e.id AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    JOIN schedule_templates st_e ON esa.template_id = st_e.id
    JOIN schedule_cycles sc_e ON st_e.cycle_id = sc_e.id
    JOIN schedule_cycle_items sci_e ON sci_e.cycle_id = sc_e.id AND sci_e.day_number = (((DATEDIFF(c.day_date, st_e.base_date) % sc_e.cycle_length) + sc_e.cycle_length) % sc_e.cycle_length + 1)
    JOIN shift_definitions sd_p ON sci_e.shift_id = sd_p.id
    JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    
    UNION ALL

    SELECT 
        ovr.override_date, e2.id, e2.full_name,
        ovr.equipment_id, ovr.shift_id,
        'OVR' AS l_type, ovr.status AS l_status, sd_o.shift_number AS s_num
    FROM schedule_overrides ovr
    JOIN employees e2 ON ovr.employee_id = e2.id
    JOIN shift_definitions sd_o ON ovr.shift_id = sd_o.id
)

-- 3. ИТОГОВАЯ СБОРКА
SELECT 
    qs.day_date AS "Дата",
    qs.eq_name AS "Станок",
    qs.shift_name AS "Смена",

    -- ИНДИКАТОР ПОТРЕБНОСТИ
    CASE 
        WHEN qs.is_cancelled = 1 THEN '⚪ Не требуется (Отмена)'
        WHEN qs.active_mode = 'manual_only' AND NOT qs.has_plan_entry THEN '⚪ Не требуется (Вне плана)'
        WHEN (
            SELECT COUNT(DISTINCT el.emp_id) FROM EmployeeLinks el 
            WHERE el.day_date = qs.day_date 
            AND (SELECT 1 FROM absences a WHERE a.employee_id = el.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31') LIMIT 1) IS NULL
            AND (
                (el.l_type = 'OVR' AND el.l_status = 2 AND el.eq_id = qs.eq_id AND el.shift_id = qs.shift_id AND el.s_num > 0)
                OR
                (el.l_type = 'PLAN' AND el.eq_id = qs.eq_id AND el.shift_id = qs.shift_id 
                 AND NOT EXISTS (SELECT 1 FROM schedule_overrides o3 WHERE o3.employee_id = el.emp_id AND o3.override_date = qs.day_date AND o3.shift_id = qs.shift_id AND o3.status = 2))
            )
        ) > 0 THEN '✅ Укомплектовано'
        WHEN qs.active_mode = 'strict_schedule' THEN '🚨 ТРЕБУЕТСЯ'
        ELSE '⚠️ НУЖНО НАЗНАЧЕНИЕ'
    END AS "Потребность",

    -- ПЛАН_ГРАФИК (Изначальный план из циклов)
    GROUP_CONCAT(DISTINCT 
        CASE WHEN esl.l_type = 'PLAN' AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id THEN
            CONCAT(esl.full_name, 
                CASE 
                    WHEN (SELECT 1 FROM absences a WHERE a.employee_id = esl.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31') LIMIT 1) IS NOT NULL THEN ' (❌ Болен)'
                    WHEN EXISTS (SELECT 1 FROM EmployeeLinks ov WHERE ov.l_type = 'OVR' AND ov.emp_id = esl.emp_id AND ov.day_date = qs.day_date AND ov.shift_id = qs.shift_id AND ov.s_num = 0) THEN ' (🚫 Отмена)'
                    WHEN EXISTS (SELECT 1 FROM EmployeeLinks ov2 WHERE ov2.l_type = 'OVR' AND ov2.emp_id = esl.emp_id AND ov2.day_date = qs.day_date AND ov2.shift_id = qs.shift_id AND ov2.l_status = 2 AND ov2.eq_id != qs.eq_id) THEN ' (➡️ Переведен)'
                    ELSE ' (✅)' 
                END
            )
        END 
    SEPARATOR ' | ') AS "План_График",

    -- НАЗНАЧЕНИЯ (Утвержденные оверрайды)
    GROUP_CONCAT(DISTINCT 
        CASE WHEN esl.l_type = 'OVR' AND esl.l_status = 2 AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id AND esl.s_num > 0 THEN 
            esl.full_name 
        END 
    SEPARATOR ', ') AS "Назначения",

    -- ЧЕРНОВИКИ (Предварительные правки)
    GROUP_CONCAT(DISTINCT 
        CASE WHEN esl.l_type = 'OVR' AND esl.l_status < 2 AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id THEN 
            CONCAT('📝 ', esl.full_name) 
        END 
    SEPARATOR ', ') AS "Черновики",

    -- УТВЕРЖДЕННЫЙ ФАКТ (Финальный список)
    GROUP_CONCAT(DISTINCT 
        CASE 
            WHEN (SELECT 1 FROM absences a WHERE a.employee_id = esl.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31') LIMIT 1) IS NULL 
            AND (
                (esl.l_type = 'OVR' AND esl.l_status = 2 AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id AND esl.s_num > 0)
                OR
                (esl.l_type = 'PLAN' AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id 
                 AND NOT EXISTS (SELECT 1 FROM schedule_overrides o4 WHERE o4.employee_id = esl.emp_id AND o4.override_date = qs.day_date AND o4.shift_id = qs.shift_id AND o4.status = 2 LIMIT 1))
            ) THEN esl.full_name
        END 
    SEPARATOR ', ') AS "Утвержденный_Факт"

FROM EquipmentShifts qs
LEFT JOIN EmployeeLinks esl ON qs.day_date = esl.day_date
GROUP BY qs.day_date, qs.eq_id, qs.shift_id
ORDER BY qs.day_date, qs.eq_sort, qs.shift_number;
