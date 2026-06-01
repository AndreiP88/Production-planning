SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';

WITH RECURSIVE calendar AS (
    -- 1. Генерируем сетку дат месяца
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY
    FROM calendar
    WHERE day_date < @end_date
),
EmployeeData AS (
    -- 2. Собираем работающих сотрудников и их актуальные данные на каждый день
    SELECT 
        c.day_date,
        e.id AS emp_id,
        e.full_name,
        -- Актуальная должность
        (SELECT p.name FROM employee_position_assignments epa 
         JOIN positions p ON epa.position_id = p.id
         WHERE epa.employee_id = e.id AND epa.valid_from <= c.day_date 
         ORDER BY epa.valid_from DESC LIMIT 1) AS current_position,
        -- Актуальное "родное" оборудование
        (SELECT eqa.equipment_id FROM employee_equipment_assignments eqa 
         WHERE eqa.employee_id = e.id AND eqa.valid_from <= c.day_date 
         ORDER BY eqa.valid_from DESC LIMIT 1) AS default_eq_id,
        -- Актуальный график (шаблон)
        (SELECT esa.template_id FROM employee_schedule_assignments esa 
         WHERE esa.employee_id = e.id AND esa.valid_from <= c.day_date 
         ORDER BY esa.valid_from DESC LIMIT 1) AS template_id
    FROM calendar c
    CROSS JOIN employees e
    INNER JOIN employment_periods ep ON ep.employee_id = e.id
        AND (c.day_date >= ep.hire_date AND (ep.fire_date IS NULL OR c.day_date <= ep.fire_date))
)
SELECT 
    ed.day_date AS "Дата",
    ed.full_name AS "Сотрудник",
    ed.current_position AS "Должность",
    
    -- 1. Смена по графику (план)
    CONCAT(sd_plan.shift_number, ' (', sd_plan.name, ')') AS "Смена_по_графику",

    -- 2. Отсутствия (Больничный/Отпуск)
    COALESCE(abt.name, '-') AS "Отсутствие",

    -- 3. Смена с учетом назначений (Факт)
    -- Показывает ручную правку, если она есть, иначе план
    CASE 
        WHEN ovr.id IS NOT NULL THEN CONCAT(sd_ovr.shift_number, ' (', sd_ovr.name, ')')
        ELSE CONCAT(sd_plan.shift_number, ' (', sd_plan.name, ')')
    END AS "Смена_с_учетом_назначений",

    -- 4. Статус согласования
    CASE 
        WHEN ovr.id IS NULL THEN 'Ок (По графику)'
        WHEN ovr.status = 0 THEN '⏳ Черновик'
        WHEN ovr.status = 1 THEN '👤 Подтверждено'
        WHEN ovr.status = 2 THEN '✅ Утверждено'
        WHEN ovr.status = -1 THEN '❌ Отклонено'
    END AS "Согласование",

    -- 5. Оборудование (Фактическое)
    -- Если сотрудник в правке назначен на конкретный станок — выводим его, иначе родной
    COALESCE(eq_ovr.name, eq_def.name, 'Вне оборудования') AS "Оборудование",

    -- 6. Смена в табель (Число)
    -- В табель идет число только если человек не болен И (смена по плану ИЛИ утвержденная правка)
    CASE 
        WHEN abs.id IS NOT NULL THEN 0
        WHEN ovr.id IS NOT NULL THEN IF(ovr.status = 2, sd_ovr.shift_number, 0)
        ELSE sd_plan.shift_number
    END AS "Смена_в_табель"

FROM EmployeeData ed
-- Привязка к плановому циклу смен
LEFT JOIN schedule_templates st ON ed.template_id = st.id
LEFT JOIN schedule_items si ON si.template_id = st.id 
    AND si.day_number = (MOD(DATEDIFF(ed.day_date, st.base_date), st.cycle_length) + 1)
LEFT JOIN shift_definitions sd_plan ON si.shift_id = sd_plan.id

-- Ручные правки графиков
LEFT JOIN schedule_overrides ovr ON ovr.employee_id = ed.emp_id AND ovr.override_date = ed.day_date
LEFT JOIN shift_definitions sd_ovr ON ovr.shift_id = sd_ovr.id
LEFT JOIN equipment eq_ovr ON ovr.equipment_id = eq_ovr.id

-- Родное оборудование
LEFT JOIN equipment eq_def ON ed.default_eq_id = eq_def.id

-- Отсутствия
LEFT JOIN absences abs ON abs.employee_id = ed.emp_id 
    AND ed.day_date >= abs.start_date 
    AND (abs.end_date IS NULL OR ed.day_date <= abs.end_date)
LEFT JOIN absence_types abt ON abs.type_id = abt.id

ORDER BY ed.day_date, ed.full_name;
