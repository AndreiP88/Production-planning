SET @target_date = '2026-05-07';
SET @target_shift_num = 1;

WITH EmployeeStatus AS (
    SELECT 
        e.id, 
        e.full_name,
        -- 1. Актуальная должность
        (SELECT p.name FROM employee_position_assignments epa_inner 
         JOIN positions p ON epa_inner.position_id = p.id 
         WHERE epa_inner.employee_id = e.id AND epa_inner.valid_from <= @target_date 
         ORDER BY epa_inner.valid_from DESC LIMIT 1) AS position_name,

        -- 2. Плановая смена
        (SELECT sd_e.shift_number FROM employee_schedule_assignments esa 
         JOIN schedule_templates st_e ON esa.template_id = st_e.id
         JOIN schedule_cycles sc_e ON st_e.cycle_id = sc_e.id
         JOIN schedule_cycle_items sci_e ON sci_e.cycle_id = sc_e.id 
         JOIN shift_definitions sd_e ON sci_e.shift_id = sd_e.id
         WHERE esa.employee_id = e.id AND esa.valid_from <= @target_date 
           AND sci_e.day_number = (MOD(DATEDIFF(@target_date, st_e.base_date) % sc_e.cycle_length + sc_e.cycle_length, sc_e.cycle_length) + 1)
         ORDER BY esa.valid_from DESC LIMIT 1) AS plan_shift_num,
         
        -- 3. Закрепленный станок
        (SELECT eqa.equipment_id FROM employee_equipment_assignments eqa 
         WHERE eqa.employee_id = e.id AND eqa.valid_from <= @target_date 
         ORDER BY eqa.valid_from DESC LIMIT 1) AS default_eq_id,

        -- 4. Причина отсутствия
        (SELECT abt.name FROM absences abs 
         JOIN absence_types abt ON abs.type_id = abt.id 
         WHERE abs.employee_id = e.id 
           AND @target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31') 
         LIMIT 1) AS abs_reason,
                
        -- 5. Флаг явной отмены плана
        EXISTS (SELECT 1 FROM schedule_overrides ovr_c 
                WHERE ovr_c.employee_id = e.id AND ovr_c.override_date = @target_date 
                AND ovr_c.status = 2 AND ovr_c.is_cancellation = 1) AS is_plan_cancelled,

        -- 6. Информация о ручном назначении (даже если болеет)
        (SELECT ovr.id FROM schedule_overrides ovr 
         WHERE ovr.employee_id = e.id AND ovr.override_date = @target_date 
           AND ovr.status = 2 AND ovr.is_cancellation = 0 LIMIT 1) AS override_id,

        (SELECT CONCAT(eq_ovr.name, ' (смена ', sd_ovr.shift_number, ')') 
         FROM schedule_overrides ovr 
         JOIN equipment eq_ovr ON ovr.equipment_id = eq_ovr.id
         JOIN shift_definitions sd_ovr ON ovr.shift_id = sd_ovr.id
         WHERE ovr.employee_id = e.id AND ovr.override_date = @target_date 
           AND ovr.status = 2 AND ovr.is_cancellation = 0 LIMIT 1) AS current_manual_work
    FROM employees e
    JOIN employment_periods ep ON ep.employee_id = e.id 
        AND (@target_date BETWEEN ep.hire_date AND COALESCE(ep.fire_date, '2099-12-31'))
)
SELECT 
    es.id AS "ID_Сотр",
    es.full_name AS "Сотрудник",
    es.position_name AS "Должность",
    COALESCE(CAST(es.plan_shift_num AS CHAR), 'Вых') AS "Смена_План",
    
    CASE 
        -- ПРИОРИТЕТ 1: Если назначен вручную, показываем КУДА (даже если есть больничный)
        WHEN es.current_manual_work IS NOT NULL THEN CONCAT('📌 НАЗНАЧЕН: ', es.current_manual_work)
        
        -- ПРИОРИТЕТ 2: Если правок нет, показываем причину отсутствия
        WHEN es.abs_reason IS NOT NULL THEN CONCAT('❌ ', es.abs_reason)
        
        -- ПРИОРИТЕТ 3: Свободные (отмена или простой станка)
        WHEN es.is_plan_cancelled THEN '🟢 СВОБОДЕН (Смена отменена)'
        WHEN es.plan_shift_num = @target_shift_num AND (
            EXISTS (SELECT 1 FROM equipment_daily_plan edp WHERE edp.equipment_id = es.default_eq_id AND edp.plan_date = @target_date AND edp.is_cancelled = 1)
            OR
            ((SELECT staffing_mode FROM equipment WHERE id = es.default_eq_id) = 'manual_only' AND NOT EXISTS (SELECT 1 FROM equipment_daily_plan WHERE equipment_id = es.default_eq_id AND plan_date = @target_date))
        ) THEN '🟢 СВОБОДЕН (Простой станка)'
        
        -- ПРИОРИТЕТ 4: Занят по графику
        WHEN es.plan_shift_num = @target_shift_num THEN '⚙️ ЗАНЯТ (Свой станок)'
        ELSE '---'
    END AS "Статус",
    
    es.override_id AS "ID_Override",
    
    CASE 
        -- Если уже работает в искомую смену (хоть по плану, хоть по подмене) - приоритет 5
        WHEN es.current_manual_work LIKE CONCAT('%(смена ', @target_shift_num, ')%') OR (es.plan_shift_num = @target_shift_num AND es.abs_reason IS NULL AND NOT es.is_plan_cancelled) THEN 5
        -- Если на больничном и НЕ назначен - приоритет 5
        WHEN es.abs_reason IS NOT NULL AND es.current_manual_work IS NULL THEN 5
        -- Самые свободные
        WHEN es.is_plan_cancelled OR (es.plan_shift_num = @target_shift_num AND es.current_manual_work IS NULL AND es.abs_reason IS NULL) THEN 1
        WHEN es.plan_shift_num IS NULL OR es.plan_shift_num = 0 THEN 2 
        WHEN es.plan_shift_num != @target_shift_num THEN 3 
        ELSE 4 
    END AS priority_order

FROM EmployeeStatus es
ORDER BY priority_order ASC, es.plan_shift_num ASC, es.full_name ASC;
