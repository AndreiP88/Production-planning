SET @target_date = '2026-05-20';
SET @target_shift_num = 1; -- Ищем на 1-ю смену
SET @target_area_id = 1;   -- Для работы на 1-м участке

WITH EmployeeStatus AS (
    SELECT 
        e.id, 
        e.full_name,
        p.name AS position_name,
        -- Определяем плановую смену сотрудника
        (SELECT sd_e.shift_number FROM employee_schedule_assignments esa 
         JOIN schedule_templates st_e ON esa.template_id = st_e.id
         JOIN schedule_cycles sc_e ON st_e.cycle_id = sc_e.id
         JOIN schedule_cycle_items sci_e ON sci_e.cycle_id = sc_e.id 
         JOIN shift_definitions sd_e ON sci_e.shift_id = sd_e.id
         WHERE esa.employee_id = e.id AND esa.valid_from <= @target_date 
           AND sci_e.day_number = (MOD(DATEDIFF(@target_date, st_e.base_date), sc_e.cycle_length) + 1)
         ORDER BY esa.valid_from DESC LIMIT 1) AS plan_shift,
         
        -- На каком станке он должен работать по плану
        (SELECT eq.name FROM employee_equipment_assignments eqa 
         JOIN equipment eq ON eqa.equipment_id = eq.id
         WHERE eqa.employee_id = e.id AND eqa.valid_from <= @target_date 
         ORDER BY eqa.valid_from DESC LIMIT 1) AS default_equipment_name,
         
        -- Проверка, не болен ли он
        EXISTS (SELECT 1 FROM absences abs 
                WHERE abs.employee_id = e.id AND @target_date >= abs.start_date 
                AND (abs.end_date IS NULL OR @target_date <= abs.end_date)) AS is_absent,
                
        -- Проверка, не назначен ли он уже вручную куда-то
        (SELECT eq_ovr.name FROM schedule_overrides ovr 
         JOIN equipment eq_ovr ON ovr.equipment_id = eq_ovr.id
         WHERE ovr.employee_id = e.id AND ovr.override_date = @target_date AND ovr.status = 2 LIMIT 1) AS manual_assigned_to
    FROM employees e
    JOIN employment_periods ep ON ep.employee_id = e.id 
        AND (@target_date >= ep.hire_date AND (ep.fire_date IS NULL OR @target_date <= ep.fire_date))
    LEFT JOIN employee_position_assignments epa ON epa.employee_id = e.id 
        AND epa.valid_from = (SELECT MAX(valid_from) FROM employee_position_assignments WHERE employee_id = e.id AND valid_from <= @target_date)
    LEFT JOIN positions p ON epa.position_id = p.id
)
SELECT 
    id AS "ID Сотрудниа",
    full_name AS "Сотрудник",
    position_name AS "Должность",
    CASE 
        WHEN is_absent THEN '❌ БОЛЕН/ОТСУТСТВУЕТ'
        WHEN manual_assigned_to IS NOT NULL THEN CONCAT('📌 УЖЕ ПОДМЕНЯЕТ на ', manual_assigned_to)
        WHEN plan_shift = @target_shift_num THEN CONCAT('⚙️ РАБОТАЕТ на ', COALESCE(default_equipment_name, 'участке'))
        WHEN plan_shift = 0 OR plan_shift IS NULL THEN '🟢 ВЫХОДНОЙ'
        ELSE '🟡 ДРУГАЯ СМЕНА'
    END AS "Текущий_статус",
    
    -- ЛОГИКА ПРИОРИТЕТОВ
    CASE 
        WHEN is_absent OR manual_assigned_to IS NOT NULL THEN 4 -- Недоступен совсем
        WHEN plan_shift = 0 OR plan_shift IS NULL THEN 1        -- ПЕРВЫЙ ПРИОРИТЕТ: Свободные (выходные)
        WHEN plan_shift != @target_shift_num THEN 2            -- ВТОРОЙ ПРИОРИТЕТ: Люди из других смен
        ELSE 3                                                  -- ТРЕТИЙ ПРИОРИТЕТ: Переназначение с другого станка
    END AS priority_order

FROM EmployeeStatus
ORDER BY priority_order ASC, position_name ASC, full_name ASC;
