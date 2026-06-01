SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';
SET @target_area_id = 1;

WITH RECURSIVE calendar AS (
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < @end_date
),
-- 1. Сетка всех требуемых смен оборудования
CombinedTargetShifts AS (
    SELECT 
        day_date, eq_id, eq_name, staffing_mode, shift_id, shift_name, shift_number,
        MAX(is_manual_entry) as has_manual_entry,
        MAX(is_cancelled_entry) as is_cancelled
    FROM (
        -- Плановые смены из графиков станков (через циклы)
        SELECT 
            c.day_date, eq.id AS eq_id, eq.name AS eq_name, eq.staffing_mode,
            sd.id AS shift_id, sd.name AS shift_name, sd.shift_number, 
            0 as is_manual_entry,
            COALESCE((SELECT is_cancelled FROM equipment_daily_plan WHERE equipment_id = eq.id AND plan_date = c.day_date AND shift_id = sd.id), 0) as is_cancelled_entry
        FROM calendar c
        CROSS JOIN equipment eq
        JOIN schedule_templates st ON eq.template_id = st.id
        JOIN schedule_cycles sc ON st.cycle_id = sc.id
        JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
            AND sci.day_number = (MOD(DATEDIFF(c.day_date, st.base_date), sc.cycle_length) + 1)
        JOIN shift_definitions sd ON sci.shift_id = sd.id
        WHERE eq.work_area_id = @target_area_id AND sd.shift_number > 0
          AND c.day_date >= eq.commissioned_at AND (eq.decommissioned_at IS NULL OR c.day_date <= eq.decommissioned_at)

        UNION ALL

        -- Смены, добавленные в план оборудования или через подмены сотрудников
        SELECT edp.plan_date, eq.id, eq.name, eq.staffing_mode, sd.id, sd.name, sd.shift_number, 1, edp.is_cancelled
        FROM equipment_daily_plan edp JOIN equipment eq ON edp.equipment_id = eq.id JOIN shift_definitions sd ON edp.shift_id = sd.id WHERE eq.work_area_id = @target_area_id
        UNION ALL
        SELECT ovr.override_date, eq.id, eq.name, eq.staffing_mode, sd.id, sd.name, sd.shift_number, 1, 0
        FROM schedule_overrides ovr JOIN equipment eq ON ovr.equipment_id = eq.id JOIN shift_definitions sd ON ovr.shift_id = sd.id WHERE eq.work_area_id = @target_area_id AND ovr.status = 2
    ) as raw_data
    GROUP BY day_date, eq_id, eq_name, staffing_mode, shift_id, shift_name, shift_number
),
-- 2. Сбор данных по людям (План, Факт, Статус)
FinalCalculation AS (
    SELECT 
        cts.*,
        -- КТО ДОЛЖЕН БЫТЬ (по его графику и циклу)
        (SELECT GROUP_CONCAT(DISTINCT e.full_name SEPARATOR ', ')
         FROM employees e
         JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id AND eqa.equipment_id = cts.eq_id
         JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
            AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= cts.day_date)
         JOIN schedule_templates st_e ON esa.template_id = st_e.id
         JOIN schedule_cycles sc_e ON st_e.cycle_id = sc_e.id
         JOIN schedule_cycle_items sci_e ON sci_e.cycle_id = sc_e.id 
            AND sci_e.day_number = (MOD(DATEDIFF(cts.day_date, st_e.base_date), sc_e.cycle_length) + 1)
         WHERE (SELECT shift_number FROM shift_definitions WHERE id = sci_e.shift_id) = cts.shift_number) AS Plan_Names,

        -- ПРИЧИНА ОТСУТСТВИЯ ИЛИ ПЕРЕНАЗНАЧЕНИЯ
        (SELECT GROUP_CONCAT(DISTINCT 
            CASE 
                WHEN abs.id IS NOT NULL THEN CONCAT('❌ ', abt.name)
                WHEN ovr.id IS NOT NULL AND ovr.equipment_id != cts.eq_id THEN CONCAT('➡️ НАЗНАЧЕН на ', (SELECT name FROM equipment WHERE id = ovr.equipment_id))
                ELSE NULL 
            END SEPARATOR '; ')
         FROM employees e2
         JOIN employee_equipment_assignments eqa2 ON eqa2.employee_id = e2.id AND eqa2.equipment_id = cts.eq_id
         LEFT JOIN absences abs ON abs.employee_id = e2.id AND cts.day_date >= abs.start_date AND (abs.end_date IS NULL OR cts.day_date <= abs.end_date)
         LEFT JOIN absence_types abt ON abs.type_id = abt.id
         LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e2.id AND ovr.override_date = cts.day_date AND ovr.status = 2
         WHERE EXISTS (SELECT 1 FROM employee_schedule_assignments esa2 
                       JOIN schedule_templates st_e2 ON esa2.template_id = st_e2.id
                       JOIN schedule_cycles sc_e2 ON st_e2.cycle_id = sc_e2.id
                       JOIN schedule_cycle_items sci_e2 ON sci_e2.cycle_id = sc_e2.id
                       WHERE esa2.employee_id = e2.id AND esa2.valid_from <= cts.day_date 
                       AND (SELECT shift_number FROM shift_definitions WHERE id = sci_e2.shift_id) = cts.shift_number
                       AND sci_e2.day_number = (MOD(DATEDIFF(cts.day_date, st_e2.base_date), sc_e2.cycle_length) + 1)
                       LIMIT 1)
        ) AS Plan_Status_Note,

        -- ФАКТИЧЕСКИЕ ИСПОЛНИТЕЛИ
        (SELECT GROUP_CONCAT(DISTINCT e3.full_name ORDER BY e3.full_name SEPARATOR ', ')
         FROM employees e3
         LEFT JOIN employee_equipment_assignments eqa3 ON eqa3.employee_id = e3.id AND eqa3.equipment_id = cts.eq_id
         LEFT JOIN schedule_overrides ovr3 ON ovr3.employee_id = e3.id AND ovr3.override_date = cts.day_date AND ovr3.status = 2
         LEFT JOIN employee_schedule_assignments esa3 ON esa3.employee_id = e3.id
            AND esa3.valid_from = (SELECT MAX(v) FROM (SELECT valid_from as v, employee_id as eid FROM employee_schedule_assignments) as t WHERE eid = e3.id AND v <= cts.day_date)
         LEFT JOIN schedule_templates st_e3 ON esa3.template_id = st_e3.id
         LEFT JOIN schedule_cycles sc_e3 ON st_e3.cycle_id = sc_e3.id
         LEFT JOIN schedule_cycle_items sci_e3 ON sc_e3.id IS NOT NULL AND sci_e3.cycle_id = sc_e3.id 
            AND sci_e3.day_number = (MOD(DATEDIFF(cts.day_date, st_e3.base_date), sc_e3.cycle_length) + 1)
         WHERE (
            (ovr3.equipment_id = cts.eq_id AND (SELECT shift_number FROM shift_definitions WHERE id = ovr3.shift_id) = cts.shift_number)
            OR 
            (eqa3.equipment_id = cts.eq_id AND (SELECT shift_number FROM shift_definitions WHERE id = sci_e3.shift_id) = cts.shift_number AND ovr3.id IS NULL)
         )
         AND NOT EXISTS (SELECT 1 FROM absences abs3 WHERE abs3.employee_id = e3.id AND cts.day_date >= abs3.start_date AND (abs3.end_date IS NULL OR cts.day_date <= abs3.end_date))
        ) AS Fact_Executors
    FROM CombinedTargetShifts cts
)
SELECT 
    day_date AS "Дата",
    eq_id AS "ID Оборудования",
    eq_name AS "Оборудование",
    shift_id AS "ID Смены",
    shift_name AS "Смена",
    COALESCE(Plan_Names, '-') AS "Должен_быть",
    COALESCE(Plan_Status_Note, '-') AS "Статус_планового",
    COALESCE(Fact_Executors, '') AS "Фактически_на_смене",
    CASE 
        WHEN is_cancelled = 1 THEN '🛑 ОТМЕНЕНО'
        WHEN Fact_Executors LIKE '%, %' THEN '👥 ГРУППОВАЯ РАБОТА'
        WHEN Fact_Executors IS NOT NULL AND Fact_Executors <> '' THEN '✅ ОК'
        WHEN has_manual_entry = 1 OR staffing_mode = 'strict_schedule' THEN '⚠️ ALERT: ПУСТАЯ СМЕНА'
        ELSE '⚪ ОЖИДАНИЕ'
    END AS "Состояние"
FROM FinalCalculation
ORDER BY day_date, eq_name, shift_number;
