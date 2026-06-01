/*
 Navicat Premium Dump SQL

 Source Server         : Local
 Source Server Type    : MySQL
 Source Server Version : 80040 (8.0.40)
 Source Host           : localhost:3309
 Source Schema         : plan

 Target Server Type    : MySQL
 Target Server Version : 80040 (8.0.40)
 File Encoding         : 65001

 Date: 02/05/2026 03:57:26
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
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of absence_types
-- ----------------------------
INSERT INTO `absence_types` VALUES (1, 'Больничный');
INSERT INTO `absence_types` VALUES (2, 'Отпуск');
INSERT INTO `absence_types` VALUES (3, 'Отгул');

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
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  CONSTRAINT `check_abs_dates` CHECK ((`end_date` is null) or (`end_date` >= `start_date`))
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of absences
-- ----------------------------

-- ----------------------------
-- Table structure for employee_areas
-- ----------------------------
DROP TABLE IF EXISTS `employee_areas`;
CREATE TABLE `employee_areas`  (
  `employee_id` int NOT NULL,
  `work_area_id` int NOT NULL,
  PRIMARY KEY (`employee_id`, `work_area_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of employee_areas
-- ----------------------------
INSERT INTO `employee_areas` VALUES (1, 1);
INSERT INTO `employee_areas` VALUES (2, 1);
INSERT INTO `employee_areas` VALUES (3, 1);
INSERT INTO `employee_areas` VALUES (4, 1);

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
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of employee_equipment_assignments
-- ----------------------------
INSERT INTO `employee_equipment_assignments` VALUES (1, 1, 1, '2014-08-14');
INSERT INTO `employee_equipment_assignments` VALUES (2, 2, 1, '2013-01-01');
INSERT INTO `employee_equipment_assignments` VALUES (3, 3, 1, '2017-01-01');
INSERT INTO `employee_equipment_assignments` VALUES (4, 4, 1, '2019-01-01');

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
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of employee_position_assignments
-- ----------------------------
INSERT INTO `employee_position_assignments` VALUES (1, 1, 1, '2014-08-14');
INSERT INTO `employee_position_assignments` VALUES (2, 2, 1, '2013-01-01');
INSERT INTO `employee_position_assignments` VALUES (3, 3, 1, '2015-01-01');
INSERT INTO `employee_position_assignments` VALUES (4, 4, 1, '2019-01-01');

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
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of employee_schedule_assignments
-- ----------------------------
INSERT INTO `employee_schedule_assignments` VALUES (1, 1, 1, '2010-01-01');
INSERT INTO `employee_schedule_assignments` VALUES (2, 2, 2, '2010-01-01');
INSERT INTO `employee_schedule_assignments` VALUES (3, 3, 3, '2010-01-01');
INSERT INTO `employee_schedule_assignments` VALUES (4, 4, 4, '2010-01-01');

-- ----------------------------
-- Table structure for employees
-- ----------------------------
DROP TABLE IF EXISTS `employees`;
CREATE TABLE `employees`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `full_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `system_role` enum('worker','master','chief') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'worker',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of employees
-- ----------------------------
INSERT INTO `employees` VALUES (1, 'Павельчук Андрей Анатольевич', 'worker');
INSERT INTO `employees` VALUES (2, 'Коськин Кирилл Сергеевич', 'worker');
INSERT INTO `employees` VALUES (3, 'Михалевич Алексей', 'worker');
INSERT INTO `employees` VALUES (4, 'Гудков Даниил', 'worker');

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
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  CONSTRAINT `check_emp_life` CHECK ((`fire_date` is null) or (`fire_date` >= `hire_date`))
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of employment_periods
-- ----------------------------
INSERT INTO `employment_periods` VALUES (1, 1, '2014-08-14', NULL);
INSERT INTO `employment_periods` VALUES (2, 2, '2013-01-01', NULL);
INSERT INTO `employment_periods` VALUES (3, 3, '2015-01-01', NULL);
INSERT INTO `employment_periods` VALUES (4, 4, '2019-01-01', NULL);

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
  `sort_order` int NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC) USING BTREE,
  CONSTRAINT `check_eq_life` CHECK ((`decommissioned_at` is null) or (`decommissioned_at` >= `commissioned_at`))
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of equipment
-- ----------------------------
INSERT INTO `equipment` VALUES (1, 1, 7, 'Versor', '15', '2013-01-01', NULL, 0);
INSERT INTO `equipment` VALUES (2, 1, 7, 'Diana', '9', '2009-01-01', NULL, 0);

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
  `comment` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `equipment_id`(`equipment_id` ASC, `plan_date` ASC, `shift_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of equipment_daily_plan
-- ----------------------------
INSERT INTO `equipment_daily_plan` VALUES (1, 2, '2026-05-01', 2, 1, NULL);

-- ----------------------------
-- Table structure for positions
-- ----------------------------
DROP TABLE IF EXISTS `positions`;
CREATE TABLE `positions`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of positions
-- ----------------------------
INSERT INTO `positions` VALUES (1, 'Машинист Продольно-склеивающегоавтомата');

-- ----------------------------
-- Table structure for schedule_items
-- ----------------------------
DROP TABLE IF EXISTS `schedule_items`;
CREATE TABLE `schedule_items`  (
  `template_id` int NOT NULL,
  `day_number` int NOT NULL,
  `shift_id` int NOT NULL,
  PRIMARY KEY (`template_id`, `day_number`, `shift_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of schedule_items
-- ----------------------------
INSERT INTO `schedule_items` VALUES (1, 1, 1);
INSERT INTO `schedule_items` VALUES (1, 2, 1);
INSERT INTO `schedule_items` VALUES (1, 3, 3);
INSERT INTO `schedule_items` VALUES (1, 4, 3);
INSERT INTO `schedule_items` VALUES (1, 5, 2);
INSERT INTO `schedule_items` VALUES (1, 6, 2);
INSERT INTO `schedule_items` VALUES (1, 7, 3);
INSERT INTO `schedule_items` VALUES (1, 8, 3);
INSERT INTO `schedule_items` VALUES (2, 1, 2);
INSERT INTO `schedule_items` VALUES (2, 2, 2);
INSERT INTO `schedule_items` VALUES (2, 3, 3);
INSERT INTO `schedule_items` VALUES (2, 4, 3);
INSERT INTO `schedule_items` VALUES (2, 5, 1);
INSERT INTO `schedule_items` VALUES (2, 6, 1);
INSERT INTO `schedule_items` VALUES (2, 7, 3);
INSERT INTO `schedule_items` VALUES (2, 8, 3);
INSERT INTO `schedule_items` VALUES (3, 1, 3);
INSERT INTO `schedule_items` VALUES (3, 2, 3);
INSERT INTO `schedule_items` VALUES (3, 3, 2);
INSERT INTO `schedule_items` VALUES (3, 4, 2);
INSERT INTO `schedule_items` VALUES (3, 5, 3);
INSERT INTO `schedule_items` VALUES (3, 6, 3);
INSERT INTO `schedule_items` VALUES (3, 7, 1);
INSERT INTO `schedule_items` VALUES (3, 8, 1);
INSERT INTO `schedule_items` VALUES (4, 1, 3);
INSERT INTO `schedule_items` VALUES (4, 2, 3);
INSERT INTO `schedule_items` VALUES (4, 3, 1);
INSERT INTO `schedule_items` VALUES (4, 4, 1);
INSERT INTO `schedule_items` VALUES (4, 5, 3);
INSERT INTO `schedule_items` VALUES (4, 6, 3);
INSERT INTO `schedule_items` VALUES (4, 7, 2);
INSERT INTO `schedule_items` VALUES (4, 8, 2);
INSERT INTO `schedule_items` VALUES (5, 1, 1);
INSERT INTO `schedule_items` VALUES (5, 2, 1);
INSERT INTO `schedule_items` VALUES (5, 3, 0);
INSERT INTO `schedule_items` VALUES (5, 4, 0);
INSERT INTO `schedule_items` VALUES (6, 1, 1);
INSERT INTO `schedule_items` VALUES (6, 2, 1);
INSERT INTO `schedule_items` VALUES (6, 3, 1);
INSERT INTO `schedule_items` VALUES (6, 4, 1);
INSERT INTO `schedule_items` VALUES (6, 5, 1);
INSERT INTO `schedule_items` VALUES (6, 6, 0);
INSERT INTO `schedule_items` VALUES (6, 7, 0);
INSERT INTO `schedule_items` VALUES (7, 1, 1);
INSERT INTO `schedule_items` VALUES (7, 1, 2);
INSERT INTO `schedule_items` VALUES (8, 1, 1);

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
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE,
  UNIQUE INDEX `employee_id`(`employee_id` ASC, `override_date` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of schedule_overrides
-- ----------------------------
INSERT INTO `schedule_overrides` VALUES (1, 3, '2026-05-01', 1, 2, 2, NULL, '2026-05-02 03:28:59');

-- ----------------------------
-- Table structure for schedule_templates
-- ----------------------------
DROP TABLE IF EXISTS `schedule_templates`;
CREATE TABLE `schedule_templates`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `base_date` date NOT NULL,
  `cycle_length` int NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of schedule_templates
-- ----------------------------
INSERT INTO `schedule_templates` VALUES (1, 'Сменный 1', '2009-12-31', 8);
INSERT INTO `schedule_templates` VALUES (2, 'Сменный 2', '2009-12-31', 8);
INSERT INTO `schedule_templates` VALUES (3, 'Сменный 3', '2009-12-31', 8);
INSERT INTO `schedule_templates` VALUES (4, 'Сменный 4', '2009-12-31', 8);
INSERT INTO `schedule_templates` VALUES (5, 'Сменный 5', '2009-12-31', 4);
INSERT INTO `schedule_templates` VALUES (6, 'Сменный 6', '2009-12-31', 7);
INSERT INTO `schedule_templates` VALUES (7, 'Круглосуточный', '2009-12-31', 1);
INSERT INTO `schedule_templates` VALUES (8, 'Только дневные, ежедневно', '2009-12-31', 1);

-- ----------------------------
-- Table structure for shift_definitions
-- ----------------------------
DROP TABLE IF EXISTS `shift_definitions`;
CREATE TABLE `shift_definitions`  (
  `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `shift_number` int NOT NULL,
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `category` enum('worker','equipment','universal') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT 'universal',
  `start_time` time NULL DEFAULT NULL,
  `end_time` time NULL DEFAULT NULL,
  `is_work_day` tinyint(1) NULL DEFAULT 1,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of shift_definitions
-- ----------------------------
INSERT INTO `shift_definitions` VALUES (1, 1, '1 смена', 'worker', '08:00:00', '20:00:00', 1);
INSERT INTO `shift_definitions` VALUES (2, 2, '2 смена', 'worker', '20:00:00', '08:00:00', 1);
INSERT INTO `shift_definitions` VALUES (3, 0, 'Выходной', 'worker', '00:00:00', '00:00:00', 0);
INSERT INTO `shift_definitions` VALUES (4, 1, 'Круглосуточно', 'equipment', NULL, NULL, 1);
INSERT INTO `shift_definitions` VALUES (5, 1, 'Только день', 'equipment', NULL, NULL, 1);

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
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of work_areas
-- ----------------------------
INSERT INTO `work_areas` VALUES (1, 'Участок склейки', 0);

SET FOREIGN_KEY_CHECKS = 1;
