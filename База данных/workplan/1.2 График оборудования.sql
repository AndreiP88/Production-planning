-- ============================================================================
-- 1. УСТАНОВКА ПАРАМЕТРОВ
-- ============================================================================
SET @start_date = '2026-05-01';
SET @end_date   = '2026-05-31';
SET @target_area_id = 1;

-- ============================================================================
-- 2. ГЕНЕРАЦИЯ ОТЧЕТА
-- ============================================================================
WITH RECURSIVE calendar AS (
    SELECT CAST(@start_date AS DATE) AS day_date
    UNION ALL
    SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < @end_date
),

-- СРЕЗ СОСТОЯНИЙ СОТРУДНИКОВ (Кто где должен быть и что с ними по факту)
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
         WHERE abs.employee_id = e.id 
           AND c.day_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31') 
         LIMIT 1) AS absence_reason,
        -- Оперативное переназначение
        ovr.equipment_id AS override_eq_id,
        ovr_sd.shift_number AS override_shift_number,
        (SELECT name FROM equipment WHERE id = ovr.equipment_id) AS target_eq_name
    FROM calendar c
    CROSS JOIN employees e
    -- Актуальный график сотрудника
    JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
        AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    JOIN schedule_templates st ON esa.template_id = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (MOD(DATEDIFF(c.day_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
    -- Актуальное закрепление за станком
    LEFT JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
        AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
    -- Утвержденные правки (status = 2)
    LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id AND ovr.override_date = c.day_date AND ovr.status = 2
    LEFT JOIN shift_definitions ovr_sd ON ovr.shift_id = ovr_sd.id
),

-- СЕТКА ТРЕБУЕМЫХ СМЕН ОБОРУДОВАНИЯ
EquipmentGrid AS (
    SELECT 
        c.day_date, eq.id AS eq_id, eq.name AS eq_name, eq.staffing_mode,
        sd.id AS shift_id, sd.name AS shift_name, sd.shift_number,
        wa.name AS area_name,
        wa.sort_order AS area_sort,
        eq.sort_order AS eq_sort,
        COALESCE(edp.is_cancelled, 0) as is_cancelled
    FROM calendar c
    CROSS JOIN equipment eq
    JOIN work_areas wa ON eq.work_area_id = wa.id
    JOIN schedule_templates st ON eq.template_id = st.id
    JOIN schedule_cycles sc ON st.cycle_id = sc.id
    JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
        AND sci.day_number = (MOD(DATEDIFF(c.day_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
    JOIN shift_definitions sd ON sci.shift_id = sd.id
    LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = eq.id AND edp.plan_date = c.day_date AND edp.shift_id = sd.id
    WHERE eq.work_area_id = @target_area_id 
      AND c.day_date BETWEEN eq.commissioned_at AND COALESCE(eq.decommissioned_at, '2099-12-31')
)

-- ФИНАЛЬНЫЙ ВЫВОД
SELECT 
    g.day_date AS "Дата",
    g.area_name AS "Участок",
    g.eq_name AS "Оборудование",
    g.shift_name AS "Смена",
    
    -- ПЛАНОВЫЙ СОСТАВ И СТАТУСЫ (кто на месте, кто болеет, кто ушел)
    GROUP_CONCAT(DISTINCT 
        CASE WHEN eds.assigned_eq_id = g.eq_id AND eds.planned_shift_number = g.shift_number THEN
            CONCAT(
                eds.full_name, 
                CASE 
                    WHEN eds.absence_reason IS NOT NULL THEN CONCAT(' (❌ ', eds.absence_reason, ')')
                    WHEN eds.override_eq_id IS NOT NULL AND eds.override_eq_id <> g.eq_id THEN 
                        CONCAT(' (➡️ на ', eds.target_eq_name, ')')
                    ELSE ' (✅)' 
                END
            )
        END 
    SEPARATOR ' | ') AS "План_и_Статусы",

    -- ФАКТИЧЕСКИЕ ИСПОЛНИТЕЛИ (те, кто реально работает здесь сейчас)
    GROUP_CONCAT(DISTINCT CASE 
        WHEN eds.absence_reason IS NULL AND (
            (eds.override_eq_id = g.eq_id AND eds.override_shift_number = g.shift_number) OR
            (eds.assigned_eq_id = g.eq_id AND eds.planned_shift_number = g.shift_number AND eds.override_eq_id IS NULL)
        ) THEN eds.full_name
    END SEPARATOR ', ') AS "Фактически_на_смене",

    -- ОБЩЕЕ СОСТОЯНИЕ
    CASE 
        WHEN g.is_cancelled = 1 THEN '🛑 ОТМЕНЕНО'
        WHEN COUNT(DISTINCT CASE 
            WHEN eds.absence_reason IS NULL AND (
                (eds.override_eq_id = g.eq_id AND eds.override_shift_number = g.shift_number) OR
                (eds.assigned_eq_id = g.eq_id AND eds.planned_shift_number = g.shift_number AND eds.override_eq_id IS NULL)
            ) THEN eds.employee_id END) > 0 THEN '✅ ОК'
        WHEN g.staffing_mode = 'strict_schedule' THEN '⚠️ ALERT: ПУСТАЯ СМЕНА'
        ELSE '⚪ ОЖИДАНИЕ'
    END AS "Состояние"

FROM EquipmentGrid g
LEFT JOIN EmployeeDailyStatus eds ON g.day_date = eds.day_date
GROUP BY g.day_date, g.eq_id, g.shift_id
ORDER BY 
    g.day_date ASC, 
    g.area_sort ASC, 
    g.eq_sort ASC, 
    g.shift_number ASC;
