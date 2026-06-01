SET @target_date = '2026-05-07';
SET @target_shift_id = 1;

WITH TargetShift AS (
    SELECT id, shift_number, start_time, end_time, name as target_shift_name,
           CAST(CONCAT(@target_date, ' ', start_time) AS DATETIME) as target_start_dt,
           CAST(CONCAT(IF(end_time < start_time, @target_date + INTERVAL 1 DAY, @target_date), ' ', end_time) AS DATETIME) as target_end_dt
    FROM shift_definitions WHERE shift_number = @target_shift_id
),
AllEmployeeActivities AS (
    -- 1. ПЛАНОВАЯ РАБОТА
    SELECT 
        e.id as emp_id, sd.id as shift_id, sd.shift_number, sd.name as shift_name, 
        eq.name as eq_name, NULL as ovr_id, 'План' as source_type,
        c.day_date as cal_date,
        CAST(CONCAT(c.day_date, ' ', sd.start_time) AS DATETIME) as start_dt,
        CAST(CONCAT(IF(sd.end_time < sd.start_time, c.day_date + INTERVAL 1 DAY, c.day_date), ' ', sd.end_time) AS DATETIME) as end_dt
    FROM (SELECT CAST(@target_date - INTERVAL 1 DAY AS DATE) as day_date UNION SELECT CAST(@target_date AS DATE) UNION SELECT CAST(@target_date + INTERVAL 1 DAY AS DATE)) c
    CROSS JOIN employees e
    JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
        AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    JOIN schedule_cycle_items sci ON sci.cycle_id = (SELECT cycle_id FROM schedule_templates WHERE id = esa.template_id)
        AND sci.day_number = (MOD(DATEDIFF(c.day_date, (SELECT base_date FROM schedule_templates WHERE id = esa.template_id)) % (SELECT cycle_length FROM schedule_cycles sc JOIN schedule_templates st ON sc.id = st.cycle_id WHERE st.id = esa.template_id) + (SELECT cycle_length FROM schedule_cycles sc JOIN schedule_templates st ON sc.id = st.cycle_id WHERE st.id = esa.template_id), (SELECT cycle_length FROM schedule_cycles sc JOIN schedule_templates st ON sc.id = st.cycle_id WHERE st.id = esa.template_id)) + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
    JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
        AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    JOIN equipment eq ON eqa.equipment_id = eq.id
    WHERE sd.shift_number > 0
      AND NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND c.day_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31'))
      AND NOT EXISTS (SELECT 1 FROM schedule_overrides ovr WHERE ovr.employee_id = e.id AND ovr.override_date = c.day_date AND ovr.shift_id = sd.id AND ovr.status = 2)

    UNION ALL

    -- 2. РУЧНЫЕ НАЗНАЧЕНИЯ
    SELECT 
        ovr.employee_id, ovr.shift_id, sd_o.shift_number, sd_o.name, 
        eq_o.name, ovr.id, 'Назначение',
        ovr.override_date as cal_date,
        CAST(CONCAT(ovr.override_date, ' ', sd_o.start_time) AS DATETIME),
        CAST(CONCAT(IF(sd_o.end_time < sd_o.start_time, ovr.override_date + INTERVAL 1 DAY, ovr.override_date), ' ', sd_o.end_time) AS DATETIME)
    FROM schedule_overrides ovr
    JOIN shift_definitions sd_o ON ovr.shift_id = sd_o.id
    JOIN equipment eq_o ON ovr.equipment_id = eq_o.id
    WHERE ovr.override_date BETWEEN @target_date - INTERVAL 1 DAY AND @target_date + INTERVAL 1 DAY
      AND ovr.status = 2 AND ovr.is_cancellation = 0 
      AND sd_o.shift_number > 0
)
SELECT 
    e.id AS "ID_Сотр",
    e.full_name AS "Сотрудник",

    (SELECT a.ovr_id FROM AllEmployeeActivities a, TargetShift ts 
     WHERE a.emp_id = e.id AND a.shift_id = ts.id AND a.start_dt = ts.target_start_dt AND a.ovr_id IS NOT NULL LIMIT 1) AS "ID_Override",

    (SELECT CONCAT(IF(a.source_type = 'План', 'План: ', 'Назначение: '), a.shift_name, ' (', a.eq_name, ')') 
     FROM AllEmployeeActivities a, TargetShift ts 
     WHERE a.emp_id = e.id AND a.shift_id = ts.id AND a.start_dt = ts.target_start_dt LIMIT 1) AS "Текущая_активность",

    (SELECT CONCAT(DATE_FORMAT(a.cal_date, '%d.%m.%Y'), ' ', a.source_type, ': ', a.shift_name, ' (', a.eq_name, ')') 
     FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8 ORDER BY a.end_dt DESC LIMIT 1) AS "Смежная_ДО",

    (SELECT CONCAT(DATE_FORMAT(a.cal_date, '%d.%m.%Y'), ' ', a.source_type, ': ', a.shift_name, ' (', a.eq_name, ')') 
     FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8 ORDER BY a.start_dt ASC LIMIT 1) AS "Смежная_ПОСЛЕ",

    CASE 
        WHEN EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND @target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31'))
            THEN (SELECT 
                    CASE 
                        WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.shift_id = ts.id AND a.start_dt = ts.target_start_dt AND a.source_type = 'Назн')
                        THEN CONCAT('⚠️ РАБОТАЕТ ПРИ: ', abt.name)
                        ELSE CONCAT('❌ ', abt.name)
                    END
                  FROM absences abs2 JOIN absence_types abt ON abs2.type_id = abt.id WHERE abs2.employee_id = e.id AND @target_date BETWEEN abs2.start_date AND COALESCE(abs2.end_date, '2099-12-31') LIMIT 1)
        WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.shift_id = ts.id AND a.start_dt = ts.target_start_dt) THEN '⛔ ЗАНЯТ'
        WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND ((a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8) OR (a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8))) THEN '🟡 ДОСТУПЕН (смежные)'
        ELSE '🟢 ДОСТУПЕН'
    END AS "Текущий_статус"

FROM employees e
WHERE EXISTS (SELECT 1 FROM employment_periods ep WHERE ep.employee_id = e.id AND @target_date BETWEEN ep.hire_date AND COALESCE(ep.fire_date, '2099-12-31'))

ORDER BY 
    -- ПРЯМОЙ ПОВТОР ЛОГИКИ ДЛЯ ЖЕСТКОЙ СОРТИРОВКИ
    (CASE 
        -- 1. Сначала полностью свободные
        WHEN NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND @target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31'))
             AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.shift_id = ts.id AND a.start_dt = ts.target_start_dt)
             AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND ((a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8) OR (a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8)))
             THEN 1
        -- 2. Свободные, но со смежными сменами
        WHEN NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND @target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31'))
             AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.shift_id = ts.id AND a.start_dt = ts.target_start_dt)
             THEN 2
        -- 3. Занятые (уже работают)
        WHEN NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND @target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31'))
             THEN 3
        -- 4. Те, кто выведен во время отпуска/больничного
        WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.shift_id = ts.id AND a.start_dt = ts.target_start_dt AND a.source_type = 'Назн')
             THEN 4
        -- 5. Просто отсутствующие (болеют/отпуск)
        ELSE 5
    END) ASC, 
    e.full_name ASC;
