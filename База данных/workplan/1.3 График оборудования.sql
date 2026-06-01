-- ============================================================================
-- ПАРАМЕТРЫ
-- ============================================================================
SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';
SET @target_area_id = 1;

-- ============================================================================
-- ГЕНЕРАЦИЯ ОТЧЕТА
-- ============================================================================
WITH RECURSIVE calendar AS (
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < @end_date
),

-- 1. СЕТКА ОБОРУДОВАНИЯ С УЧЕТОМ ИСТОРИИ НАСТРОЕК
EquipmentGrid AS (
    SELECT 
        c.day_date, 
        eq.id AS eq_id, 
        eq.name AS eq_name,
        wa.sort_order AS area_sort,
        eq.sort_order AS eq_sort,
        -- Актуальный режим работы на каждую дату
        COALESCE(
            (SELECT staffing_mode FROM equipment_staffing_history 
             WHERE equipment_id = eq.id AND valid_from <= c.day_date 
             ORDER BY valid_from DESC LIMIT 1),
            eq.staffing_mode
        ) AS active_staffing_mode,
        -- Актуальный график (цикл) на каждую дату
        COALESCE(
            (SELECT template_id FROM equipment_schedule_history 
             WHERE equipment_id = eq.id AND valid_from <= c.day_date 
             ORDER BY valid_from DESC LIMIT 1),
            eq.template_id
        ) AS active_template_id
    FROM calendar c
    CROSS JOIN equipment eq
    JOIN work_areas wa ON eq.work_area_id = wa.id
    WHERE eq.work_area_id = @target_area_id 
      AND c.day_date BETWEEN eq.commissioned_at AND COALESCE(eq.decommissioned_at, '2099-12-31')
),

-- 2. СРЕЗ СМЕН ОБОРУДОВАНИЯ (накладываем циклы на исторические настройки)
EquipmentShifts AS (
    SELECT 
        eg.*,
        sd.id AS shift_id, sd.name AS shift_name, sd.shift_number,
        COALESCE(edp.is_cancelled, 0) as is_cancelled,
        edp.id as plan_entry_id
    FROM EquipmentGrid eg
    JOIN schedule_templates st ON eg.active_template_id = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (MOD(DATEDIFF(eg.day_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
    LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = eg.eq_id AND edp.plan_date = eg.day_date AND edp.shift_id = sd.id
),

-- 3. СОСТОЯНИЕ СОТРУДНИКОВ (План и Факт)
EmployeeDailyStatus AS (
    SELECT 
        c.day_date,
        e.id AS employee_id,
        e.full_name,
        eqa.equipment_id AS assigned_eq_id,
        sd.shift_number AS planned_shift_number,
        -- Причина отсутствия
        (SELECT abt.name FROM absences abs 
         JOIN absence_types abt ON abs.type_id = abt.id 
         WHERE abs.employee_id = e.id AND c.day_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31') 
         LIMIT 1) AS absence_reason,
        -- Переназначения
        ovr.equipment_id AS override_eq_id,
        ovr_sd.shift_number AS override_shift_number,
        (SELECT name FROM equipment WHERE id = ovr.equipment_id) AS target_eq_name
    FROM calendar c
    CROSS JOIN employees e
    JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
        AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    JOIN schedule_templates st ON esa.template_id = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (MOD(DATEDIFF(c.day_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
    LEFT JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
        AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id AND ovr.override_date = c.day_date AND ovr.status = 2
    LEFT JOIN shift_definitions ovr_sd ON ovr.shift_id = ovr_sd.id
)

-- 4. ФИНАЛЬНЫЙ СУММАРНЫЙ ОТЧЕТ
SELECT 
    es.day_date AS "Дата",
    es.eq_name AS "Оборудование",
    es.shift_name AS "Смена",

    -- ИНДИКАТОР ПОТРЕБНОСТИ (Логика из карточки смены)
    CASE 
        WHEN es.is_cancelled = 1 THEN 'Не требуется (Остановка)'
        WHEN es.active_staffing_mode = 'manual_only' AND es.plan_entry_id IS NULL THEN 'Не требуется (Вне плана)'
        WHEN COUNT(DISTINCT CASE 
            WHEN eds.absence_reason IS NULL AND (
                (eds.override_eq_id = es.eq_id AND eds.override_shift_number = es.shift_number) OR
                (eds.assigned_eq_id = es.eq_id AND eds.planned_shift_number = es.shift_number AND eds.override_eq_id IS NULL)
            ) THEN eds.employee_id END) > 0 THEN 'Люди не нужны (ОК)'
        ELSE '⚠️ ТРЕБУЕТСЯ НАЗНАЧЕНИЕ'
    END AS "Нужно_назначить",

    -- ПЛАНОВЫЙ СОСТАВ (Кто должен быть + причина отсутствия)
    GROUP_CONCAT(DISTINCT 
        CASE WHEN eds.assigned_eq_id = es.eq_id AND eds.planned_shift_number = es.shift_number THEN
            CONCAT(eds.full_name, 
                CASE 
                    WHEN eds.absence_reason IS NOT NULL THEN CONCAT(' (❌ ', eds.absence_reason, ')')
                    WHEN eds.override_eq_id IS NOT NULL AND eds.override_eq_id <> es.eq_id THEN CONCAT(' (➡️ на ', eds.target_eq_name, ')')
                    ELSE ' (✅)' 
                END
            )
        END 
    SEPARATOR ' | ') AS "План_и_Статусы",

    -- ФАКТИЧЕСКИЙ СОСТАВ (Кто реально работает)
    GROUP_CONCAT(DISTINCT CASE 
        WHEN eds.absence_reason IS NULL AND (
            (eds.override_eq_id = es.eq_id AND eds.override_shift_number = es.shift_number) OR
            (eds.assigned_eq_id = es.eq_id AND eds.planned_shift_number = es.shift_number AND eds.override_eq_id IS NULL)
        ) THEN eds.full_name
    END SEPARATOR ', ') AS "Фактически_на_смене"

FROM EquipmentShifts es
LEFT JOIN EmployeeDailyStatus eds ON es.day_date = eds.day_date
GROUP BY es.day_date, es.eq_id, es.shift_id
ORDER BY es.day_date, es.area_sort, es.eq_sort, es.shift_number;
