/*
 Navicat Premium Dump SQL

 Source Server         : versor
 Source Server Type    : MySQL
 Source Server Version : 80040 (8.0.40)
 Source Host           : 192.168.0.34:3309
 Source Schema         : workplan

 Target Server Type    : MySQL
 Target Server Version : 80040 (8.0.40)
 File Encoding         : 65001

 Date: 16/06/2026 06:10:41
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for absence_types
-- ----------------------------
DROP TABLE IF EXISTS `absence_types`;
CREATE TABLE `absence_types`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for absences
-- ----------------------------
DROP TABLE IF EXISTS `absences`;
CREATE TABLE `absences`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `employee_id` int NOT NULL,
  `type_id` int NOT NULL,
  `start_date` date NOT NULL,
  `end_date` date NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for contact_types
-- ----------------------------
DROP TABLE IF EXISTS `contact_types`;
CREATE TABLE `contact_types`  (
  `id` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for employee_contacts
-- ----------------------------
DROP TABLE IF EXISTS `employee_contacts`;
CREATE TABLE `employee_contacts`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `employee_id` int NOT NULL,
  `contact_type_id` int UNSIGNED NOT NULL,
  `contact_value` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `emp_contact`(`employee_id` ASC, `contact_type_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for employee_equipment_assignments
-- ----------------------------
DROP TABLE IF EXISTS `employee_equipment_assignments`;
CREATE TABLE `employee_equipment_assignments`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `employee_id` int NOT NULL,
  `equipment_id` int NULL DEFAULT NULL,
  `valid_from` date NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `employee_id`(`employee_id` ASC, `valid_from` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for employee_position_assignments
-- ----------------------------
DROP TABLE IF EXISTS `employee_position_assignments`;
CREATE TABLE `employee_position_assignments`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `employee_id` int NOT NULL,
  `position_id` int NOT NULL,
  `valid_from` date NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `employee_id`(`employee_id` ASC, `valid_from` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for employee_schedule_assignments
-- ----------------------------
DROP TABLE IF EXISTS `employee_schedule_assignments`;
CREATE TABLE `employee_schedule_assignments`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `employee_id` int NOT NULL,
  `template_id` int NOT NULL,
  `valid_from` date NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `employee_id`(`employee_id` ASC, `valid_from` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for employees
-- ----------------------------
DROP TABLE IF EXISTS `employees`;
CREATE TABLE `employees`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `last_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `first_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `patronymic` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `full_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `system_role` enum('worker','master','chief') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'worker',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for employment_periods
-- ----------------------------
DROP TABLE IF EXISTS `employment_periods`;
CREATE TABLE `employment_periods`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `employee_id` int NOT NULL,
  `hire_date` date NOT NULL,
  `fire_date` date NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for equipment
-- ----------------------------
DROP TABLE IF EXISTS `equipment`;
CREATE TABLE `equipment`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `work_area_id` int NOT NULL,
  `template_id` int NULL DEFAULT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `commissioned_at` date NOT NULL,
  `decommissioned_at` date NULL DEFAULT NULL,
  `staffing_mode` enum('strict_schedule','manual_only') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT 'strict_schedule',
  `sort_order` int NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 8 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for equipment_daily_plan
-- ----------------------------
DROP TABLE IF EXISTS `equipment_daily_plan`;
CREATE TABLE `equipment_daily_plan`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `equipment_id` int NOT NULL,
  `plan_date` date NOT NULL,
  `shift_id` int NOT NULL,
  `is_cancelled` tinyint(1) NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `equipment_id`(`equipment_id` ASC, `plan_date` ASC, `shift_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for equipment_schedule_history
-- ----------------------------
DROP TABLE IF EXISTS `equipment_schedule_history`;
CREATE TABLE `equipment_schedule_history`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `equipment_id` int NOT NULL,
  `template_id` int NULL DEFAULT NULL,
  `valid_from` date NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `equipment_id`(`equipment_id` ASC, `valid_from` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for equipment_staffing_history
-- ----------------------------
DROP TABLE IF EXISTS `equipment_staffing_history`;
CREATE TABLE `equipment_staffing_history`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `equipment_id` int NOT NULL,
  `staffing_mode` enum('strict_schedule','manual_only') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `valid_from` date NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `equipment_id`(`equipment_id` ASC, `valid_from` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 14 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for positions
-- ----------------------------
DROP TABLE IF EXISTS `positions`;
CREATE TABLE `positions`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `system_role` enum('worker','master','chief') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'worker',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for schedule_cycle_items
-- ----------------------------
DROP TABLE IF EXISTS `schedule_cycle_items`;
CREATE TABLE `schedule_cycle_items`  (
  `cycle_id` int NOT NULL,
  `day_number` int NOT NULL,
  `shift_id` int NOT NULL,
  PRIMARY KEY (`cycle_id`, `day_number`, `shift_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for schedule_cycles
-- ----------------------------
DROP TABLE IF EXISTS `schedule_cycles`;
CREATE TABLE `schedule_cycles`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `cycle_length` int NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 11 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for schedule_overrides
-- ----------------------------
DROP TABLE IF EXISTS `schedule_overrides`;
CREATE TABLE `schedule_overrides`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `employee_id` int NOT NULL,
  `override_date` date NOT NULL,
  `shift_id` int NULL DEFAULT NULL,
  `equipment_id` int NULL DEFAULT NULL,
  `status` int NULL DEFAULT 0,
  `comment` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `is_cancellation` tinyint(1) NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `employee_id`(`employee_id` ASC, `override_date` ASC, `shift_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for schedule_templates
-- ----------------------------
DROP TABLE IF EXISTS `schedule_templates`;
CREATE TABLE `schedule_templates`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `cycle_id` int NOT NULL,
  `base_date` date NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 11 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for shift_definitions
-- ----------------------------
DROP TABLE IF EXISTS `shift_definitions`;
CREATE TABLE `shift_definitions`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `shift_number` int NOT NULL,
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `category` enum('worker','equipment','universal') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT 'universal',
  `start_time` time NOT NULL DEFAULT '08:00:00',
  `end_time` time NOT NULL DEFAULT '20:00:00',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 14 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for work_areas
-- ----------------------------
DROP TABLE IF EXISTS `work_areas`;
CREATE TABLE `work_areas`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `sort_order` int NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Procedure structure for GetEquipmentShiftCard
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetEquipmentShiftCard`;
delimiter ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEquipmentShiftCard`(
    IN p_target_date DATE,
    IN p_target_shift_number INT,
    IN p_target_equipment_id INT
)
BEGIN
    WITH CurrentEquipmentSettings AS (
        SELECT 
            e.id, e.name,
            COALESCE(
                (SELECT staffing_mode FROM equipment_staffing_history 
                 WHERE equipment_id = e.id AND valid_from <= p_target_date 
                 ORDER BY valid_from DESC LIMIT 1),
                e.staffing_mode
            ) AS active_staffing_mode,
            
            sd_target.id AS target_shift_id,
            sd_target.name AS target_shift_name,
            sd_target.start_time AS target_start_time,
            sd_target.end_time AS target_end_time,
            
            EXISTS (
                SELECT 1 
                FROM schedule_templates st
                JOIN schedule_cycles sc ON st.cycle_id = sc.id
                JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id
                WHERE st.id = e.template_id
                  AND sci.shift_id = sd_target.id
                  AND sci.day_number = (MOD(DATEDIFF(p_target_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
            ) AS is_equipment_working_by_plan
        FROM equipment e
        CROSS JOIN (SELECT id, name, start_time, end_time FROM shift_definitions WHERE shift_number = p_target_shift_number LIMIT 1) sd_target
        WHERE e.id = p_target_equipment_id
    ),

    EmployeeStatus AS (
        SELECT 
            e.id AS employee_id,
            e.full_name,
            ces.target_shift_id,
            
            (eqa.equipment_id = p_target_equipment_id AND sd_plan.shift_number = p_target_shift_number) as is_planned_here,
            
            (SELECT abt.name FROM absences abs 
             JOIN absence_types abt ON abs.type_id = abt.id 
             WHERE abs.employee_id = e.id 
               AND p_target_date >= abs.start_date 
               AND (abs.end_date IS NULL OR p_target_date <= abs.end_date) 
             LIMIT 1) AS abs_reason,
            
            ovr.id AS ovr_id,
            ovr.status AS ovr_status,
            ovr.is_cancellation AS ovr_is_cancel,
            ovr.equipment_id AS ovr_eq_id,
            ovr.comment AS ovr_comment
            
        FROM employees e
        CROSS JOIN CurrentEquipmentSettings ces
        JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
            AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= p_target_date)
        JOIN schedule_templates st ON esa.template_id = st.id
        JOIN schedule_cycles sc ON st.cycle_id = sc.id
        JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
            AND sci.day_number = (MOD(DATEDIFF(p_target_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
        JOIN shift_definitions sd_plan ON sci.shift_id = sd_plan.id
        LEFT JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
            AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= p_target_date)
        
        LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id 
            AND ovr.override_date = p_target_date 
            AND ovr.shift_id = ces.target_shift_id
        
        WHERE 
            (eqa.equipment_id = p_target_equipment_id AND sd_plan.shift_number = p_target_shift_number)
            OR (ovr.equipment_id = p_target_equipment_id AND ovr.id IS NOT NULL)
    ),

    ActiveWorkforce AS (
        SELECT COUNT(*) as active_count
        FROM EmployeeStatus
        WHERE (ovr_status = 2 AND ovr_eq_id = p_target_equipment_id AND ovr_is_cancel = 0)
           OR (is_planned_here = 1 AND abs_reason IS NULL AND (ovr_status IS NULL OR ovr_status != 2 OR (ovr_is_cancel = 0 AND ovr_eq_id = p_target_equipment_id)))
    )

    SELECT 
        ces.name AS equipment_name,
        ces.target_shift_name AS shift_name,
        DATE_FORMAT(ces.target_start_time, '%H:%i') AS time_start,
        DATE_FORMAT(ces.target_end_time, '%H:%i') AS time_end,
        
        -- Технические поля для фронтенда/бэкенда
        edp.id AS edp_id, 
        COALESCE(edp.is_cancelled, 0) AS is_equipment_cancelled,
        
        CASE 
            WHEN edp.is_cancelled = 1 THEN 'Не требуется (Остановка станка)'
            WHEN (SELECT active_count FROM ActiveWorkforce) > 0 THEN '✅ Укомплектовано'
            WHEN ces.is_equipment_working_by_plan = 0 THEN 'Не требуется (Вне графика)'
            WHEN ces.active_staffing_mode = 'manual_only' THEN '⚪ Ожидание назначения'
            ELSE '🚨 ТРЕБУЕТСЯ ПЕРСОНАЛ'
        END AS staffing_requirement,

        CASE WHEN es.is_planned_here = 1 THEN es.employee_id END AS plan_employee_id,
        CASE WHEN es.is_planned_here = 1 THEN es.full_name END AS plan_employee_name,
        CASE 
            WHEN es.is_planned_here = 1 AND es.abs_reason IS NOT NULL THEN CONCAT('❌ ', es.abs_reason)
            WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_is_cancel = 1 
                THEN CONCAT('🛑 ОТМЕНЕН (', COALESCE(es.ovr_comment, 'Причина не указана'), ')')
            WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_eq_id != p_target_equipment_id 
                THEN CONCAT('🔄 ПЕРЕВЕДЕН на ', (SELECT name FROM equipment WHERE id = es.ovr_eq_id), 
                            IF(es.ovr_comment IS NOT NULL, CONCAT(' [', es.ovr_comment, ']'), ''))
            WHEN es.is_planned_here = 1 THEN '✅ В графике' 
        END AS plan_status,

        CASE WHEN es.ovr_status < 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id THEN es.ovr_id END AS draft_override_id,
        CASE WHEN es.ovr_status < 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id THEN es.full_name END AS draft_employee_name,

        CASE WHEN es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id THEN es.ovr_id END AS approved_override_id,
        CASE WHEN es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id THEN es.full_name END AS approved_employee_name,

        CASE 
            WHEN es.abs_reason IS NOT NULL THEN CONCAT('--- (', es.abs_reason, ')')
            WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_is_cancel = 1 THEN '--- (Отмена мастером)'
            WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_eq_id != p_target_equipment_id THEN '--- (Переведен)'
            
            WHEN (es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id)
              OR (es.is_planned_here = 1 AND es.abs_reason IS NULL AND (es.ovr_status IS NULL OR es.ovr_status != 2)) 
            THEN '✅ РАБОТАЕТ'
            ELSE '---'
        END AS final_fact_status

    FROM CurrentEquipmentSettings ces
    LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = ces.id 
        AND edp.plan_date = p_target_date 
        AND edp.shift_id = ces.target_shift_id
    LEFT JOIN EmployeeStatus es ON (es.is_planned_here = 1 OR es.ovr_id IS NOT NULL)
    ORDER BY es.is_planned_here DESC, es.full_name ASC;

END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for GetEquipmentStaffingReport
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetEquipmentStaffingReport`;
delimiter ;;
CREATE DEFINER=`oxyfox`@`versorpc` PROCEDURE `GetEquipmentStaffingReport`(
    IN p_start_date DATE,
    IN p_end_date DATE,
    IN p_target_area_id INT
)
BEGIN
    SET SESSION group_concat_max_len = 10000;

    WITH RECURSIVE calendar AS (
        SELECT p_start_date AS day_date
        UNION ALL
        SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < p_end_date
    ),
    EquipmentShifts AS (
        -- ... (этот блок остается без изменений, как в вашем исходном запросе)
        SELECT 
            c.day_date, eq.id AS eq_id, eq.name AS eq_name, eq.code AS eq_code, eq.sort_order AS eq_sort,
            sd.id AS shift_id, sd.name AS shift_name, sd.shift_number,
            COALESCE((SELECT is_cancelled FROM equipment_daily_plan edp WHERE edp.equipment_id = eq.id AND edp.plan_date = c.day_date AND edp.shift_id = sd.id), 0) as is_cancelled,
            COALESCE((SELECT staffing_mode FROM equipment_staffing_history WHERE equipment_id = eq.id AND valid_from <= c.day_date ORDER BY valid_from DESC LIMIT 1), eq.staffing_mode) AS active_mode,
            EXISTS (SELECT 1 FROM equipment_daily_plan edp2 WHERE edp2.equipment_id = eq.id AND edp2.plan_date = c.day_date AND edp2.shift_id = sd.id) AS has_plan_entry
        FROM calendar c
        CROSS JOIN equipment eq
        JOIN schedule_templates st ON COALESCE((SELECT template_id FROM equipment_schedule_history WHERE equipment_id = eq.id AND valid_from <= c.day_date ORDER BY valid_from DESC LIMIT 1), eq.template_id) = st.id
        JOIN schedule_cycles sc ON st.cycle_id = sc.id
        JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
            AND sci.day_number = (((DATEDIFF(c.day_date, st.base_date) % sc.cycle_length) + sc.cycle_length) % sc.cycle_length + 1)
        JOIN shift_definitions sd ON sci.shift_id = sd.id
        WHERE eq.work_area_id = p_target_area_id
    ),
    EmployeeLinks AS (
        -- ... (этот блок тоже без изменений)
        SELECT c.day_date, e.id AS emp_id, e.full_name, eqa.equipment_id AS eq_id, sd_p.id AS shift_id, 'PLAN' AS l_type, NULL AS l_status, 0 AS is_cancellation
        FROM calendar c CROSS JOIN employees e
        JOIN employee_schedule_assignments esa ON esa.employee_id = e.id AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
        JOIN schedule_templates st_e ON esa.template_id = st_e.id
        JOIN schedule_cycles sc_e ON st_e.cycle_id = sc_e.id
        JOIN schedule_cycle_items sci_e ON sci_e.cycle_id = sc_e.id AND sci_e.day_number = (((DATEDIFF(c.day_date, st_e.base_date) % sc_e.cycle_length) + sc_e.cycle_length) % sc_e.cycle_length + 1)
        JOIN shift_definitions sd_p ON sci_e.shift_id = sd_p.id
        JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
        UNION ALL
        SELECT ovr.override_date, e2.id, e2.full_name, ovr.equipment_id, ovr.shift_id, 'OVR' AS l_type, ovr.status AS l_status, ovr.is_cancellation
        FROM schedule_overrides ovr JOIN employees e2 ON ovr.employee_id = e2.id
    )
    SELECT 
        qs.day_date AS `Date`,
        qs.eq_id AS `EquipId`,
        qs.eq_name AS `EquipName`,
        qs.eq_code AS `EquipCode`,
        qs.shift_number AS `ShiftNum`,
        qs.shift_name AS `Shift`,
        -- Логика "Потребности" (ваша исходная)
        CASE 
            WHEN qs.is_cancelled = 1 THEN '⚪ Не требуется (Отмена)'
            WHEN qs.active_mode = 'manual_only' AND NOT qs.has_plan_entry THEN '⚪ Не требуется (Вне плана)'
            WHEN (SELECT COUNT(DISTINCT el.emp_id) FROM EmployeeLinks el WHERE el.day_date = qs.day_date AND (NOT EXISTS (SELECT 1 FROM absences a WHERE a.employee_id = el.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31')) OR EXISTS (SELECT 1 FROM schedule_overrides o_f WHERE o_f.employee_id = el.emp_id AND o_f.override_date = qs.day_date AND o_f.shift_id = qs.shift_id AND o_f.status = 2 AND o_f.is_cancellation = 0)) AND ((el.l_type = 'OVR' AND el.l_status = 2 AND el.eq_id = qs.eq_id AND el.shift_id = qs.shift_id AND el.is_cancellation = 0) OR (el.l_type = 'PLAN' AND el.eq_id = qs.eq_id AND el.shift_id = qs.shift_id AND NOT EXISTS (SELECT 1 FROM schedule_overrides o3 WHERE o3.employee_id = el.emp_id AND o3.override_date = qs.day_date AND o3.shift_id = qs.shift_id AND o3.status = 2)))) > 0 THEN '✅ Укомплектовано'
            WHEN qs.active_mode = 'strict_schedule' THEN '🚨 ТРЕБУЕТСЯ'
            ELSE '⚠️ НУЖНО НАЗНАЧЕНИЕ'
        END AS `NeedStatus`,
        -- ПЛАН_ГРАФИК
        GROUP_CONCAT(DISTINCT 
        CASE WHEN esl.l_type = 'PLAN' AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id THEN
            CONCAT(esl.full_name, 
                CASE 
                    -- 1. Если есть больничный/отпуск — всегда выводим причину (даже если назначен оверрайд)
                    WHEN EXISTS (SELECT 1 FROM absences a WHERE a.employee_id = esl.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31'))
                        THEN (SELECT CONCAT(' (❌ ', abt.name, ')') FROM absences abs_in JOIN absence_types abt ON abs_in.type_id = abt.id WHERE abs_in.employee_id = esl.emp_id AND qs.day_date BETWEEN abs_in.start_date AND COALESCE(abs_in.end_date, '2099-12-31') LIMIT 1)
                    
                    -- 2. Если нет больничного, проверяем ручные отмены/переводы
                    WHEN EXISTS (SELECT 1 FROM schedule_overrides o_c WHERE o_c.employee_id = esl.emp_id AND o_c.override_date = qs.day_date AND o_c.shift_id = qs.shift_id AND o_c.status = 2 AND o_c.is_cancellation = 1) 
                        THEN ' (🚫 Отмена)'
                    WHEN EXISTS (SELECT 1 FROM schedule_overrides o_m WHERE o_m.employee_id = esl.emp_id AND o_m.override_date = qs.day_date AND o_m.shift_id = qs.shift_id AND o_m.status = 2 AND o_m.equipment_id != qs.eq_id AND o_m.is_cancellation = 0) 
                        THEN CONCAT(' (➡️ ', (SELECT eq_dest.name FROM equipment eq_dest WHERE eq_dest.id = (SELECT o_dest.equipment_id FROM schedule_overrides o_dest WHERE o_dest.employee_id = esl.emp_id AND o_dest.override_date = qs.day_date AND o_dest.shift_id = qs.shift_id AND o_dest.status = 2 AND o_dest.is_cancellation = 0 LIMIT 1)), ')')
                    ELSE ' (✅)' 
                END
            )
        END 
    SEPARATOR ' | ') AS `PlanAndStatuses`,
        -- НАЗНАЧЕНИЯ (ВОЗВРАЩЕНО)
        GROUP_CONCAT(DISTINCT CASE WHEN esl.l_type = 'OVR' AND esl.l_status = 2 AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id AND esl.is_cancellation = 0 THEN CONCAT('✅ ', esl.full_name) END SEPARATOR ', ') AS `Assignments`,
        -- ЧЕРНОВИКИ (ВОЗВРАЩЕНО)
        GROUP_CONCAT(DISTINCT CASE WHEN esl.l_type = 'OVR' AND esl.l_status < 2 AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id AND esl.is_cancellation = 0 THEN CONCAT('📝 ', esl.full_name) END SEPARATOR ', ') AS `Drafts`,
        -- УТВЕРЖДЕННЫЙ ФАКТ
        GROUP_CONCAT(DISTINCT CASE WHEN ( (esl.l_type = 'OVR' AND esl.l_status = 2 AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id AND esl.is_cancellation = 0) OR (esl.l_type = 'PLAN' AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id AND NOT EXISTS (SELECT 1 FROM absences a WHERE a.employee_id = esl.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31')) AND NOT EXISTS (SELECT 1 FROM schedule_overrides o4 WHERE o4.employee_id = esl.emp_id AND o4.override_date = qs.day_date AND o4.shift_id = qs.shift_id AND o4.status = 2)) ) THEN esl.full_name END SEPARATOR ', ') AS `ApprovedFact`
    FROM EquipmentShifts qs
    LEFT JOIN EmployeeLinks esl ON qs.day_date = esl.day_date
    GROUP BY qs.day_date, qs.eq_id, qs.shift_id
    ORDER BY qs.day_date, qs.eq_sort, qs.eq_id, qs.shift_number;
END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
