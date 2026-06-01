-- ============================================================================
-- ПАРАМЕТРЫ ПОИСКА (Входные данные для карточки смены)
-- ============================================================================
SET @target_date = '2026-05-01';
SET @target_shift_number = 2;
SET @target_equipment_id = 1;

-- ============================================================================
-- ГЕНЕРАЦИЯ ДАННЫХ
-- ============================================================================
WITH CurrentEquipmentSettings AS (
    -- Получаем настройки станка, актуальные на выбранную дату
    SELECT 
        e.id,
        e.name,
        -- Актуальный режим из истории (если нет истории, берем текущий из таблицы equipment)
        COALESCE(
            (SELECT staffing_mode FROM equipment_staffing_history 
             WHERE equipment_id = e.id AND valid_from <= @target_date 
             ORDER BY valid_from DESC LIMIT 1),
            e.staffing_mode
        ) AS active_staffing_mode,
        -- Актуальный график из истории
        COALESCE(
            (SELECT template_id FROM equipment_schedule_history 
             WHERE equipment_id = e.id AND valid_from <= @target_date 
             ORDER BY valid_from DESC LIMIT 1),
            e.template_id
        ) AS active_template_id
    FROM equipment e
    WHERE e.id = @target_equipment_id
),

EmployeeStatus AS (
    -- Собираем всех, кто имеет отношение к этому станку в эту смену
    SELECT 
        e.id AS employee_id,
        e.full_name,
        -- Флаг: должен ли быть здесь по своему графику и закреплению
        (eqa.equipment_id = @target_equipment_id AND sd.shift_number = @target_shift_number) as is_planned_here,
        -- Флаг: назначен ли сюда вручную (override)
        (ovr.equipment_id = @target_equipment_id AND ovr_sd.shift_number = @target_shift_number) as is_overridden_here,
        -- Причина отсутствия
        (SELECT abt.name FROM absences abs 
         JOIN absence_types abt ON abs.type_id = abt.id 
         WHERE abs.employee_id = e.id 
           AND @target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31') 
         LIMIT 1) AS absence_reason,
        -- Информация о переводах
        ovr.equipment_id as current_ovr_eq_id,
        (SELECT name FROM equipment WHERE id = ovr.equipment_id) AS moved_to_equipment
    FROM employees e
    -- Актуальный график сотрудника
    JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
        AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= @target_date)
    JOIN schedule_templates st ON esa.template_id = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (MOD(DATEDIFF(@target_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
    -- Актуальное закрепление за станком
    LEFT JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
        AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= @target_date)
    -- Оперативные правки (статус 2 = Утверждено)
    LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id AND ovr.override_date = @target_date AND ovr.status = 2
    LEFT JOIN shift_definitions ovr_sd ON ovr.shift_id = ovr_sd.id
),

ActiveWorkforce AS (
    -- Считаем количество реально работающих на месте
    SELECT COUNT(*) as active_count
    FROM EmployeeStatus
    WHERE (is_overridden_here = 1 AND absence_reason IS NULL)
       OR (is_planned_here = 1 AND absence_reason IS NULL AND (current_ovr_eq_id IS NULL OR current_ovr_eq_id = @target_equipment_id))
)

-- ============================================================================
-- ФИНАЛЬНЫЙ ВЫВОД
-- ============================================================================
SELECT 
    ces.name AS "Станок",
    ces.active_staffing_mode AS "Режим_работы",
    
    -- Состояние станка из плана
    CASE 
        WHEN edp.is_cancelled = 1 THEN '🛑 ОСТАНОВЛЕН'
        ELSE '⚙️ В РАБОТЕ'
    END AS "Статус_в_плане",

    -- Динамический индикатор потребности в назначении
    CASE 
        WHEN edp.is_cancelled = 1 THEN 'Люди не нужны (Остановка)'
        WHEN ces.active_staffing_mode = 'manual_only' AND edp.id IS NULL THEN 'Не требуется (Вне плана)'
        WHEN (SELECT active_count FROM ActiveWorkforce) > 0 THEN 'Люди не нужны (ОК)'
        ELSE '⚠️ ТРЕБУЕТСЯ НАЗНАЧЕНИЕ'
    END AS "Нужно_назначить",

    es.employee_id AS "ID_Сотр",
    es.full_name AS "Сотрудник",

    -- Столбец ПЛАНА (что должно быть по графикам)
    CASE 
        WHEN es.is_planned_here = 1 THEN 
            CASE 
                WHEN es.absence_reason IS NOT NULL THEN CONCAT('❌ ', es.absence_reason)
                WHEN es.current_ovr_eq_id IS NOT NULL AND es.current_ovr_eq_id != ces.id 
                    THEN CONCAT('➡️ Переведен на ', es.moved_to_equipment)
                ELSE '✅ В графике'
            END
        ELSE '---'
    END AS "По_плану",

    -- Столбец ФАКТА (кто реально стоит у станка)
    CASE 
        WHEN es.absence_reason IS NULL AND (
            (es.is_overridden_here = 1) OR 
            (es.is_planned_here = 1 AND (es.current_ovr_eq_id IS NULL OR es.current_ovr_eq_id = ces.id))
        ) THEN '✅ РАБОТАЕТ'
        ELSE '---'
    END AS "По_факту"

FROM CurrentEquipmentSettings ces
LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = ces.id 
    AND edp.plan_date = @target_date 
    AND edp.shift_id = (SELECT id FROM shift_definitions WHERE shift_number = @target_shift_number LIMIT 1)
LEFT JOIN EmployeeStatus es ON (es.is_planned_here = 1 OR es.is_overridden_here = 1)
ORDER BY es.is_planned_here DESC, es.full_name ASC;
