SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';
SET @target_emp_id = 1; -- Укажите ID конкретного сотрудника

WITH RECURSIVE calendar AS (
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < @end_date
),
EmployeeDailyInfo AS (
    -- Собираем данные сотрудника на каждый день: график, должность, родной станок
    SELECT 
        c.day_date,
        e.id AS emp_id,
        e.full_name,
        -- 1. Находим актуальный шаблон (бригаду)
        (SELECT st.id FROM employee_schedule_assignments esa 
         JOIN schedule_templates st ON esa.template_id = st.id
         WHERE esa.employee_id = e.id AND esa.valid_from <= c.day_date 
         ORDER BY esa.valid_from DESC LIMIT 1) AS current_template_id,
        -- 2. Находим актуальную должность
        (SELECT p.name FROM employee_position_assignments epa 
         JOIN positions p ON epa.position_id = p.id
         WHERE epa.employee_id = e.id AND epa.valid_from <= c.day_date 
         ORDER BY epa.valid_from DESC LIMIT 1) AS current_position,
        -- 3. Находим родной станок
        (SELECT eq.id FROM employee_equipment_assignments eqa 
         JOIN equipment eq ON eqa.equipment_id = eq.id
         WHERE eqa.employee_id = e.id AND eqa.valid_from <= c.day_date 
         ORDER BY eqa.valid_from DESC LIMIT 1) AS default_eq_id
    FROM calendar c
    CROSS JOIN employees e
    WHERE e.id = @target_emp_id
)
SELECT 
    edi.day_date AS "Дата",
    edi.current_position AS "Должность",
    
    -- ПЛАНОВАЯ СМЕНА (из цикла)
    COALESCE(sd_plan.name, 'Выходной') AS "Смена_по_графику",

    -- СТАТУС (Болезнь / Отпуск)
    COALESCE(abt.name, '-') AS "Отсутствие",

    -- ФАКТИЧЕСКАЯ РАБОТА (с учетом правок)
    CASE 
        WHEN abt.id IS NOT NULL THEN 'Не работает'
        WHEN ovr.id IS NOT NULL THEN sd_ovr.name
        ELSE sd_plan.name 
    END AS "Фактическая_смена",

    -- ОБОРУДОВАНИЕ
    CASE 
        WHEN abt.id IS NOT NULL THEN '-'
        WHEN ovr.equipment_id IS NOT NULL THEN (SELECT name FROM equipment WHERE id = ovr.equipment_id)
        ELSE (SELECT name FROM equipment WHERE id = edi.default_eq_id)
    END AS "Станок",

    -- ТИП НАЗНАЧЕНИЯ
    CASE 
        WHEN abt.id IS NOT NULL THEN '❌ Отсутствие'
        WHEN ovr.id IS NOT NULL AND ovr.equipment_id != edi.default_eq_id THEN '🔄 Подмена'
        WHEN ovr.id IS NOT NULL THEN '✍️ Ручная правка'
        WHEN sd_plan.shift_number > 0 THEN '✅ По плану'
        ELSE '☕ Выходной'
    END AS "Тип_дня",

    -- СОГЛАСОВАНИЕ (для ручных правок)
    CASE 
        WHEN ovr.id IS NULL THEN '-'
        WHEN ovr.status = 0 THEN '⏳ Черновик'
        WHEN ovr.status = 1 THEN '👤 Подтверждено'
        WHEN ovr.status = 2 THEN '✅ Утверждено'
        ELSE '🚫 Отклонено'
    END AS "Статус_согласования"

FROM EmployeeDailyInfo edi
-- Соединяем с циклом для расчета плановой смены
LEFT JOIN schedule_templates st ON edi.current_template_id = st.id
LEFT JOIN schedule_cycles sc ON st.cycle_id = sc.id
LEFT JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
    AND sci.day_number = (MOD(DATEDIFF(edi.day_date, st.base_date), sc.cycle_length) + 1)
LEFT JOIN shift_definitions sd_plan ON sci.shift_id = sd_plan.id

-- Соединяем с ручными правками (Overrides)
LEFT JOIN schedule_overrides ovr ON ovr.employee_id = edi.emp_id AND ovr.override_date = edi.day_date
LEFT JOIN shift_definitions sd_ovr ON ovr.shift_id = sd_ovr.id

-- Соединяем с отсутствиями
LEFT JOIN absences abs ON abs.employee_id = edi.emp_id 
    AND edi.day_date >= abs.start_date AND (abs.end_date IS NULL OR edi.day_date <= abs.end_date)
LEFT JOIN absence_types abt ON abs.type_id = abt.id

ORDER BY edi.day_date;
