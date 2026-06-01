SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';
SET @target_area_id = 1;

WITH RECURSIVE calendar AS (
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < @end_date
),

-- 1. СЕТКА ОБОРУДОВАНИЯ (история настроек)
EquipmentGrid AS (
    SELECT 
        c.day_date, 
        eq.id AS eq_id, 
        eq.name AS eq_name,
        eq.sort_order AS eq_sort,
        COALESCE(
            (SELECT staffing_mode FROM equipment_staffing_history 
             WHERE equipment_id = eq.id AND valid_from <= c.day_date 
             ORDER BY valid_from DESC LIMIT 1),
            eq.staffing_mode
        ) AS active_mode,
        COALESCE(
            (SELECT template_id FROM equipment_schedule_history 
             WHERE equipment_id = eq.id AND valid_from <= c.day_date 
             ORDER BY valid_from DESC LIMIT 1),
            eq.template_id
        ) AS active_template_id
    FROM calendar c
    CROSS JOIN equipment eq
    WHERE eq.work_area_id = @target_area_id 
      AND c.day_date BETWEEN eq.commissioned_at AND COALESCE(eq.decommissioned_at, '2099-12-31')
),

-- 2. ПЛАНОВЫЕ СМЕНЫ ОБОРУДОВАНИЯ (расчет по циклам)
EquipmentShifts AS (
    SELECT 
        eg.*,
        sd.id AS shift_id, sd.name AS shift_name, sd.shift_number,
        COALESCE((SELECT is_cancelled FROM equipment_daily_plan 
                  WHERE equipment_id = eg.eq_id AND plan_date = eg.day_date AND shift_id = sd.id), 0) as is_cancelled
    FROM EquipmentGrid eg
    JOIN schedule_templates st ON eg.active_template_id = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (((DATEDIFF(eg.day_date, st.base_date) % sc.cycle_length) + sc.cycle_length) % sc.cycle_length + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
),

-- 3. СОСТОЯНИЕ СОТРУДНИКОВ
EmployeeStatus AS (
    SELECT 
        c.day_date,
        e.id AS emp_id, e.full_name,
        eqa.equipment_id AS plan_eq_id,
        sd_p.shift_number AS plan_shift_num,
        ovr.equipment_id AS ovr_eq_id,
        ovr.status AS ovr_status,
        (SELECT shift_number FROM shift_definitions WHERE id = ovr.shift_id) AS ovr_shift_num,
        (SELECT abt.name FROM absences abs JOIN absence_types abt ON abs.type_id = abt.id 
         WHERE abs.employee_id = e.id AND c.day_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31') LIMIT 1) AS abs_name
    FROM calendar c
    CROSS JOIN employees e
    LEFT JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
        AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    LEFT JOIN schedule_templates st_e ON esa.template_id = st_e.id
    LEFT JOIN schedule_cycles sc_e ON st_e.cycle_id = sc_e.id
    LEFT JOIN schedule_cycle_items sci_e ON sci_e.cycle_id = sc_e.id 
        AND sci_e.day_number = (((DATEDIFF(c.day_date, st_e.base_date) % sc_e.cycle_length) + sc_e.cycle_length) % sc_e.cycle_length + 1)
    LEFT JOIN shift_definitions sd_p ON sci_e.shift_id = sd_p.id
    LEFT JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
        AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id AND ovr.override_date = c.day_date
)

-- 4. ФИНАЛЬНАЯ СБОРКА
SELECT 
    es.day_date AS "Дата",
    es.eq_name AS "Станок",
    es.shift_name AS "Смена",

    CASE 
        WHEN es.is_cancelled = 1 THEN '🛑 Остановлен'
        WHEN es.active_mode = 'manual_only' AND NOT EXISTS (SELECT 1 FROM equipment_daily_plan WHERE equipment_id = es.eq_id AND plan_date = es.day_date AND shift_id = es.shift_id) THEN '🔘 Вне плана'
        ELSE '✅ Работа'
    END AS "Статус_Обор",

    -- ПЛАНОВЫЕ
    GROUP_CONCAT(DISTINCT 
        CASE WHEN est.plan_eq_id = es.eq_id AND est.plan_shift_num = es.shift_number THEN
            CONCAT(est.full_name, 
                CASE 
                    WHEN est.abs_name IS NOT NULL THEN CONCAT(' (❌ ', est.abs_name, ')')
                    WHEN est.ovr_status = 2 AND est.ovr_eq_id != es.eq_id THEN ' (➡️ Переведен)'
                    WHEN est.ovr_status < 2 AND est.ovr_eq_id IS NOT NULL AND est.ovr_eq_id != es.eq_id THEN ' (⏳ План переноса)'
                    ELSE ' (✅)' 
                END
            )
        END 
    SEPARATOR ' | ') AS "План_и_Отклонения",

    -- ЧЕРНОВИКИ (status < 2)
    GROUP_CONCAT(DISTINCT 
        CASE WHEN est.ovr_status < 2 AND est.ovr_eq_id = es.eq_id AND est.ovr_shift_num = es.shift_number THEN 
            CONCAT('📝 ', est.full_name) 
        END 
    SEPARATOR ', ') AS "На_согласовании",

    -- УТВЕРЖДЕННЫЙ ФАКТ
    GROUP_CONCAT(DISTINCT 
        CASE 
            WHEN est.abs_name IS NULL AND (
                (est.ovr_status = 2 AND est.ovr_eq_id = es.eq_id AND est.ovr_shift_num = es.shift_number) OR
                (est.plan_eq_id = es.eq_id AND est.plan_shift_num = es.shift_number AND (est.ovr_status IS NULL OR est.ovr_status != 2))
            ) THEN est.full_name
        END 
    SEPARATOR ', ') AS "Утвержденный_Факт"

FROM EquipmentShifts es
LEFT JOIN EmployeeStatus est ON es.day_date = est.day_date
GROUP BY es.day_date, es.eq_id, es.shift_id
ORDER BY es.day_date, es.eq_sort, es.shift_number;
