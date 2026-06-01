SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';
SET @target_emp_id = 1;

WITH RECURSIVE calendar AS (
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < @end_date
),
EmployeeDailyInfo AS (
    SELECT 
        c.day_date,
        e.id AS emp_id,
        -- Актуальный шаблон (график)
        (SELECT esa.template_id FROM employee_schedule_assignments esa 
         WHERE esa.employee_id = e.id AND esa.valid_from <= c.day_date 
         ORDER BY esa.valid_from DESC LIMIT 1) AS current_template_id,
        -- Актуальный основной станок
        (SELECT eqa.equipment_id FROM employee_equipment_assignments eqa 
         WHERE eqa.employee_id = e.id AND eqa.valid_from <= c.day_date 
         ORDER BY eqa.valid_from DESC LIMIT 1) AS default_eq_id,
        -- Название основного станка
        (SELECT eq_n.name FROM equipment eq_n 
         WHERE eq_n.id = (SELECT eqa2.equipment_id FROM employee_equipment_assignments eqa2 
                          WHERE eqa2.employee_id = e.id AND eqa2.valid_from <= c.day_date 
                          ORDER BY eqa2.valid_from DESC LIMIT 1)
        ) AS default_eq_name
    FROM calendar c
    CROSS JOIN employees e
    WHERE e.id = @target_emp_id
)
SELECT 
    edi.day_date AS "Дата",
    
    -- 1. БЛОК ПО ГРАФИКУ (ПЛАН)
    COALESCE(sd_plan.name, 'Выходной') AS "План_Смена",
    COALESCE(edi.default_eq_name, 'Участок') AS "План_Станок",

    -- 2. БЛОК НА СОГЛАСОВАНИИ (ЧЕРНОВИКИ)
    CASE WHEN ovr.id IS NOT NULL AND ovr.status < 2 THEN sd_ovr.name ELSE '---' END AS "Черновик_Смена",
    CASE WHEN ovr.id IS NOT NULL AND ovr.status < 2 THEN (SELECT name FROM equipment WHERE id = ovr.equipment_id) ELSE '---' END AS "Черновик_Станок",

    -- 3. БЛОК ФАКТИЧЕСКИЙ (УТВЕРЖДЕНО)
    CASE 
        WHEN abt.id IS NOT NULL THEN 'ОТСУТСТВИЕ'
        WHEN ovr.status = 2 THEN sd_ovr.name 
        ELSE COALESCE(sd_plan.name, 'Выходной') 
    END AS "Факт_Смена",
    CASE 
        WHEN abt.id IS NOT NULL THEN '---'
        WHEN ovr.status = 2 THEN (SELECT name FROM equipment WHERE id = ovr.equipment_id)
        ELSE COALESCE(edi.default_eq_name, 'Участок')
    END AS "Факт_Станок",

    -- 4. ПОДРОБНОСТИ (ОПИСАНИЕ ДЕЙСТВИЙ)
    CASE 
        WHEN abt.id IS NOT NULL THEN CONCAT('❌ ОТСУТСТВИЕ: ', abt.name)
        WHEN ovr.id IS NOT NULL THEN 
            CONCAT(
                CASE WHEN ovr.status < 2 THEN '⏳ Предложено перемещение: ' ELSE '✅ Утверждено: ' END,
                sd_ovr.name, ' на ', (SELECT name FROM equipment WHERE id = ovr.equipment_id),
                ' (было: ', COALESCE(sd_plan.name, 'вых'), ' на ', COALESCE(edi.default_eq_name, 'участке'), ')'
            )
        WHEN sd_plan.shift_number > 0 AND EXISTS (
            SELECT 1 FROM equipment_daily_plan edp 
            WHERE edp.equipment_id = edi.default_eq_id AND edp.plan_date = edi.day_date AND edp.is_cancelled = 1
        ) THEN '🔘 ВНИМАНИЕ: Простой вашего станка в плане'
        WHEN sd_plan.shift_number > 0 THEN '✅ Работа по графику'
        ELSE '☕ Плановый выходной'
    END AS "Статус_и_Действие",
    
    ovr.id AS "ID_Override"

FROM EmployeeDailyInfo edi
-- Связи для расчета дня цикла
LEFT JOIN schedule_templates st ON edi.current_template_id = st.id
LEFT JOIN schedule_cycles sc ON st.cycle_id = sc.id
LEFT JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
    AND sci.day_number = ( ((DATEDIFF(edi.day_date, st.base_date) % sc.cycle_length) + sc.cycle_length) % sc.cycle_length + 1 )
LEFT JOIN shift_definitions sd_plan ON sci.shift_id = sd_plan.id

-- Оперативные правки
LEFT JOIN schedule_overrides ovr ON ovr.employee_id = edi.emp_id AND ovr.override_date = edi.day_date
LEFT JOIN shift_definitions sd_ovr ON ovr.shift_id = sd_ovr.id

-- Отсутствия
LEFT JOIN absences abs ON abs.employee_id = edi.emp_id 
    AND edi.day_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')
LEFT JOIN absence_types abt ON abs.type_id = abt.id

ORDER BY edi.day_date;
