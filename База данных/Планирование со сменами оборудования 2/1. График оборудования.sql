SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';
SET @target_area_id = 1;

WITH RECURSIVE calendar AS (
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < @end_date
),
EquipmentDailyNeeds AS (
    SELECT 
        c.day_date,
        eq.id AS eq_id,
        eq.name AS eq_name,
        sd.id AS plan_shift_id,
        sd.name AS plan_shift_name,
        sd.shift_number AS plan_shift_num
    FROM calendar c
    CROSS JOIN equipment eq
    JOIN schedule_templates st ON eq.template_id = st.id
    JOIN schedule_items si ON si.template_id = st.id 
        AND si.day_number = (MOD(DATEDIFF(c.day_date, st.base_date), st.cycle_length) + 1)
    JOIN shift_definitions sd ON si.shift_id = sd.id
    WHERE eq.work_area_id = @target_area_id
      AND sd.shift_number > 0 
      AND c.day_date >= eq.commissioned_at 
      AND (eq.decommissioned_at IS NULL OR c.day_date <= eq.decommissioned_at)
)
SELECT 
    edn.day_date AS "Дата",
    edn.eq_name AS "Оборудование",
    edn.plan_shift_name AS "Смена_Станка",
    
    -- ИЩЕМ ИСПОЛНИТЕЛЯ
    (
        SELECT e.full_name 
        FROM employees e
        -- Соединяем с личными правками сотрудника
        LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id 
            AND ovr.override_date = edn.day_date 
            AND ovr.status = 2
        -- Соединяем с описанием смены из ПРАВКИ (чтобы проверить номер смены)
        LEFT JOIN shift_definitions sd_ovr ON ovr.shift_id = sd_ovr.id
        
        -- Соединяем с плановым графиком сотрудника
        JOIN employee_schedule_assignments esa ON esa.employee_id = e.id
            AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= edn.day_date)
        JOIN schedule_templates st_emp ON esa.template_id = st_emp.id
        JOIN schedule_items si_emp ON si_emp.template_id = st_emp.id 
            AND si_emp.day_number = (MOD(DATEDIFF(edn.day_date, st_emp.base_date), st_emp.cycle_length) + 1)
        JOIN shift_definitions sd_emp ON si_emp.shift_id = sd_emp.id
        
        -- Условие закрепления за оборудованием
        JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id
            AND eqa.valid_from <= edn.day_date

        WHERE (
            -- ВАРИАНТ 1: Ручное назначение на этот станок ИМЕННО В ЭТУ СМЕНУ
            (ovr.equipment_id = edn.eq_id AND sd_ovr.shift_number = edn.plan_shift_num)
            OR 
            -- ВАРИАНТ 2: Плановый сотрудник в свою смену (если на этот день нет правок)
            (eqa.equipment_id = edn.eq_id AND sd_emp.shift_number = edn.plan_shift_num AND ovr.id IS NULL)
        )
        -- Проверка отсутствия (болезнь/отпуск)
        AND NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id 
                        AND edn.day_date >= abs.start_date AND (abs.end_date IS NULL OR edn.day_date <= abs.end_date))
        
        ORDER BY ovr.id DESC, eqa.valid_from DESC LIMIT 1
    ) AS "Кто_на_смене",

    -- СТАТУС
    CASE 
        WHEN EXISTS (SELECT 1 FROM equipment_daily_plan edp 
                     WHERE edp.equipment_id = edn.eq_id AND edp.plan_date = edn.day_date 
                     AND edp.shift_id = edn.plan_shift_id AND edp.is_cancelled = TRUE) 
             THEN '🛑 ОТМЕНЕНО'

        -- Проверка: назначен ли кто-то в эту конкретную смену
        WHEN (
            SELECT COUNT(*) 
            FROM employees e
            LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id AND ovr.override_date = edn.day_date AND ovr.status = 2
            LEFT JOIN shift_definitions sd_ovr ON ovr.shift_id = sd_ovr.id
            JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id
            JOIN employee_schedule_assignments esa ON esa.employee_id = e.id
                AND esa.valid_from = (SELECT MAX(v) FROM (SELECT valid_from as v, employee_id as eid FROM employee_schedule_assignments) as t WHERE eid = e.id AND v <= edn.day_date)
            JOIN schedule_items si_emp ON si_emp.template_id = esa.template_id 
                AND si_emp.day_number = (MOD(DATEDIFF(edn.day_date, (SELECT base_date FROM schedule_templates WHERE id = esa.template_id)), (SELECT cycle_length FROM schedule_templates WHERE id = esa.template_id)) + 1)
            JOIN shift_definitions sd_emp ON si_emp.shift_id = sd_emp.id
            WHERE (
                (ovr.equipment_id = edn.eq_id AND sd_ovr.shift_number = edn.plan_shift_num)
                OR 
                (eqa.equipment_id = edn.eq_id AND sd_emp.shift_number = edn.plan_shift_num AND ovr.id IS NULL)
            )
            AND NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id 
                            AND edn.day_date >= abs.start_date AND (abs.end_date IS NULL OR edn.day_date <= abs.end_date))
        ) > 0 THEN '✅ ОК'

        ELSE '⚠️ ALERT: НЕТ ПЕРСОНАЛА'
    END AS "Состояние"

FROM EquipmentDailyNeeds edn
ORDER BY edn.day_date, edn.eq_name, edn.plan_shift_id;
