SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';
SET @target_emp_id = 4;

WITH RECURSIVE calendar AS (
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < @end_date
),

-- 1. СЕТКА ВСЕХ РАБОЧИХ СМЕН ЗАВОДА
ActiveShifts AS (
    SELECT id, shift_number, name FROM shift_definitions WHERE shift_number > 0
),

-- 2. СРЕЗ СОСТОЯНИЙ (План и Оверрайды)
ShiftAnalysis AS (
    SELECT 
        c.day_date,
        s.id AS shift_id,
        s.shift_number,
        s.name AS shift_name,
        
        -- Проверка отсутствия (на дату)
        (SELECT abt.name FROM absences abs JOIN absence_types abt ON abs.type_id = abt.id 
         WHERE abs.employee_id = @target_emp_id AND c.day_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31') LIMIT 1) AS abs_reason,

        -- План
        EXISTS (
            SELECT 1 FROM employee_schedule_assignments esa
            JOIN schedule_templates st ON esa.template_id = st.id
            JOIN schedule_cycles sc ON st.cycle_id = sc.id
            JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id
            WHERE esa.employee_id = @target_emp_id AND esa.valid_from <= c.day_date
              AND sci.shift_id = s.id
              AND sci.day_number = (((DATEDIFF(c.day_date, st.base_date) % sc.cycle_length) + sc.cycle_length) % sc.cycle_length + 1)
            ORDER BY esa.valid_from DESC LIMIT 1
        ) AS is_plan,

        (SELECT eq.name FROM employee_equipment_assignments eqa 
         JOIN equipment eq ON eqa.equipment_id = eq.id
         WHERE eqa.employee_id = @target_emp_id AND eqa.valid_from <= c.day_date 
         ORDER BY eqa.valid_from DESC LIMIT 1) AS default_eq_name,

        -- Оверрайды (с индексом ID)
        (SELECT ov.id FROM schedule_overrides ov WHERE ov.employee_id = @target_emp_id AND ov.override_date = c.day_date AND ov.shift_id = s.id LIMIT 1) AS ovr_id,
        (SELECT ov.status FROM schedule_overrides ov WHERE ov.employee_id = @target_emp_id AND ov.override_date = c.day_date AND ov.shift_id = s.id LIMIT 1) AS ovr_status,
        (SELECT ov.is_cancellation FROM schedule_overrides ov WHERE ov.employee_id = @target_emp_id AND ov.override_date = c.day_date AND ov.shift_id = s.id LIMIT 1) AS ovr_is_cancel,
        (SELECT eq.name FROM schedule_overrides ov JOIN equipment eq ON ov.equipment_id = eq.id 
         WHERE ov.employee_id = @target_emp_id AND ov.override_date = c.day_date AND ov.shift_id = s.id LIMIT 1) AS ovr_eq_name
    FROM calendar c
    CROSS JOIN ActiveShifts s
)

-- 3. ИТОГОВЫЙ ВЫВОД
SELECT 
    f_date AS "Дата", 
    f_shift AS "Смена", 
    f_plan AS "План_Станок", 
    f_draft AS "Черновик", 
    f_fact AS "Утвержденный_Факт", 
    f_type AS "Тип",
    f_ovr_id AS "ID_Override"
FROM (
    -- КАТЕГОРИЯ А: Ручные назначения (Приоритет №1)
    SELECT 
        sa.day_date AS f_date,
        sa.shift_name AS f_shift,
        IF(sa.is_plan, sa.default_eq_name, '---') AS f_plan,
        CASE 
            WHEN sa.ovr_status < 2 AND sa.ovr_is_cancel = 1 THEN '🛑 План отмены'
            WHEN sa.ovr_status < 2 AND sa.ovr_eq_name IS NOT NULL THEN CONCAT('📝 ', sa.ovr_eq_name)
            ELSE '---'
        END AS f_draft,
        CASE 
            WHEN sa.ovr_status = 2 AND sa.ovr_is_cancel = 1 THEN '🚫 Смена отменена'
            WHEN sa.ovr_status = 2 AND sa.ovr_eq_name IS NOT NULL THEN CONCAT('✅ ', sa.ovr_eq_name)
            ELSE '---'
        END AS f_fact,
        CASE 
            WHEN sa.ovr_status = 2 AND sa.ovr_is_cancel = 0 AND sa.abs_reason IS NOT NULL THEN '⚠️ Работа (в отсутствие)'
            WHEN sa.ovr_status = 2 THEN 'Ручная правка'
            ELSE 'В ожидании'
        END AS f_type,
        sa.ovr_id AS f_ovr_id,
        sa.shift_number AS sort_shift_num
    FROM ShiftAnalysis sa
    WHERE sa.ovr_status IS NOT NULL

    UNION ALL

    -- КАТЕГОРИЯ Б: Отсутствия (если на дату нет НИ ОДНОЙ ручной правки)
    SELECT 
        c.day_date AS f_date,
        'ВЕСЬ ДЕНЬ' AS f_shift,
        '---' AS f_plan,
        '---' AS f_draft,
        CONCAT('❌ ', abt.name) AS f_fact,
        'Отсутствие' AS f_type,
        NULL AS f_ovr_id,
        0 AS sort_shift_num
    FROM calendar c
    JOIN absences abs ON abs.employee_id = @target_emp_id 
        AND c.day_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')
    JOIN absence_types abt ON abs.type_id = abt.id
    WHERE NOT EXISTS (SELECT 1 FROM schedule_overrides ov WHERE ov.employee_id = @target_emp_id AND ov.override_date = c.day_date)

    UNION ALL

    -- КАТЕГОРИЯ В: Обычный график (если нет ни правок, ни отсутствия)
    SELECT 
        sa.day_date AS f_date,
        sa.shift_name AS f_shift,
        sa.default_eq_name AS f_plan,
        '---' AS f_draft,
        CONCAT('⚙️ ', sa.default_eq_name) AS f_fact,
        'График' AS f_type,
        NULL AS f_ovr_id,
        sa.shift_number AS sort_shift_num
    FROM ShiftAnalysis sa
    WHERE sa.is_plan = 1 
      AND sa.ovr_status IS NULL 
      AND sa.abs_reason IS NULL
) final_report
ORDER BY f_date ASC, sort_shift_num ASC;
