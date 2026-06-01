-- ПАРАМЕТРЫ ПОИСКА
SET @target_date = '2026-05-01';
SET @target_shift_number = 2;
SET @target_equipment_id = 1;

WITH EmployeeStatus AS (
    SELECT 
        e.id AS employee_id,
        e.full_name,
        -- Привязки
        (eqa.equipment_id = @target_equipment_id AND sd.shift_number = @target_shift_number) as is_planned_here,
        (ovr.equipment_id = @target_equipment_id AND ovr_sd.shift_number = @target_shift_number) as is_overridden_here,
        -- Отсутствие
        (SELECT abt.name FROM absences abs 
         JOIN absence_types abt ON abs.type_id = abt.id 
         WHERE abs.employee_id = e.id 
           AND @target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31') 
         LIMIT 1) AS absence_reason,
        -- Куда ушел плановый
        (SELECT name FROM equipment WHERE id = ovr.equipment_id) AS moved_to_equipment,
        ovr.equipment_id as ovr_eq_id
    FROM employees e
    JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
        AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= @target_date)
    JOIN schedule_templates st ON esa.template_id = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (MOD(DATEDIFF(@target_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
    LEFT JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
        AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= @target_date)
    LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id AND ovr.override_date = @target_date AND ovr.status = 2
    LEFT JOIN shift_definitions ovr_sd ON ovr.shift_id = ovr_sd.id
),
ActiveWorkforce AS (
    SELECT COUNT(*) as active_count
    FROM EmployeeStatus
    WHERE (is_overridden_here = 1 AND absence_reason IS NULL)
       OR (is_planned_here = 1 AND absence_reason IS NULL AND (ovr_eq_id IS NULL OR ovr_eq_id = @target_equipment_id))
)
SELECT 
    eq.name AS "Станок",
    -- Логика "Нужно назначить"
    CASE 
        WHEN edp.is_cancelled = 1 THEN 'Не требуется (Остановлен)'
        WHEN eq.staffing_mode = 'manual_only' AND edp.id IS NULL THEN 'Не требуется (Вне плана)'
        WHEN (SELECT active_count FROM ActiveWorkforce) = 0 THEN '⚠️ ТРЕБУЕТСЯ НАЗНАЧЕНИЕ'
        ELSE 'Люди не нужны (ОК)'
    END AS "Нужно_назначить",

    es.employee_id AS "ID",
    es.full_name AS "Сотрудник",

    -- СТОЛБЕЦ ПЛАНА
    CASE 
        WHEN es.is_planned_here = 1 THEN 
            CASE 
                WHEN es.absence_reason IS NOT NULL THEN CONCAT('❌ ', es.absence_reason)
                WHEN es.ovr_eq_id IS NOT NULL AND es.ovr_eq_id != @target_equipment_id 
                    THEN CONCAT('➡️ Ушел на ', es.moved_to_equipment)
                ELSE '✅ Должен быть'
            END
        ELSE '---'
    END AS "По_плану",

    -- СТОЛБЕЦ ФАКТА
    CASE 
        WHEN es.absence_reason IS NULL AND (
            (es.is_overridden_here = 1) OR 
            (es.is_planned_here = 1 AND (es.ovr_eq_id IS NULL OR es.ovr_eq_id = @target_equipment_id))
        ) THEN '✅ РАБОТАЕТ'
        ELSE '---'
    END AS "По_факту"

FROM equipment eq
LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = eq.id 
    AND edp.plan_date = @target_date 
    AND edp.shift_id = (SELECT id FROM shift_definitions WHERE shift_number = @target_shift_number LIMIT 1)
LEFT JOIN EmployeeStatus es ON (es.is_planned_here = 1 OR es.is_overridden_here = 1)
WHERE eq.id = @target_equipment_id;
