-- ============================================================================
-- ПАРАМЕТРЫ ПОИСКА
-- ============================================================================
SET @target_date = '2026-05-1';
SET @target_shift_number = 1;
SET @target_equipment_id = 1;

-- ============================================================================
-- ГЕНЕРАЦИЯ ДАННЫХ
-- ============================================================================
WITH CurrentEquipmentSettings AS (
    SELECT 
        e.id, e.name,
        COALESCE(
            (SELECT staffing_mode FROM equipment_staffing_history 
             WHERE equipment_id = e.id AND valid_from <= @target_date 
             ORDER BY valid_from DESC LIMIT 1),
            e.staffing_mode
        ) AS active_staffing_mode
    FROM equipment e
    WHERE e.id = @target_equipment_id
),

EmployeeStatus AS (
    SELECT 
        e.id AS employee_id,
        e.full_name,
        ovr.id AS override_id,
        (eqa.equipment_id = @target_equipment_id AND sd.shift_number = @target_shift_number) as is_planned_here,
        (ovr.equipment_id = @target_equipment_id AND ovr_sd.shift_number = @target_shift_number AND ovr.is_cancellation = 0) as is_overridden_here,
        ovr.is_cancellation as current_is_cancel,
        ovr.status as current_ovr_status,
        (SELECT abt.name FROM absences abs 
         JOIN absence_types abt ON abs.type_id = abt.id 
         WHERE abs.employee_id = e.id 
           AND @target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31') 
         LIMIT 1) AS absence_reason,
        (SELECT name FROM equipment WHERE id = ovr.equipment_id) AS moved_to_equipment,
        ovr.equipment_id as current_ovr_eq_id
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
        AND ovr.shift_id = (SELECT id FROM shift_definitions WHERE shift_number = @target_shift_number LIMIT 1)
    LEFT JOIN shift_definitions ovr_sd ON ovr.shift_id = ovr_sd.id
),

ActiveWorkforce AS (
    -- ИСПРАВЛЕНО: Теперь считаем активным любого, кто имеет оверрайд сюда (игнорируя болезнь)
    -- ИЛИ планового, у которого нет болезни и нет перекрывающих оверрайдов
    SELECT COUNT(*) as active_count
    FROM EmployeeStatus
    WHERE 
        (is_overridden_here = 1) -- Если назначен вручную, он "активен" для этого станка
        OR 
        (is_planned_here = 1 AND absence_reason IS NULL AND current_ovr_status IS NULL)
)

-- ============================================================================
-- ФИНАЛЬНЫЙ ВЫВОД
-- ============================================================================
SELECT 
    ces.name AS "Станок",
    ces.active_staffing_mode AS "Режим_работы",
    
    CASE 
        WHEN edp.is_cancelled = 1 THEN '🛑 ОСТАНОВЛЕН'
        ELSE '⚙️ В РАБОТЕ'
    END AS "Статус_в_плане",

    CASE 
        WHEN edp.is_cancelled = 1 THEN 'Не требуется (Остановка)'
        WHEN ces.active_staffing_mode = 'manual_only' AND edp.id IS NULL THEN 'Не требуется (Вне плана)'
        -- Теперь здесь будет "✅ Укомплектовано", если вы вывели "больного" через оверрайд
        WHEN (SELECT active_count FROM ActiveWorkforce) > 0 THEN '✅ Укомплектовано'
        ELSE '⚠️ ТРЕБУЕТСЯ НАЗНАЧЕНИЕ'
    END AS "Нужно_назначить",

    es.employee_id AS "ID_Сотр",
    es.full_name AS "Сотрудник",
    COALESCE(CAST(es.override_id AS CHAR), '---') AS "ID_Override",

    CASE 
        WHEN es.is_planned_here = 1 THEN 
            CASE 
                WHEN es.absence_reason IS NOT NULL THEN CONCAT('❌ ', es.absence_reason)
                WHEN es.current_is_cancel = 1 THEN '🚫 Отмена смены'
                WHEN es.current_ovr_eq_id IS NOT NULL AND es.current_ovr_eq_id != ces.id 
                    THEN CONCAT('➡️ на ', es.moved_to_equipment)
                ELSE '✅ В графике'
            END
        ELSE '---'
    END AS "По_плану",

    CASE 
        WHEN (
            (es.is_overridden_here = 1) 
            OR 
            (es.is_planned_here = 1 AND es.absence_reason IS NULL AND es.current_ovr_status IS NULL)
        ) THEN '✅ РАБОТАЕТ'
        ELSE '---'
    END AS "По_факту"

FROM CurrentEquipmentSettings ces
LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = ces.id 
    AND edp.plan_date = @target_date 
    AND edp.shift_id = (SELECT id FROM shift_definitions WHERE shift_number = @target_shift_number LIMIT 1)
LEFT JOIN EmployeeStatus es ON (es.is_planned_here = 1 OR es.is_overridden_here = 1)
ORDER BY es.is_planned_here DESC, es.full_name ASC;
