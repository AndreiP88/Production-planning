-- ============================================================================
-- ПАРАМЕТРЫ ПОИСКА
-- ============================================================================
SET @target_date = '2026-05-01';
SET @target_shift_number = 1;
SET @target_equipment_id = 2;

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
        ) AS active_staffing_mode,
        
        -- Получаем ID и временные рамки целевой смены
        sd_target.id AS target_shift_id,
        sd_target.name AS target_shift_name,
        sd_target.start_time AS target_start_time, -- Время начала
        sd_target.end_time AS target_end_time,     -- Время окончания
        
        -- Проверяем, должен ли станок работать в эту смену по своему графику
        EXISTS (
            SELECT 1 
            FROM schedule_templates st
            JOIN schedule_cycles sc ON st.cycle_id = sc.id
            JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id
            WHERE st.id = e.template_id
              AND sci.shift_id = sd_target.id
              AND sci.day_number = (MOD(DATEDIFF(@target_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
        ) AS is_equipment_working_by_plan
    FROM equipment e
    -- Подтягиваем справочник смен прямо в настройки оборудования
    CROSS JOIN (SELECT id, name, start_time, end_time FROM shift_definitions WHERE shift_number = @target_shift_number LIMIT 1) sd_target
    WHERE e.id = @target_equipment_id
),

EmployeeStatus AS (
    SELECT 
        e.id AS employee_id,
        e.full_name,
        ces.target_shift_id,
        
        -- 1. ДАННЫЕ ПО ПЛАНУ СОТРУДНИКА
        (eqa.equipment_id = @target_equipment_id AND sd_plan.shift_number = @target_shift_number) as is_planned_here,
        
        -- Причина отсутствия
        (SELECT abt.name FROM absences abs 
         JOIN absence_types abt ON abs.type_id = abt.id 
         WHERE abs.employee_id = e.id 
           AND @target_date >= abs.start_date 
           AND (abs.end_date IS NULL OR @target_date <= abs.end_date) 
         LIMIT 1) AS abs_reason,
        
        -- 2. ДАННЫЕ ПО ОВЕРРАЙДАМ (Строго в рамках целевой смены)
        ovr.id AS ovr_id,
        ovr.status AS ovr_status,
        ovr.is_cancellation AS ovr_is_cancel,
        ovr.equipment_id AS ovr_eq_id,
        ovr.comment AS ovr_comment
        
    FROM employees e
    CROSS JOIN CurrentEquipmentSettings ces
    JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
        AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= @target_date)
    JOIN schedule_templates st ON esa.template_id = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (MOD(DATEDIFF(@target_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
    JOIN shift_definitions sd_plan ON sci.shift_id = sd_plan.id
    LEFT JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
        AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= @target_date)
    
    LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id 
        AND ovr.override_date = @target_date 
        AND ovr.shift_id = ces.target_shift_id
    
    WHERE 
        (eqa.equipment_id = @target_equipment_id AND sd_plan.shift_number = @target_shift_number)
        OR (ovr.equipment_id = @target_equipment_id AND ovr.id IS NOT NULL)
),

ActiveWorkforce AS (
    SELECT COUNT(*) as active_count
    FROM EmployeeStatus
    WHERE (ovr_status = 2 AND ovr_eq_id = @target_equipment_id AND ovr_is_cancel = 0)
       OR (is_planned_here = 1 AND abs_reason IS NULL AND (ovr_status IS NULL OR ovr_status != 2 OR (ovr_is_cancel = 0 AND ovr_eq_id = @target_equipment_id)))
)

-- ============================================================================
-- ВЫВОД (Карточка смены)
-- ============================================================================
SELECT 
    ces.name AS "Станок",
    ces.target_shift_name AS "Название_смены",
    
    -- НОВЫЕ СТОЛБЦЫ: ВРЕМЕННЫЕ ИНТЕРВАЛЫ СМЕНЫ
    DATE_FORMAT(ces.target_start_time, '%H:%i') AS "Время_начало",
    DATE_FORMAT(ces.target_end_time, '%H:%i') AS "Время_окончание",
    
    -- ИНДИКАТОР ПОТРЕБНОСТИ
    CASE 
        WHEN edp.is_cancelled = 1 THEN 'Не требуется (Остановка станка)'
        WHEN (SELECT active_count FROM ActiveWorkforce) > 0 THEN '✅ Укомплектовано'
        WHEN ces.is_equipment_working_by_plan = 0 THEN 'Не требуется (Вне графика)'
        WHEN ces.active_staffing_mode = 'manual_only' THEN '⚪ Ожидание назначения'
        ELSE '🚨 ТРЕБУЕТСЯ ПЕРСОНАЛ'
    END AS "Потребность",

    -- 1. БЛОК: ПО ПЛАНУ
    CASE WHEN es.is_planned_here = 1 THEN es.employee_id END AS "ID_План",
    CASE WHEN es.is_planned_here = 1 THEN es.full_name END AS "Сотрудник_План",
    CASE 
        WHEN es.is_planned_here = 1 AND es.abs_reason IS NOT NULL THEN CONCAT('❌ ', es.abs_reason)
        WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_is_cancel = 1 
            THEN CONCAT('🛑 ОТМЕНЕН (', COALESCE(es.ovr_comment, 'Причина не указана'), ')')
        WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_eq_id != @target_equipment_id 
            THEN CONCAT('🔄 ПЕРЕВЕДЕН на ', (SELECT name FROM equipment WHERE id = es.ovr_eq_id), 
                        IF(es.ovr_comment IS NOT NULL, CONCAT(' [', es.ovr_comment, ']'), ''))
        WHEN es.is_planned_here = 1 THEN '✅ В графике' 
    END AS "Статус_План",

    -- 2. БЛОК: НА СОГЛАСОВАНИИ (ЧЕРНОВИКИ)
    CASE WHEN es.ovr_status < 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id THEN es.ovr_id END AS "ID_Черновик",
    CASE WHEN es.ovr_status < 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id THEN es.full_name END AS "Сотрудник_Черновик",

    -- 3. БЛОК: НАЗНАЧЕНО (УТВЕРЖДЕНО)
    CASE WHEN es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id THEN es.ovr_id END AS "ID_Назначения",
    CASE WHEN es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id THEN es.full_name END AS "Сотрудник_Назначен",

    -- ФАКТИЧЕСКИЙ РЕЗУЛЬТАТ
    CASE 
        WHEN es.abs_reason IS NOT NULL THEN CONCAT('--- (', es.abs_reason, ')')
        WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_is_cancel = 1 THEN '--- (Отмена мастером)'
        WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_eq_id != @target_equipment_id THEN '--- (Переведен)'
        
        WHEN (es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = @target_equipment_id)
          OR (es.is_planned_here = 1 AND es.abs_reason IS NULL AND (es.ovr_status IS NULL OR es.ovr_status != 2)) 
        THEN '✅ РАБОТАЕТ'
        ELSE '---'
    END AS "Итоговый_Fact"

FROM CurrentEquipmentSettings ces
LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = ces.id 
    AND edp.plan_date = @target_date 
    AND edp.shift_id = ces.target_shift_id
LEFT JOIN EmployeeStatus es ON (es.is_planned_here = 1 OR es.ovr_id IS NOT NULL)
ORDER BY es.is_planned_here DESC, es.full_name ASC;
