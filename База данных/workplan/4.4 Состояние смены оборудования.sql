-- ============================================================================
-- ПАРАМЕТРЫ ПОИСКА
-- ============================================================================
SET @target_date = '2026-05-07';
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
        -- Привязка к смене (ID)
        (SELECT id FROM shift_definitions WHERE shift_number = @target_shift_number LIMIT 1) as target_shift_id,
        
        -- 1. ДАННЫЕ ПО ПЛАНУ
        (eqa.equipment_id = @target_equipment_id AND sd.shift_number = @target_shift_number) as is_planned_here,
        (SELECT abt.name FROM absences abs JOIN absence_types abt ON abs.type_id = abt.id 
         WHERE abs.employee_id = e.id AND @target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31') LIMIT 1) AS abs_reason,
        
        -- 2. ДАННЫЕ ПО ОВЕРРАЙДАМ (Утвержденные или Черновики)
        ovr.id AS ovr_id,
        ovr.status AS ovr_status,
        ovr.is_cancellation AS ovr_is_cancel,
        ovr.equipment_id AS ovr_eq_id
        
    FROM employees e
    -- Расчет планового графика
    JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
        AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= @target_date)
    JOIN schedule_templates st ON esa.template_id = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (MOD(DATEDIFF(@target_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
    LEFT JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
        AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= @target_date)
    -- Подтягиваем любую правку на эту смену (и черновики, и утвержденные)
    LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id AND ovr.override_date = @target_date 
        AND ovr.shift_id = (SELECT id FROM shift_definitions WHERE shift_number = @target_shift_number LIMIT 1)
),

ActiveWorkforce AS (
    -- Подсчет для индикатора потребности (утвержденные оверрайды или план без отмен и болезней)
    SELECT COUNT(*) as active_count
    FROM EmployeeStatus
    WHERE (ovr_status = 2 AND ovr_eq_id = @target_equipment_id AND ovr_is_cancel = 0)
       OR (is_planned_here = 1 AND abs_reason IS NULL AND ovr_status IS NULL)
)

-- ============================================================================
-- ВЫВОД (Карточка смены)
-- ============================================================================
SELECT 
    ces.name AS "Станок",
    
    -- ИНДИКАТОРЫ
    CASE 
        WHEN edp.is_cancelled = 1 THEN 'Не требуется (Остановка)'
        WHEN (SELECT active_count FROM ActiveWorkforce) > 0 THEN '✅ Укомплектовано'
        WHEN ces.active_staffing_mode = 'strict_schedule' THEN '🚨 ТРЕБУЕТСЯ'
        ELSE '⚠️ НУЖНО НАЗНАЧЕНИЕ'
    END AS "Потребность",

    -- 1. БЛОК: ПО ПЛАНУ
    CASE WHEN es.is_planned_here = 1 THEN es.employee_id END AS "ID_План",
    CASE WHEN es.is_planned_here = 1 THEN es.full_name END AS "Сотрудник_План",
    CASE WHEN es.is_planned_here = 1 THEN COALESCE(es.abs_reason, '✅ В графике') END AS "Статус_План",

    -- 2. БЛОК: НА СОГЛАСОВАНИИ (ЧЕРНОВИКИ)
    CASE WHEN es.ovr_status < 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id THEN es.ovr_id END AS "ID_Черновик",
    CASE WHEN es.ovr_status < 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id THEN es.full_name END AS "Сотрудник_Черновик",

    -- 3. БЛОК: НАЗНАЧЕНО (УТВЕРЖДЕНО)
    CASE WHEN es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id THEN es.ovr_id END AS "ID_Назначения",
    CASE WHEN es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id THEN es.full_name END AS "Сотрудник_Назначен",

    -- ФАКТИЧЕСКИЙ РЕЗУЛЬТАТ (КТО РАБОТАЕТ)
    CASE 
        WHEN (es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id)
          OR (es.is_planned_here = 1 AND es.abs_reason IS NULL AND es.ovr_status IS NULL) 
        THEN '✅ РАБОТАЕТ'
        ELSE '---'
    END AS "Итоговый_Факт"

FROM CurrentEquipmentSettings ces
LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = ces.id 
    AND edp.plan_date = @target_date 
    AND edp.shift_id = (SELECT id FROM shift_definitions WHERE shift_number = @target_shift_number LIMIT 1)
LEFT JOIN EmployeeStatus es ON (es.is_planned_here = 1 OR es.ovr_id IS NOT NULL)
ORDER BY es.is_planned_here DESC, es.full_name ASC;
