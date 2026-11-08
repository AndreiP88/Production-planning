/*
 Navicat Premium Dump SQL

 Source Server         : localhost_3309
 Source Server Type    : MySQL
 Source Server Version : 80046 (8.0.46)
 Source Host           : localhost:3309
 Source Schema         : workplan

 Target Server Type    : MySQL
 Target Server Version : 80046 (8.0.46)
 File Encoding         : 65001

 Date: 05/11/2026 19:16:09
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
  `is_end_date_required` tinyint(1) NOT NULL DEFAULT 0,
  `status_color` varchar(7) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '#FF6347',
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
) ENGINE = InnoDB AUTO_INCREMENT = 19 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 32 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 29 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 34 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 29 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 31 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
  INDEX `code`(`code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 31 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 177 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 35 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 36 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 8 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
  `parent_plan_shift_id` bigint UNSIGNED NULL DEFAULT NULL COMMENT 'ID исходной плановой смены, с которой осуществлен перевод',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `employee_id`(`employee_id` ASC, `override_date` ASC, `shift_id` ASC) USING BTREE,
  INDEX `fk_overrides_parent_shift`(`parent_plan_shift_id` ASC) USING BTREE,
  CONSTRAINT `fk_overrides_parent_shift` FOREIGN KEY (`parent_plan_shift_id`) REFERENCES `shift_definitions` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE = InnoDB AUTO_INCREMENT = 260 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for schedule_templates
-- ----------------------------
DROP TABLE IF EXISTS `schedule_templates`;
CREATE TABLE `schedule_templates`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `cycle_id` int NOT NULL,
  `target_type` enum('universal','equipment','employee') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'universal' COMMENT 'Назначение графика: универсальный, только станки, только сотрудники',
  `base_date` date NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 12 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 15 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for status_color_palette
-- ----------------------------
DROP TABLE IF EXISTS `status_color_palette`;
CREATE TABLE `status_color_palette`  (
  `status_code` int NOT NULL,
  `status_name_ru` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `hex_back_color` varchar(9) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `hex_fore_color` varchar(9) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  PRIMARY KEY (`status_code`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb3 COLLATE = utf8mb3_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for work_areas
-- ----------------------------
DROP TABLE IF EXISTS `work_areas`;
CREATE TABLE `work_areas`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `visible_columns` json NULL COMMENT 'Настройки видимости колонок плана',
  `sort_order` int NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Procedure structure for GetAvailableEmployeesForShift
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetAvailableEmployeesForShift`;
delimiter ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAvailableEmployeesForShift`(IN p_target_date DATE,
    IN p_target_shift_id INT,
    IN p_target_equipment_id INT)
BEGIN
    -- Локальные переменные для изоляции контекста ONLY_FULL_GROUP_BY
    DECLARE v_target_date DATE;
    DECLARE v_target_shift_id INT;
    DECLARE v_target_shift_number INT;
    DECLARE v_target_equipment_id INT;
    DECLARE v_target_work_area_id INT;
    
    SET v_target_date = p_target_date;
    SET v_target_shift_id = p_target_shift_id;
    SET v_target_equipment_id = p_target_equipment_id;
    
    -- Определяем номер целевой смены и ID домашнего участка выбранного станка
    SELECT shift_number INTO v_target_shift_number FROM shift_definitions WHERE id = v_target_shift_id LIMIT 1;
    SELECT work_area_id INTO v_target_work_area_id FROM equipment WHERE id = v_target_equipment_id LIMIT 1;

    WITH TargetShift AS (
        -- Находим точные временные границы целевой смены по ее уникальному ID
        SELECT id, shift_number, start_time, end_time, name as target_shift_name,
               CAST(CONCAT(v_target_date, ' ', start_time) AS DATETIME) as target_start_dt,
               CAST(CONCAT(IF(end_time < start_time, v_target_date + INTERVAL 1 DAY, v_target_date), ' ', end_time) AS DATETIME) as target_end_dt
        FROM shift_definitions WHERE id = v_target_shift_id
        LIMIT 1
    ),
    AllEmployeeActivities AS (
        -- 1. ПЛАНОВАЯ РАБОТА С УЧЕТОМ УНИКАЛЬНЫХ СМЕН
        SELECT 
            e.id as emp_id, sd.id as shift_id, sd.shift_number, sd.name as shift_name, 
            eq.name as eq_name, eq.work_area_id, NULL as ovr_id, 'План' as source_type,
            c.day_date as cal_date,
            CAST(CONCAT(c.day_date, ' ', sd.start_time) AS DATETIME) as start_dt,
            CAST(CONCAT(IF(sd.end_time < sd.start_time, c.day_date + INTERVAL 1 DAY, c.day_date), ' ', sd.end_time) AS DATETIME) as end_dt
        FROM (SELECT CAST(v_target_date - INTERVAL 1 DAY AS DATE) as day_date UNION SELECT CAST(v_target_date AS DATE) UNION SELECT CAST(v_target_date + INTERVAL 1 DAY AS DATE)) c
        CROSS JOIN employees e
        JOIN employment_periods ep ON ep.employee_id = e.id 
            AND c.day_date >= ep.hire_date AND (ep.fire_date IS NULL OR c.day_date <= ep.fire_date)
        JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
            AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
        JOIN schedule_cycle_items sci ON sci.cycle_id = (SELECT cycle_id FROM schedule_templates WHERE id = esa.template_id)
            AND sci.day_number = (MOD(DATEDIFF(c.day_date, (SELECT base_date FROM schedule_templates WHERE id = esa.template_id)) % (SELECT cycle_length FROM schedule_cycles sc JOIN schedule_templates st ON sc.id = st.cycle_id WHERE st.id = esa.template_id) + (SELECT cycle_length FROM schedule_cycles sc JOIN schedule_templates st ON sc.id = st.cycle_id WHERE st.id = esa.template_id), (SELECT cycle_length FROM schedule_cycles sc JOIN schedule_templates st ON sc.id = st.cycle_id WHERE st.id = esa.template_id)) + 1)
        JOIN shift_definitions sd ON sci.shift_id = sd.id
        LEFT JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
            AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= c.day_date)
        LEFT JOIN equipment eq ON eqa.equipment_id = eq.id
        WHERE sd.shift_number > 0 -- Жестко отсекаем выходные с номером 0
          AND NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND c.day_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31'))
          AND NOT EXISTS (SELECT 1 FROM schedule_overrides ovr WHERE ovr.employee_id = e.id AND ovr.override_date = c.day_date AND ovr.status = 2)

        UNION ALL

        -- 2. РУЧНЫЕ НАЗНАЧЕНИЯ
        SELECT 
            ovr.employee_id, ovr.shift_id, sd_o.shift_number, sd_o.name, 
            eq_o.name, eq_o.work_area_id, ovr.id, 'Назначение',
            ovr.override_date as cal_date,
            CAST(CONCAT(ovr.override_date, ' ', sd_o.start_time) AS DATETIME),
            CAST(CONCAT(IF(sd_o.end_time < sd_o.start_time, ovr.override_date + INTERVAL 1 DAY, ovr.override_date), ' ', sd_o.end_time) AS DATETIME)
        FROM schedule_overrides ovr
        JOIN shift_definitions sd_o ON ovr.shift_id = sd_o.id
        JOIN equipment eq_o ON ovr.equipment_id = eq_o.id
        WHERE ovr.override_date BETWEEN v_target_date - INTERVAL 1 DAY AND v_target_date + INTERVAL 1 DAY
          AND ovr.status = 2 AND ovr.is_cancellation = 0 
          AND sd_o.shift_number > 0 -- Жестко отсекаем выходные с номером 0
    ),
    EmployeeBaseArea AS (
        -- Рассчитываем текущую производственную зону сотрудника
        SELECT 
            e_b.id AS emp_id,
            COALESCE(
                (SELECT eq_o.work_area_id FROM schedule_overrides ovr_b JOIN equipment eq_o ON ovr_b.equipment_id = eq_o.id WHERE ovr_b.employee_id = e_b.id AND ovr_b.override_date = v_target_date AND ovr_b.status = 2 LIMIT 1),
                (SELECT eq_p.work_area_id FROM employee_equipment_assignments eqa_b JOIN equipment eq_p ON eqa_b.equipment_id = eq_p.id WHERE eqa_b.employee_id = e_b.id AND eqa_b.valid_from <= v_target_date ORDER BY eqa_b.valid_from DESC LIMIT 1)
            ) AS work_area_id
        FROM employees e_b
    )

    SELECT 
        e.id AS EmployeeId,
        e.full_name AS EmployeeName,
        
        (SELECT a.ovr_id FROM AllEmployeeActivities a, TargetShift ts 
         WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt AND a.ovr_id IS NOT NULL LIMIT 1) AS OverrideId,

        -- Если у человека в этой смене по AllEmployeeActivities пусто, пишем Выходной
        COALESCE(
            (SELECT CONCAT(IF(a.source_type = 'План', 'План: ', 'Назначение: '), a.shift_name, ' (', a.eq_name, ')') 
             FROM AllEmployeeActivities a, TargetShift ts 
             WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt LIMIT 1),
            ''
        ) AS CurrentActivity,

        (SELECT CONCAT(DATE_FORMAT(a.cal_date, '%d.%m.%Y'), ' ', a.source_type, ': ', a.shift_name, ' (', a.eq_name, ')') 
         FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8 ORDER BY a.end_dt DESC LIMIT 1) AS AdjacentBefore,

        (SELECT CONCAT(DATE_FORMAT(a.cal_date, '%d.%m.%Y'), ' ', a.source_type, ': ', a.shift_name, ' (', a.eq_name, ')') 
         FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8 ORDER BY a.start_dt ASC LIMIT 1) AS AdjacentAfter,
        
        -- УРОВНИ ДОСТУПНОСТИ ПО ЗОНАМ ОТВЕТСТВЕННОСТИ
        CASE 
            WHEN EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND v_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')) THEN 0
            WHEN eb.work_area_id = v_target_work_area_id THEN
                CASE 
                    WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) THEN 1
                    WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND ((a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8) OR (a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8))) THEN 2
                    ELSE 3
                END
            ELSE
                CASE 
                    WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) THEN 6
                    WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND ((a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8) OR (a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8))) THEN 5
                    ELSE 4
                END
        END AS StatusCode,
        
        CASE 
            WHEN EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND v_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31'))
                THEN (SELECT CASE WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt AND a.source_type = 'Назначение') THEN CONCAT('⚠️ РАБОТАЕТ ПРИ: ', abt.name) ELSE CONCAT('❌ ', abt.name) END FROM absences abs2 JOIN absence_types abt ON abs2.type_id = abt.id WHERE abs2.employee_id = e.id AND v_target_date BETWEEN abs2.start_date AND COALESCE(abs2.end_date, '2099-12-31') LIMIT 1)
            
            WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) THEN '⛔ ЗАНЯТ'
            WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND ((a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8) OR (a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8))) THEN '🟡 ДОСТУПЕН (смежные)'
            ELSE '🟢 ДОСТУПЕН'
        END AS CurrentStatus

    FROM employees e
    JOIN EmployeeBaseArea eb ON e.id = eb.emp_id
    WHERE EXISTS (SELECT 1 FROM employment_periods ep WHERE ep.employee_id = e.id AND v_target_date BETWEEN ep.hire_date AND COALESCE(ep.fire_date, '2099-12-31'))

    -- РАНЖИРОВАНИЕ: Свободные своего участка -> Смежные своего -> Свободные чужого -> Смежные чужого -> Занятые
    ORDER BY 
        (CASE 
WHEN eb.work_area_id = v_target_work_area_id AND NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND v_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')) AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND ((a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8) OR (a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8))) THEN 1
WHEN eb.work_area_id = v_target_work_area_id AND NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND v_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')) AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) THEN 2
WHEN eb.work_area_id = v_target_work_area_id THEN 3
WHEN eb.work_area_id != v_target_work_area_id AND NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND v_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')) AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND ((a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8) OR (a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8))) THEN 4
WHEN eb.work_area_id != v_target_work_area_id AND NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND v_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')) AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) THEN 5
ELSE 6
END) ASC,
e.full_name ASC;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for GetEquipmentShiftCard
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetEquipmentShiftCard`;
delimiter ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEquipmentShiftCard`(IN p_target_date DATE,
    IN p_target_shift_id INT,
    IN p_target_equipment_id INT)
BEGIN
    WITH CurrentEquipmentSettings AS (
        SELECT 
            e.id, 
            e.name,
            COALESCE(
                (SELECT staffing_mode FROM equipment_staffing_history 
                 WHERE equipment_id = e.id AND valid_from <= p_target_date 
                 ORDER BY valid_from DESC LIMIT 1),
                e.staffing_mode
            ) AS staffing_mode,
            
            COALESCE(
                (SELECT template_id FROM equipment_schedule_history 
                 WHERE equipment_id = e.id AND valid_from <= p_target_date 
                 ORDER BY valid_from DESC LIMIT 1),
                e.template_id
            ) AS template_id
        FROM equipment e
        WHERE e.id = p_target_equipment_id
    ),

    TargetShiftDetails AS (
        SELECT 
            sd.id AS target_shift_id,
            sd.shift_number AS target_shift_number,
            sd.name AS target_shift_name,
            sd.start_time AS target_start_time,
            sd.end_time AS target_end_time,
            ces.id AS equipment_id,
            ces.name AS equipment_name,
            ces.staffing_mode,
            
            (SELECT COUNT(1) 
             FROM schedule_templates st
             JOIN schedule_cycles sc ON st.cycle_id = sc.id
             JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id
             JOIN shift_definitions sd_plan ON sci.shift_id = sd_plan.id
             WHERE st.id = ces.template_id
               AND sd_plan.shift_number = sd.shift_number
               AND sci.day_number = (((DATEDIFF(p_target_date, st.base_date) % sc.cycle_length) + sc.cycle_length) % sc.cycle_length + 1)
            ) AS is_equipment_working_by_plan
        FROM shift_definitions sd
        CROSS JOIN CurrentEquipmentSettings ces
        WHERE sd.id = p_target_shift_id
        LIMIT 1
    ),

    EmployeeStatus AS (
        SELECT 
            e.id AS employee_id,
            e.full_name,
            tsd.target_shift_id,
            
            -- Проверяем: привязан ли человек к этому станку по графику и совпадает ли номер смены
            (COALESCE((SELECT eqa.equipment_id FROM employee_equipment_assignments eqa WHERE eqa.employee_id = e.id AND eqa.valid_from <= p_target_date ORDER BY eqa.valid_from DESC LIMIT 1), 0) = p_target_equipment_id 
             AND sd_emp.shift_number = tsd.target_shift_number) as is_planned_here,
            
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
        CROSS JOIN TargetShiftDetails tsd
        JOIN employment_periods ep ON ep.employee_id = e.id 
            AND p_target_date >= ep.hire_date 
            AND (ep.fire_date IS NULL OR p_target_date <= ep.fire_date)
            
        JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
            AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments 
                                  WHERE employee_id = e.id AND valid_from <= p_target_date)
        JOIN schedule_templates st ON esa.template_id = st.id
        JOIN schedule_cycles sc ON st.cycle_id = sc.id
        JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
            AND sci.day_number = (((DATEDIFF(p_target_date, st.base_date) % sc.cycle_length) + sc.cycle_length) % sc.cycle_length + 1)
        JOIN shift_definitions sd_emp ON sci.shift_id = sd_emp.id
        
        -- Ручные оверрайды ищем по ID открываемой смены ячейки
        LEFT JOIN schedule_overrides ovr ON ovr.employee_id = e.id 
            AND ovr.override_date = p_target_date 
            AND ovr.shift_id = tsd.target_shift_id
        
        WHERE 
            (COALESCE((SELECT eqa.equipment_id FROM employee_equipment_assignments eqa WHERE eqa.employee_id = e.id AND eqa.valid_from <= p_target_date ORDER BY eqa.valid_from DESC LIMIT 1), 0) = p_target_equipment_id 
             AND sd_emp.shift_number = tsd.target_shift_number)
            OR (ovr.equipment_id = p_target_equipment_id AND ovr.id IS NOT NULL)
    ),

    ActiveWorkforce AS (
        SELECT COUNT(DISTINCT employee_id) as active_count
        FROM EmployeeStatus
        WHERE (ovr_status = 2 AND ovr_eq_id = p_target_equipment_id AND ovr_is_cancel = 0)
           OR (is_planned_here = 1 AND abs_reason IS NULL 
               AND NOT EXISTS (SELECT 1 FROM schedule_overrides o6 WHERE o6.employee_id = EmployeeStatus.employee_id AND o6.override_date = p_target_date AND o6.status = 2 AND o6.parent_plan_shift_id = target_shift_id)
               AND (ovr_status IS NULL OR ovr_status != 2 OR (ovr_is_cancel = 0 AND ovr_eq_id = p_target_equipment_id)))
    ),
    
    FactStaffGroup AS (
        SELECT GROUP_CONCAT(DISTINCT es_f.full_name SEPARATOR ', ') AS approved_staff_names
        FROM EmployeeStatus es_f
        WHERE (es_f.ovr_status = 2 AND es_f.ovr_eq_id = p_target_equipment_id AND es_f.ovr_is_cancel = 0)
           OR (es_f.is_planned_here = 1 AND es_f.abs_reason IS NULL 
               AND NOT EXISTS (SELECT 1 FROM schedule_overrides o6 WHERE o6.employee_id = es_f.employee_id AND o6.override_date = p_target_date AND o6.status = 2 AND o6.parent_plan_shift_id = target_shift_id)
               AND (es_f.ovr_status IS NULL OR es_f.ovr_status != 2))
    )

    SELECT 
        tsd.equipment_name,
        tsd.target_shift_id AS shift_id,
        tsd.target_shift_number AS shift_number,
        tsd.target_shift_name AS shift_name,
        DATE_FORMAT(tsd.target_start_time, '%H:%i') AS time_start,
        DATE_FORMAT(tsd.target_end_time, '%H:%i') AS time_end,
        edp.id AS edp_id, 
        COALESCE(edp.is_cancelled, 0) AS is_equipment_cancelled,
        MAX(tsd.is_equipment_working_by_plan) AS IsWorkingByPlan, 
        tsd.staffing_mode AS ActiveStaffingMode,
        
        CASE 
            WHEN COALESCE(edp.is_cancelled, 0) = 1 THEN 0 
            WHEN (SELECT active_count FROM ActiveWorkforce) > 0 THEN 2
            WHEN tsd.staffing_mode = 'manual_only' AND edp.id IS NULL THEN 4
            WHEN MAX(tsd.is_equipment_working_by_plan) = 0 AND edp.id IS NULL THEN 5 
            ELSE 3 
        END AS staffing_requirement_code,
        
        CASE 
            WHEN COALESCE(edp.is_cancelled, 0) = 1 THEN 'Не требуется (Работа отменена)'
            WHEN (SELECT active_count FROM ActiveWorkforce) > 0 THEN '✅ Укомплектовано'
            WHEN tsd.staffing_mode = 'manual_only' AND edp.id IS NULL THEN 'Ожидание назначения'
            WHEN MAX(tsd.is_equipment_working_by_plan) = 0 AND edp.id IS NULL THEN 'Не требуется (Вне графика)'
            ELSE '🚨 ТРЕБУЕТСЯ ПЕРСОНАЛ'
        END AS staffing_requirement,
        
        (SELECT active_count FROM ActiveWorkforce) AS active_staff_count,
        
        CASE WHEN es.is_planned_here = 1 THEN es.employee_id END AS plan_employee_id,
        CASE WHEN es.is_planned_here = 1 THEN es.full_name END AS plan_employee_name,
        CASE 
            WHEN es.is_planned_here = 1 AND es.abs_reason IS NOT NULL THEN 1              
            WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_is_cancel = 1 THEN 2 
            WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_eq_id != p_target_equipment_id THEN 3 
            WHEN es.is_planned_here = 1 AND EXISTS (SELECT 1 FROM schedule_overrides o6 WHERE o6.employee_id = es.employee_id AND o6.override_date = p_target_date AND o6.status = 2 AND o6.parent_plan_shift_id = tsd.target_shift_id) THEN 4
            ELSE 0 
        END AS plan_status_code,
        
        CASE 
            WHEN es.is_planned_here = 1 AND es.abs_reason IS NOT NULL THEN CONCAT('❌ ', es.abs_reason)
            WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_is_cancel = 1 
                THEN CONCAT('ОТМЕНЕН (', IFNULL(es.ovr_comment, 'Причина не указана'), ')')
            
            WHEN es.is_planned_here = 1 AND EXISTS (SELECT 1 FROM schedule_overrides o_c WHERE o_c.employee_id = es.employee_id AND o_c.override_date = p_target_date AND o_c.status = 2 AND o_c.parent_plan_shift_id = tsd.target_shift_id AND o_c.equipment_id != p_target_equipment_id)
                THEN CONCAT('ПЕРЕВЕДЕН на ', 
                            (SELECT name FROM equipment WHERE id = (SELECT equipment_id FROM schedule_overrides WHERE employee_id = es.employee_id AND override_date = p_target_date AND status = 2 AND parent_plan_shift_id = tsd.target_shift_id LIMIT 1)), 
                            ' в ', 
                            (SELECT name FROM shift_definitions WHERE id = (SELECT shift_id FROM schedule_overrides WHERE employee_id = es.employee_id AND override_date = p_target_date AND status = 2 AND parent_plan_shift_id = tsd.target_shift_id LIMIT 1)))
            
            WHEN es.is_planned_here = 1 AND EXISTS (SELECT 1 FROM schedule_overrides o_s WHERE o_s.employee_id = es.employee_id AND o_s.override_date = p_target_date AND o_s.status = 2 AND o_s.parent_plan_shift_id = tsd.target_shift_id AND o_s.equipment_id = p_target_equipment_id)
                THEN CONCAT('ПЕРЕВЕДЕН в ', 
                            (SELECT name FROM shift_definitions WHERE id = (SELECT shift_id FROM schedule_overrides WHERE employee_id = es.employee_id AND override_date = p_target_date AND status = 2 AND parent_plan_shift_id = tsd.target_shift_id LIMIT 1)))
            
            WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_eq_id != p_target_equipment_id 
                THEN CONCAT('ПЕРЕВЕДЕН на ', (SELECT name FROM equipment WHERE id = es.ovr_eq_id))
WHEN es.is_planned_here = 1 THEN 'В графике'
END AS plan_status,
CASE
WHEN es.is_planned_here = 1 AND es.ovr_id IS NOT NULL THEN es.ovr_id
WHEN es.is_planned_here = 1 AND es.ovr_id IS NULL THEN
COALESCE((SELECT o_id.id FROM schedule_overrides o_id WHERE o_id.employee_id = es.employee_id AND o_id.override_date = p_target_date AND o_id.status = 2 AND o_id.parent_plan_shift_id = tsd.target_shift_id LIMIT 1), 0)
ELSE NULL
END AS plan_override_id,
CASE WHEN es.ovr_status < 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id THEN es.ovr_id END AS draft_override_id,
CASE WHEN es.ovr_status < 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id THEN es.full_name END AS draft_employee_name,
CASE WHEN es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id THEN es.ovr_id END AS approved_override_id,
CASE WHEN es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id THEN es.full_name END AS approved_employee_name,
CASE
WHEN es.abs_reason IS NOT NULL THEN CONCAT('--- (', es.abs_reason, ')')
WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_is_cancel = 1 THEN '--- (Отмена мастером)'
WHEN es.is_planned_here = 1 AND EXISTS (SELECT 1 FROM schedule_overrides o_c WHERE o_c.employee_id = es.employee_id AND o_c.override_date = p_target_date AND o_c.status = 2 AND o_c.parent_plan_shift_id = tsd.target_shift_id AND o_c.equipment_id != p_target_equipment_id)
THEN CONCAT('--- (Переведен на ', (SELECT name FROM equipment WHERE id = (SELECT equipment_id FROM schedule_overrides WHERE employee_id = es.employee_id AND override_date = p_target_date AND status = 2 AND parent_plan_shift_id = tsd.target_shift_id LIMIT 1)), ')')
WHEN es.is_planned_here = 1 AND EXISTS (SELECT 1 FROM schedule_overrides o_s WHERE o_s.employee_id = es.employee_id AND o_s.override_date = p_target_date AND o_s.status = 2 AND o_s.parent_plan_shift_id = tsd.target_shift_id AND o_s.equipment_id = p_target_equipment_id)
THEN CONCAT('--- (Переведен в ', (SELECT name FROM shift_definitions WHERE id = (SELECT shift_id FROM schedule_overrides WHERE employee_id = es.employee_id AND override_date = p_target_date AND status = 2 AND parent_plan_shift_id = tsd.target_shift_id LIMIT 1)), ')')
WHEN es.is_planned_here = 1 AND es.ovr_status = 2 AND es.ovr_eq_id != p_target_equipment_id THEN '--- (Переведен)'
WHEN (es.ovr_status = 2 AND es.ovr_is_cancel = 0 AND es.ovr_eq_id = p_target_equipment_id)
OR (es.is_planned_here = 1 AND es.abs_reason IS NULL AND (es.ovr_status IS NULL OR es.ovr_status != 2))
THEN 'РАБОТАЕТ'
ELSE '---'
END AS final_fact_status,
(SELECT approved_staff_names FROM FactStaffGroup) AS StaffNames
FROM TargetShiftDetails tsd
LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = tsd.equipment_id
AND edp.plan_date = p_target_date AND edp.shift_id = tsd.target_shift_id
LEFT JOIN EmployeeStatus es ON (es.is_planned_here = 1 OR es.ovr_id IS NOT NULL)
CROSS JOIN FactStaffGroup
GROUP BY
tsd.equipment_name, tsd.target_shift_id, tsd.target_shift_number, tsd.target_shift_name, tsd.target_start_time,
tsd.target_end_time, edp.id, edp.is_cancelled, tsd.staffing_mode,
es.is_planned_here, es.employee_id, es.full_name, es.abs_reason, es.ovr_status,
es.ovr_is_cancel, es.ovr_eq_id, es.ovr_comment, es.ovr_id
ORDER BY es.is_planned_here DESC, es.full_name ASC;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for GetEquipmentStaffingForPeriod
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetEquipmentStaffingForPeriod`;
delimiter ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEquipmentStaffingForPeriod`(IN p_start_date DATE,
    IN p_end_date DATE,
    IN p_equipment_id INT)
BEGIN
    SET SESSION group_concat_max_len = 10000;

    WITH RECURSIVE calendar AS (
        SELECT p_start_date AS day_date
        UNION ALL
        SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < p_end_date
    ),
    TargetEquipmentShifts AS (
        SELECT 
            c.day_date, 
            sd.id AS shift_id, 
            sd.name AS shift_name, 
            sd.shift_number,
            sd.start_time AS shift_start_time,
            sd.end_time AS shift_end_time,
            COALESCE((
                SELECT edp.is_cancelled 
                FROM equipment_daily_plan edp 
                WHERE edp.equipment_id = eq.id 
                  AND edp.plan_date = c.day_date 
                  AND edp.shift_id = sd.id 
                LIMIT 1
            ), 0) AS is_cancelled
        FROM calendar c
        JOIN equipment eq ON eq.id = p_equipment_id
        JOIN schedule_templates st ON COALESCE(
            (SELECT template_id FROM equipment_schedule_history 
             WHERE equipment_id = eq.id AND valid_from <= c.day_date 
             ORDER BY valid_from DESC LIMIT 1), eq.template_id) = st.id
        JOIN schedule_cycles sc ON st.cycle_id = sc.id
        JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
            AND sci.day_number = (((DATEDIFF(c.day_date, st.base_date) % sc.cycle_length) + sc.cycle_length) % sc.cycle_length + 1)
        JOIN shift_definitions sd ON sci.shift_id = sd.id

        UNION DISTINCT

        SELECT 
            edp_out.plan_date AS day_date, 
            sd.id AS shift_id, 
            sd.name AS shift_name, 
            sd.shift_number,
            sd.start_time AS shift_start_time,
            sd.end_time AS shift_end_time,
            edp_out.is_cancelled AS is_cancelled
        FROM equipment_daily_plan edp_out
        JOIN shift_definitions sd ON edp_out.shift_id = sd.id
        WHERE edp_out.equipment_id = p_equipment_id
          AND edp_out.plan_date BETWEEN p_start_date AND p_end_date
    )

    SELECT 
        qs.day_date AS `Date`,
        qs.shift_number AS `ShiftNum`,
        qs.shift_name AS `ShiftName`,
        qs.shift_start_time AS `TimeStart`,
        qs.shift_end_time AS `TimeEnd`,
        qs.is_cancelled AS `IsCancelled`, 
        
        COALESCE(
            -- Очередь 1: Ручные переназначения (С защитой от уволенных)
            (SELECT GROUP_CONCAT(DISTINCT e.full_name SEPARATOR ', ') 
             FROM schedule_overrides ovr 
             JOIN employees e ON ovr.employee_id = e.id 
             -- Проверяем, что сотрудник еще работал на дату оверрайда
             JOIN employment_periods ep_ovr ON ep_ovr.employee_id = e.id
                 AND ovr.override_date >= ep_ovr.hire_date
                 AND (ep_ovr.fire_date IS NULL OR ovr.override_date <= ep_ovr.fire_date)
             WHERE ovr.override_date = qs.day_date 
               AND ovr.equipment_id = p_equipment_id 
               AND ovr.shift_id = qs.shift_id
               AND ovr.status = 2 
               AND ovr.is_cancellation = 0),
               
            -- Очередь 2: Базовый план (С защитой от уволенных и переведенных)
            (SELECT GROUP_CONCAT(DISTINCT e.full_name SEPARATOR ', ') 
             FROM employee_equipment_assignments eqa 
             JOIN employees e ON eqa.employee_id = e.id
             JOIN employment_periods ep_plan ON ep_plan.employee_id = e.id
                 AND qs.day_date >= ep_plan.hire_date AND (ep_plan.fire_date IS NULL OR qs.day_date <= ep_plan.fire_date)
             JOIN employee_schedule_assignments esa ON esa.employee_id = e.id
             JOIN schedule_templates st_e ON esa.template_id = st_e.id
             JOIN schedule_cycles sc_e ON st_e.cycle_id = sc_e.id
             JOIN schedule_cycle_items sci_e ON sci_e.cycle_id = sc_e.id 
                 AND sci_e.day_number = (((DATEDIFF(qs.day_date, st_e.base_date) % sc_e.cycle_length) + sc_e.cycle_length) % sc_e.cycle_length + 1)
             WHERE eqa.equipment_id = p_equipment_id 
               AND sci_e.shift_id = qs.shift_id
               
               AND eqa.valid_from = (
                   SELECT MAX(valid_from) 
                   FROM employee_equipment_assignments 
                   WHERE employee_id = e.id AND valid_from <= qs.day_date
               )
               
               AND NOT EXISTS (
                   SELECT 1 FROM employee_equipment_assignments eqa_newer
                   WHERE eqa_newer.employee_id = e.id
                     AND eqa_newer.valid_from <= qs.day_date
                     AND eqa_newer.valid_from >= eqa.valid_from
                     AND eqa_newer.equipment_id <> p_equipment_id
               )

               AND esa.valid_from = (
                   SELECT MAX(valid_from) 
                   FROM employee_schedule_assignments 
                   WHERE employee_id = e.id AND valid_from <= qs.day_date
               )

               AND NOT EXISTS (SELECT 1 FROM schedule_overrides ovr_c WHERE ovr_c.employee_id = e.id AND ovr_c.override_date = qs.day_date AND ovr_c.status = 2 AND ovr_c.is_cancellation = 1)
               -- НОВОЕ: Проверка межсменного перевода (убираем человека из этой плановой смены)
               AND NOT EXISTS (SELECT 1 FROM schedule_overrides o6 WHERE o6.employee_id = e.id AND o6.override_date = qs.day_date AND o6.status = 2 AND o6.parent_plan_shift_id = qs.shift_id)
               AND NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND qs.day_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')))

        ) AS `StaffNames`
        
    FROM TargetEquipmentShifts qs
    ORDER BY qs.day_date ASC, qs.shift_number ASC;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for GetEquipmentStaffingReport
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetEquipmentStaffingReport`;
delimiter ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEquipmentStaffingReport`(IN p_start_date DATE,
    IN p_end_date DATE,
    IN p_target_area_id INT)
BEGIN
    -- Локальные переменные для обхода бага MySQL с рекурсией
    DECLARE v_start_date DATE;
    DECLARE v_end_date DATE;
    
    SET v_start_date = p_start_date;
    SET v_end_date = p_end_date;
    SET SESSION group_concat_max_len = 10000;

    WITH RECURSIVE calendar AS (
        SELECT v_start_date AS day_date
        UNION ALL
        SELECT day_date + INTERVAL 1 DAY FROM calendar WHERE day_date < v_end_date
    ),
    EquipmentShifts AS (
        SELECT 
            c.day_date, eq.id AS eq_id, eq.name AS eq_name, 
            eq.code AS eq_code, eq.sort_order AS eq_sort,
            sd.id AS shift_id, sd.name AS shift_name, sd.shift_number, 
            sd.start_time AS shift_start_time, sd.end_time AS shift_end_time,
            COALESCE((SELECT is_cancelled FROM equipment_daily_plan edp 
                      WHERE edp.equipment_id = eq.id AND edp.plan_date = c.day_date 
                        AND edp.shift_id = sd.id), 0) as is_cancelled,
            COALESCE((SELECT staffing_mode FROM equipment_staffing_history 
                      WHERE equipment_id = eq.id AND valid_from <= c.day_date 
                      ORDER BY valid_from DESC LIMIT 1), eq.staffing_mode) AS active_mode,
            EXISTS (SELECT 1 FROM equipment_daily_plan edp2 
                    WHERE edp2.equipment_id = eq.id AND edp2.plan_date = c.day_date 
                      AND edp2.shift_id = sd.id) AS has_plan_entry
        FROM calendar c
        CROSS JOIN equipment eq
        JOIN schedule_templates st ON COALESCE(
            (SELECT template_id FROM equipment_schedule_history 
             WHERE equipment_id = eq.id AND valid_from <= c.day_date 
             ORDER BY valid_from DESC LIMIT 1), eq.template_id) = st.id
        JOIN schedule_cycles sc ON st.cycle_id = sc.id
        JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id 
            AND sci.day_number = (((DATEDIFF(c.day_date, st.base_date) % sc.cycle_length) 
                + sc.cycle_length) % sc.cycle_length + 1)
        JOIN shift_definitions sd ON sci.shift_id = sd.id
        WHERE eq.work_area_id = p_target_area_id
    ),
    EmployeeLinks AS (
        SELECT c.day_date, e.id AS emp_id, e.full_name, eqa.equipment_id AS eq_id, 
               sd_p.id AS shift_id, 'PLAN' AS l_type, NULL AS l_status, 0 AS is_cancellation
        FROM calendar c CROSS JOIN employees e
        JOIN employment_periods ep ON ep.employee_id = e.id 
            AND c.day_date >= ep.hire_date 
            AND (ep.fire_date IS NULL OR c.day_date <= ep.fire_date)
        JOIN employee_schedule_assignments esa ON esa.employee_id = e.id 
            AND esa.valid_from = (SELECT MAX(valid_from) FROM employee_schedule_assignments 
                                  WHERE employee_id = e.id AND valid_from <= c.day_date)
        JOIN schedule_templates st_e ON esa.template_id = st_e.id
        JOIN schedule_cycles sc_e ON st_e.cycle_id = sc_e.id
        JOIN schedule_cycle_items sci_e ON sci_e.cycle_id = sc_e.id 
            AND sci_e.day_number = (((DATEDIFF(c.day_date, st_e.base_date) % sc_e.cycle_length) 
                + sc_e.cycle_length) % sc_e.cycle_length + 1)
        JOIN shift_definitions sd_p ON sci_e.shift_id = sd_p.id
        JOIN employee_equipment_assignments eqa ON eqa.employee_id = e.id 
            AND eqa.valid_from = (SELECT MAX(valid_from) FROM employee_equipment_assignments 
                                  WHERE employee_id = e.id AND valid_from <= c.day_date)
        UNION ALL
        SELECT ovr.override_date, e2.id, e2.full_name, ovr.equipment_id, ovr.shift_id, 
               'OVR' AS l_type, ovr.status AS l_status, ovr.is_cancellation
        FROM schedule_overrides ovr JOIN employees e2 ON ovr.employee_id = e2.id
        JOIN employment_periods ep2 ON ep2.employee_id = e2.id 
            AND ovr.override_date >= ep2.hire_date 
            AND (ep2.fire_date IS NULL OR ovr.override_date <= ep2.fire_date)
    ),
    ManualEquipmentActivities AS (
        SELECT 
            edp_m.plan_date AS day_date, eq_m.id AS eq_id, eq_m.name AS eq_name, 
            eq_m.code AS eq_code, eq_m.sort_order AS eq_sort,
            sd_m.id AS shift_id, sd_m.name AS shift_name, sd_m.shift_number,
            sd_m.start_time AS shift_start_time, sd_m.end_time AS shift_end_time,
            edp_m.is_cancelled,
            COALESCE((SELECT staffing_mode FROM equipment_staffing_history 
                      WHERE equipment_id = eq_m.id AND valid_from <= edp_m.plan_date 
                      ORDER BY valid_from DESC LIMIT 1), eq_m.staffing_mode) AS active_mode,
            1 AS has_plan_entry
        FROM equipment_daily_plan edp_m
        JOIN equipment eq_m ON edp_m.equipment_id = eq_m.id
        JOIN shift_definitions sd_m ON edp_m.shift_id = sd_m.id
        WHERE eq_m.work_area_id = p_target_area_id
          AND edp_m.plan_date BETWEEN v_start_date AND v_end_date
          AND edp_m.is_cancelled = 0
          AND NOT EXISTS (
              SELECT 1 FROM EquipmentShifts es_chk 
              WHERE es_chk.day_date = edp_m.plan_date 
                AND es_chk.eq_id = eq_m.id AND es_chk.shift_id = sd_m.id
          )
    ),
    CombinedGrid AS (
        SELECT * FROM EquipmentShifts
        UNION ALL
        SELECT * FROM ManualEquipmentActivities
    ),
    
    -- АВТОНОМНЫЙ ПРЕДРАСЧЕТ ВСЕХ СТАТУСОВ СОТРУДНИКОВ (НЕЛОМАЮЩИЙСЯ И ЧИСТЫЙ)
    EmployeeShiftStatuses AS (
        SELECT 
            el_s.day_date,
            el_s.emp_id,
            el_s.shift_id,
            el_s.eq_id,
            CASE 
                -- 1. Больничный / отпуск
                WHEN EXISTS (SELECT 1 FROM absences a WHERE a.employee_id = el_s.emp_id AND el_s.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31'))
                    THEN COALESCE((SELECT CONCAT(' (❌ ', abt.name, ')') FROM absences abs_in JOIN absence_types abt ON abs_in.type_id = abt.id WHERE abs_in.employee_id = el_s.emp_id AND el_s.day_date BETWEEN abs_in.start_date AND COALESCE(abs_in.end_date, '2099-12-31') LIMIT 1), ' (❌ Отсутствует)')
                
                -- 2. Отмена смены мастером
                WHEN EXISTS (SELECT 1 FROM schedule_overrides o_c WHERE o_c.employee_id = el_s.emp_id AND o_c.override_date = el_s.day_date AND o_c.shift_id = el_s.shift_id AND o_c.status = 2 AND o_c.is_cancellation = 1) 
                    THEN ' (🚫 Отмена)'
                
                -- 3. Межсменный перевод (в другую смену и/или станок)
                WHEN EXISTS (SELECT 1 FROM schedule_overrides o_cross WHERE o_cross.employee_id = el_s.emp_id AND o_cross.override_date = el_s.day_date AND o_cross.status = 2 AND o_cross.is_cancellation = 0 AND o_cross.parent_plan_shift_id = el_s.shift_id)
                    THEN COALESCE((
                        SELECT 
                            CASE 
                                WHEN o_cr.equipment_id != el_s.eq_id THEN CONCAT(' (➡️ ', eq_dest.name, ' в ', sd_dest.name, ')')
                                ELSE CONCAT(' (➡️ ', sd_dest.name, ')')
                            END
                        FROM schedule_overrides o_cr
                        JOIN equipment eq_dest ON o_cr.equipment_id = eq_dest.id
                        JOIN shift_definitions sd_dest ON o_cr.shift_id = sd_dest.id
                        WHERE o_cr.employee_id = el_s.emp_id AND o_cr.override_date = el_s.day_date AND o_cr.status = 2 AND o_cr.is_cancellation = 0 AND o_cr.parent_plan_shift_id = el_s.shift_id
                        LIMIT 1
                    ), ' (➡️ Переведен)')
                
                -- 4. Внутрисменный перевод (смена та же, изменился станок)
                WHEN EXISTS (SELECT 1 FROM schedule_overrides o_m WHERE o_m.employee_id = el_s.emp_id AND o_m.override_date = el_s.day_date AND o_m.shift_id = el_s.shift_id AND o_m.status = 2 AND o_m.equipment_id != el_s.eq_id AND o_m.is_cancellation = 0) 
                    THEN CONCAT(' (➡️ ', COALESCE((SELECT name FROM equipment WHERE id = (SELECT equipment_id FROM schedule_overrides WHERE employee_id = el_s.emp_id AND override_date = el_s.day_date AND shift_id = el_s.shift_id AND status = 2 AND is_cancellation = 0 LIMIT 1)), 'другой станок'), ')')
                
                ELSE ' (✅)'
            END AS status_text
        FROM EmployeeLinks el_s
        WHERE el_s.l_type = 'PLAN'
    ),
    
    PreCalculatedReport AS (
        SELECT 
            qs.day_date, qs.eq_id, qs.eq_name, qs.eq_code, qs.eq_sort,
            qs.shift_id, qs.shift_name, qs.shift_number, qs.shift_start_time, qs.shift_end_time,
            qs.is_cancelled, qs.active_mode, qs.has_plan_entry,
            
            -- СБОРКА СТРОКИ ПЛАНА С ГОТОВЫМИ СТАТУСАМИ ИЗ НАШЕГО CTE
            GROUP_CONCAT(DISTINCT 
                CASE WHEN esl.l_type = 'PLAN' AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id THEN
                    CONCAT(esl.full_name, COALESCE(ess.status_text, ' (✅)'))
                END 
            SEPARATOR ' | ') AS `PlanAndStatuses`,
            
            GROUP_CONCAT(DISTINCT CASE WHEN esl.l_type = 'OVR' && esl.l_status = 2 && esl.eq_id = qs.eq_id && esl.shift_id = qs.shift_id && esl.is_cancellation = 0 THEN CONCAT('✅ ', esl.full_name) END SEPARATOR ', ') AS `Assignments`,
            GROUP_CONCAT(DISTINCT CASE WHEN esl.l_type = 'OVR' && esl.l_status < 2 && esl.eq_id = qs.eq_id && esl.shift_id = qs.shift_id && esl.is_cancellation = 0 THEN CONCAT('📝 ', esl.full_name) END SEPARATOR ', ') AS `Drafts`,
            
            GROUP_CONCAT(DISTINCT 
                CASE 
                    WHEN esl.l_type = 'OVR' AND esl.l_status = 2 AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id AND esl.is_cancellation = 0 
                    THEN esl.full_name
                    
                    WHEN esl.l_type = 'PLAN' AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id 
AND NOT EXISTS (SELECT 1 FROM absences a WHERE a.employee_id = esl.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31'))
AND NOT EXISTS (SELECT 1 FROM schedule_overrides o4 WHERE o4.employee_id = esl.emp_id AND o4.override_date = qs.day_date AND o4.shift_id = qs.shift_id AND o4.status = 2 AND o4.is_cancellation = 1)
AND NOT EXISTS (SELECT 1 FROM schedule_overrides o5 WHERE o5.employee_id = esl.emp_id AND o5.override_date = qs.day_date AND o5.shift_id = qs.shift_id AND o5.status = 2 AND o5.equipment_id != qs.eq_id AND o5.is_cancellation = 0)
AND NOT EXISTS (SELECT 1 FROM schedule_overrides o6 WHERE o6.employee_id = esl.emp_id AND o6.override_date = qs.day_date AND o6.status = 2 AND o6.parent_plan_shift_id = qs.shift_id)
THEN esl.full_name
ELSE NULL
END
SEPARATOR ', ') AS ApprovedFact
FROM CombinedGrid qs
LEFT JOIN EmployeeLinks esl ON qs.day_date = esl.day_date AND qs.shift_id = esl.shift_id
LEFT JOIN EmployeeShiftStatuses ess ON esl.day_date = ess.day_date AND esl.emp_id = ess.emp_id AND esl.shift_id = ess.shift_id AND esl.eq_id = ess.eq_id
GROUP BY qs.day_date, qs.eq_id, qs.eq_name, qs.eq_code, qs.shift_id, qs.shift_name,
qs.shift_number, qs.shift_start_time, qs.shift_end_time, qs.is_cancelled,
qs.active_mode, qs.has_plan_entry, qs.eq_sort
)
-- ЖЕСТКОЕ СВЯЗЫВАНИЕ ЦВЕТА К ОДОБРЕННОМУ ФАКТУ
        SELECT 
        qs.day_date AS Date, 
        qs.eq_id AS EquipId, 
        qs.eq_name AS EquipName, 
        qs.eq_code AS EquipCode,
        qs.shift_id AS ShiftId, 
        qs.shift_number AS ShiftNum, 
        qs.shift_name AS Shift, 
        qs.shift_start_time AS TimeStart, 
        qs.shift_end_time AS TimeEnd,
        
        CASE 
            WHEN qs.is_cancelled = 1 THEN 0                           
            WHEN qs.active_mode = 'manual_only' AND NOT qs.has_plan_entry THEN 1 
            WHEN (
                SELECT COUNT(DISTINCT el.emp_id) FROM EmployeeLinks el 
                WHERE el.day_date = qs.day_date AND el.shift_id = qs.shift_id
                  AND (
                    (el.l_type = 'OVR' AND el.l_status = 2 AND el.eq_id = qs.eq_id AND el.is_cancellation = 0)
                    OR
                    (el.l_type = 'PLAN' AND el.eq_id = qs.eq_id
                     AND NOT EXISTS (SELECT 1 FROM absences a WHERE a.employee_id = el.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31'))
                     AND NOT EXISTS (SELECT 1 FROM schedule_overrides o4 WHERE o4.employee_id = el.emp_id AND o4.override_date = qs.day_date AND o4.shift_id = qs.shift_id AND o4.status = 2 AND o4.is_cancellation = 1)
                     -- ИСПРАВЛЕНО И СИНХРОНИЗИРОВАННО С КОДОМ НИЖЕ (Анализ межсменных и внутрисменных уводов)
                     AND NOT EXISTS (SELECT 1 FROM schedule_overrides o5 WHERE o5.employee_id = el.emp_id AND o5.override_date = qs.day_date AND o5.status = 2 AND o5.is_cancellation = 0 AND ((o5.shift_id = qs.shift_id AND o5.equipment_id != qs.eq_id) OR o5.parent_plan_shift_id = qs.shift_id))
                    )
                  )
            ) > 0 THEN 2 
            WHEN qs.active_mode = 'strict_schedule' THEN 3            
            ELSE 4                                                    
        END AS NeedStatusCode,
        
        CASE 
            WHEN qs.is_cancelled = 1 THEN 'Не требуется (Отмена)'
            WHEN qs.active_mode = 'manual_only' AND NOT qs.has_plan_entry THEN 'Не требуется (Вне плана)'
            WHEN (
                SELECT COUNT(DISTINCT el.emp_id) FROM EmployeeLinks el 
                WHERE el.day_date = qs.day_date AND el.shift_id = qs.shift_id
                  AND (
                    (el.l_type = 'OVR' AND el.l_status = 2 AND el.eq_id = qs.eq_id AND el.is_cancellation = 0)
                    OR
                    (el.l_type = 'PLAN' AND el.eq_id = qs.eq_id
                     AND NOT EXISTS (SELECT 1 FROM absences a WHERE a.employee_id = el.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31'))
                     AND NOT EXISTS (SELECT 1 FROM schedule_overrides o4 WHERE o4.employee_id = el.emp_id AND o4.override_date = qs.day_date AND o4.shift_id = qs.shift_id AND o4.status = 2 AND o4.is_cancellation = 1)
                     -- ИСПРАВЛЕНО И СИНХРОНИЗИРОВАННО С КОДОМ НИЖЕ
                     AND NOT EXISTS (SELECT 1 FROM schedule_overrides o5 WHERE o5.employee_id = el.emp_id AND o5.override_date = qs.day_date AND o5.status = 2 AND o5.is_cancellation = 0 AND ((o5.shift_id = qs.shift_id AND o5.equipment_id != qs.eq_id) OR o5.parent_plan_shift_id = qs.shift_id))
                    )
                  )
            ) > 0 THEN 'Укомплектовано'
            WHEN qs.active_mode = 'strict_schedule' THEN 'ТРЕБУЕТСЯ'
            ELSE 'НУЖНО НАЗНАЧЕНИЕ'
        END AS NeedStatus,
        
        GROUP_CONCAT(DISTINCT 
            CASE WHEN esl.l_type = 'PLAN' AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id THEN 
                CONCAT(esl.full_name, 
                    CASE 
                        WHEN EXISTS (SELECT 1 FROM absences a WHERE a.employee_id = esl.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31')) 
                            THEN (SELECT CONCAT(' (Х ', abt.name, ')') FROM absences abs_in JOIN absence_types abt ON abs_in.type_id = abt.id WHERE abs_in.employee_id = esl.emp_id AND qs.day_date BETWEEN abs_in.start_date AND COALESCE(abs_in.end_date, '2099-12-31') LIMIT 1)
                        
                        WHEN EXISTS (SELECT 1 FROM schedule_overrides o_mov WHERE o_mov.employee_id = esl.emp_id AND o_mov.override_date = qs.day_date AND o_mov.status = 2 AND o_mov.is_cancellation = 0 AND ((o_mov.shift_id = qs.shift_id AND o_mov.equipment_id != qs.eq_id) OR o_mov.parent_plan_shift_id = qs.shift_id))
                            THEN (
                                SELECT 
                                    CASE 
                                        WHEN o_s.equipment_id != qs.eq_id AND o_s.parent_plan_shift_id = qs.shift_id AND o_s.shift_id != qs.shift_id
                                            THEN CONCAT(' (Переведен на ', (SELECT name FROM equipment WHERE id = o_s.equipment_id), ' в ', (SELECT name FROM shift_definitions WHERE id = o_s.shift_id), ')')
                                        WHEN o_s.equipment_id = qs.eq_id AND o_s.parent_plan_shift_id = qs.shift_id AND o_s.shift_id != qs.shift_id
                                            THEN CONCAT(' (Переведен в ', (SELECT name FROM shift_definitions WHERE id = o_s.shift_id), ')')
                                        WHEN o_s.equipment_id != qs.eq_id AND o_s.shift_id = qs.shift_id
                                            THEN CONCAT(' (Переведен на ', (SELECT name FROM equipment WHERE id = o_s.equipment_id), ')')
                                        ELSE ' (Переведен)'
                                    END
                                FROM schedule_overrides o_s
                                WHERE o_s.employee_id = esl.emp_id AND o_s.override_date = qs.day_date AND o_s.status = 2 AND o_s.is_cancellation = 0 AND ((o_s.shift_id = qs.shift_id AND o_s.equipment_id != qs.eq_id) OR o_s.parent_plan_shift_id = qs.shift_id)
                                LIMIT 1
                            )
                        
                        WHEN EXISTS (SELECT 1 FROM schedule_overrides o_c WHERE o_c.employee_id = esl.emp_id AND o_c.override_date = qs.day_date AND o_c.shift_id = qs.shift_id AND o_c.status = 2 AND o_c.is_cancellation = 1)
                            THEN ' (Отменен)' 
                        
                        WHEN qs.is_cancelled = 1 THEN ' (Без рабочего места)' 
                            
                        ELSE '' 
                    END
                ) 
            END 
        SEPARATOR ' | ') AS PlanAndStatuses,
        
        GROUP_CONCAT(DISTINCT 
            CASE WHEN esl.l_type = 'OVR' AND esl.l_status = 2 AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id AND esl.is_cancellation = 0 
                THEN CONCAT('✅ ', esl.full_name, 
                    CASE 
                        WHEN qs.is_cancelled = 1 THEN ' (Без рабочего места)' 
                        ELSE '' 
                    END
                ) 
            END 
        SEPARATOR ', ') AS Assignments,
        
        GROUP_CONCAT(DISTINCT CASE WHEN esl.l_type = 'OVR' && esl.l_status < 2 && esl.eq_id = qs.eq_id && esl.shift_id = qs.shift_id && esl.is_cancellation = 0 THEN CONCAT('📝 ', esl.full_name) END SEPARATOR ', ') AS Drafts,
        
        -- СЕКЦИЯ APPROVEDFACT ТЕПЕРЬ ПОЛНОСТЬЮ СИНХРОННА С ВЕРХНИМИ КАСКАДАМИ CASE
        GROUP_CONCAT(DISTINCT 
            CASE 
                WHEN esl.l_type = 'OVR' AND esl.l_status = 2 AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id AND esl.is_cancellation = 0 THEN esl.full_name
                WHEN esl.l_type = 'PLAN' AND esl.eq_id = qs.eq_id AND esl.shift_id = qs.shift_id 
                     AND NOT EXISTS (SELECT 1 FROM absences a WHERE a.employee_id = esl.emp_id AND qs.day_date BETWEEN a.start_date AND COALESCE(a.end_date, '2099-12-31')) 
                     AND NOT EXISTS (SELECT 1 FROM schedule_overrides o4 WHERE o4.employee_id = esl.emp_id AND o4.override_date = qs.day_date AND o4.shift_id = qs.shift_id AND o4.status = 2 AND o4.is_cancellation = 1)
                     -- ИСПРАВЛЕНО: Теперь и факт учитывает кросс-сменные переводы!
                     AND NOT EXISTS (SELECT 1 FROM schedule_overrides o5 WHERE o5.employee_id = esl.emp_id AND o5.override_date = qs.day_date AND o5.status = 2 AND o5.is_cancellation = 0 AND ((o5.shift_id = qs.shift_id AND o5.equipment_id != qs.eq_id) OR o5.parent_plan_shift_id = qs.shift_id))
                THEN esl.full_name ELSE NULL
            END
        SEPARATOR ', ') AS ApprovedFact
    FROM CombinedGrid qs
    LEFT JOIN EmployeeLinks esl ON qs.day_date = esl.day_date AND qs.shift_id = esl.shift_id
    GROUP BY qs.day_date, qs.eq_id, qs.eq_name, qs.eq_code, qs.shift_id, qs.shift_name, 
             qs.shift_number, qs.shift_start_time, qs.shift_end_time, qs.is_cancelled, 
             qs.active_mode, qs.has_plan_entry, qs.eq_sort
    ORDER BY qs.day_date, qs.eq_sort, qs.eq_id, qs.shift_number;
END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
