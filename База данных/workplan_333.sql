/*
 Navicat Premium Dump SQL

 Source Server         : local
 Source Server Type    : MySQL
 Source Server Version : 80400 (8.4.0)
 Source Host           : localhost:3309
 Source Schema         : workplan

 Target Server Type    : MySQL
 Target Server Version : 80400 (8.4.0)
 File Encoding         : 65001

 Date: 10/09/2026 21:43:39
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
-- Records of absence_types
-- ----------------------------
INSERT INTO `absence_types` VALUES (1, 'Больничный', 0, '#FF6347');
INSERT INTO `absence_types` VALUES (2, 'Отпуск', 1, '#4CAF50');
INSERT INTO `absence_types` VALUES (3, 'Отгул', 1, '#FF0000');

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
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of absences
-- ----------------------------
INSERT INTO `absences` VALUES (1, 4, 1, '2026-05-08', '2026-05-20');
INSERT INTO `absences` VALUES (2, 2, 2, '2026-05-07', '2026-05-21');
INSERT INTO `absences` VALUES (4, 1, 2, '2026-07-19', '2026-08-01');
INSERT INTO `absences` VALUES (5, 3, 2, '2026-09-03', '2026-09-16');
INSERT INTO `absences` VALUES (6, 2, 1, '2026-09-01', '2026-09-04');

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
-- Records of contact_types
-- ----------------------------
INSERT INTO `contact_types` VALUES (1, 'phone', 'Телефон');
INSERT INTO `contact_types` VALUES (2, 'email', 'Электронная почта');
INSERT INTO `contact_types` VALUES (3, 'address', 'Адрес проживания');

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
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of employee_contacts
-- ----------------------------

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
) ENGINE = InnoDB AUTO_INCREMENT = 21 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of employee_equipment_assignments
-- ----------------------------
INSERT INTO `employee_equipment_assignments` VALUES (1, 1, 1, '2014-08-14');
INSERT INTO `employee_equipment_assignments` VALUES (2, 2, 1, '2010-01-01');
INSERT INTO `employee_equipment_assignments` VALUES (3, 3, 1, '2010-01-01');
INSERT INTO `employee_equipment_assignments` VALUES (4, 4, 1, '2010-01-01');
INSERT INTO `employee_equipment_assignments` VALUES (5, 5, 1, '2010-01-01');
INSERT INTO `employee_equipment_assignments` VALUES (6, 6, 2, '2017-06-20');
INSERT INTO `employee_equipment_assignments` VALUES (7, 7, 2, '2012-07-02');
INSERT INTO `employee_equipment_assignments` VALUES (8, 8, 4, '2022-07-02');
INSERT INTO `employee_equipment_assignments` VALUES (9, 9, 10, '2012-07-09');
INSERT INTO `employee_equipment_assignments` VALUES (10, 10, 10, '2012-07-09');
INSERT INTO `employee_equipment_assignments` VALUES (11, 11, 12, '2010-07-09');
INSERT INTO `employee_equipment_assignments` VALUES (12, 1, 13, '2026-07-01');
INSERT INTO `employee_equipment_assignments` VALUES (13, 12, 14, '2018-07-13');
INSERT INTO `employee_equipment_assignments` VALUES (14, 13, 17, '2018-07-13');
INSERT INTO `employee_equipment_assignments` VALUES (15, 14, 15, '2010-07-13');
INSERT INTO `employee_equipment_assignments` VALUES (16, 15, 16, '2018-07-14');
INSERT INTO `employee_equipment_assignments` VALUES (17, 7, 13, '2026-08-01');
INSERT INTO `employee_equipment_assignments` VALUES (18, 16, 7, '2026-08-01');
INSERT INTO `employee_equipment_assignments` VALUES (19, 17, 7, '2026-02-01');
INSERT INTO `employee_equipment_assignments` VALUES (20, 18, 7, '2026-02-08');

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
) ENGINE = InnoDB AUTO_INCREMENT = 19 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of employee_position_assignments
-- ----------------------------
INSERT INTO `employee_position_assignments` VALUES (1, 1, 1, '2014-08-14');
INSERT INTO `employee_position_assignments` VALUES (2, 2, 1, '2014-08-14');
INSERT INTO `employee_position_assignments` VALUES (3, 3, 1, '2016-01-01');
INSERT INTO `employee_position_assignments` VALUES (4, 4, 1, '2015-01-01');
INSERT INTO `employee_position_assignments` VALUES (5, 5, 1, '2020-01-01');
INSERT INTO `employee_position_assignments` VALUES (6, 6, 1, '2017-06-20');
INSERT INTO `employee_position_assignments` VALUES (7, 7, 1, '2012-07-02');
INSERT INTO `employee_position_assignments` VALUES (8, 8, 1, '2022-07-02');
INSERT INTO `employee_position_assignments` VALUES (9, 9, 3, '2012-07-09');
INSERT INTO `employee_position_assignments` VALUES (10, 10, 3, '2012-07-09');
INSERT INTO `employee_position_assignments` VALUES (11, 11, 4, '2010-07-09');
INSERT INTO `employee_position_assignments` VALUES (12, 12, 7, '2018-07-13');
INSERT INTO `employee_position_assignments` VALUES (13, 13, 5, '2018-07-13');
INSERT INTO `employee_position_assignments` VALUES (14, 14, 6, '2010-07-13');
INSERT INTO `employee_position_assignments` VALUES (15, 15, 6, '2018-07-14');
INSERT INTO `employee_position_assignments` VALUES (16, 16, 1, '2026-08-01');
INSERT INTO `employee_position_assignments` VALUES (17, 17, 1, '2026-02-01');
INSERT INTO `employee_position_assignments` VALUES (18, 18, 1, '2026-02-08');

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
) ENGINE = InnoDB AUTO_INCREMENT = 21 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of employee_schedule_assignments
-- ----------------------------
INSERT INTO `employee_schedule_assignments` VALUES (1, 1, 1, '2009-12-20');
INSERT INTO `employee_schedule_assignments` VALUES (2, 2, 3, '2010-01-02');
INSERT INTO `employee_schedule_assignments` VALUES (3, 3, 4, '2010-01-04');
INSERT INTO `employee_schedule_assignments` VALUES (4, 4, 2, '2010-01-06');
INSERT INTO `employee_schedule_assignments` VALUES (5, 5, 2, '2019-01-01');
INSERT INTO `employee_schedule_assignments` VALUES (6, 6, 1, '2017-06-20');
INSERT INTO `employee_schedule_assignments` VALUES (7, 7, 4, '2012-07-02');
INSERT INTO `employee_schedule_assignments` VALUES (8, 8, 2, '2022-07-02');
INSERT INTO `employee_schedule_assignments` VALUES (9, 9, 1, '2012-07-09');
INSERT INTO `employee_schedule_assignments` VALUES (10, 10, 3, '2012-07-09');
INSERT INTO `employee_schedule_assignments` VALUES (11, 11, 11, '2010-07-09');
INSERT INTO `employee_schedule_assignments` VALUES (12, 12, 6, '2018-07-13');
INSERT INTO `employee_schedule_assignments` VALUES (13, 1, 6, '2026-07-01');
INSERT INTO `employee_schedule_assignments` VALUES (14, 13, 11, '2018-07-13');
INSERT INTO `employee_schedule_assignments` VALUES (15, 14, 11, '2010-07-13');
INSERT INTO `employee_schedule_assignments` VALUES (16, 15, 11, '2018-07-14');
INSERT INTO `employee_schedule_assignments` VALUES (17, 7, 5, '2026-08-01');
INSERT INTO `employee_schedule_assignments` VALUES (18, 16, 12, '2026-08-01');
INSERT INTO `employee_schedule_assignments` VALUES (19, 17, 13, '2026-02-01');
INSERT INTO `employee_schedule_assignments` VALUES (20, 18, 14, '2026-02-08');

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
) ENGINE = InnoDB AUTO_INCREMENT = 19 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of employees
-- ----------------------------
INSERT INTO `employees` VALUES (1, 'Павельчук', 'Андрей', 'Анатольевич', 'Павельчук А. А.', 'worker');
INSERT INTO `employees` VALUES (2, 'Коськин', 'Кирилл', 'Сергеевич', 'Коськин К. С.', 'worker');
INSERT INTO `employees` VALUES (3, 'Михалевич', 'Алексей', 'Сергеевич', 'Михалевич А. С.', 'worker');
INSERT INTO `employees` VALUES (4, 'Гудков', 'Даниил', 'Владимирович', 'Гудков Д. В.', 'worker');
INSERT INTO `employees` VALUES (5, 'Петров', 'Петр', 'Петрович', 'Петров П. П.', 'worker');
INSERT INTO `employees` VALUES (6, 'Ольхов', 'Леонид', 'Андреевич', 'Ольхов Л. А.', 'worker');
INSERT INTO `employees` VALUES (7, 'Надточий', 'Олег', 'Игоревич', 'Надточий О. И.', 'worker');
INSERT INTO `employees` VALUES (8, 'Светлов', 'Антон', '', 'Светлов А.', 'worker');
INSERT INTO `employees` VALUES (9, 'Собаль', 'Татьяна', 'Иосифовна', 'Собаль Т. И.', 'worker');
INSERT INTO `employees` VALUES (10, 'Кохан', 'Анастасия', 'Сергеевна', 'Кохан А. С.', 'worker');
INSERT INTO `employees` VALUES (11, 'Кошель', 'Юрий', 'Васильевич', 'Кошель Ю. В.', 'worker');
INSERT INTO `employees` VALUES (12, 'Давидович', 'Дмитрий', 'Сергеевич', 'Давидович Д. С.', 'worker');
INSERT INTO `employees` VALUES (13, 'Золотая', 'Галина', '', 'Золотая Г.', 'worker');
INSERT INTO `employees` VALUES (14, 'Журко', 'Андрей', 'Михайлович', 'Журко А. М.', 'worker');
INSERT INTO `employees` VALUES (15, 'Лойко', 'Александр', 'Петрович', 'Лойко А. П.', 'worker');
INSERT INTO `employees` VALUES (16, 'Сидоров', 'Семен', 'Семенович', 'Сидоров С. С.', 'worker');
INSERT INTO `employees` VALUES (17, 'Сидорчук', 'Сидор', 'Сидорович', 'Сидорчук С. С.', 'worker');
INSERT INTO `employees` VALUES (18, 'Сидоренко', 'Семион', 'Феликсович', 'Сидоренко С. Ф.', 'worker');

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
) ENGINE = InnoDB AUTO_INCREMENT = 19 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of employment_periods
-- ----------------------------
INSERT INTO `employment_periods` VALUES (1, 1, '2014-08-14', NULL);
INSERT INTO `employment_periods` VALUES (2, 2, '2015-01-01', NULL);
INSERT INTO `employment_periods` VALUES (3, 3, '2016-01-01', NULL);
INSERT INTO `employment_periods` VALUES (4, 4, '2016-01-01', NULL);
INSERT INTO `employment_periods` VALUES (5, 5, '2026-08-18', '2026-08-18');
INSERT INTO `employment_periods` VALUES (6, 6, '2017-06-20', NULL);
INSERT INTO `employment_periods` VALUES (7, 7, '2012-07-02', NULL);
INSERT INTO `employment_periods` VALUES (8, 8, '2022-07-02', '2026-08-20');
INSERT INTO `employment_periods` VALUES (9, 9, '2012-07-09', NULL);
INSERT INTO `employment_periods` VALUES (10, 10, '2012-07-09', NULL);
INSERT INTO `employment_periods` VALUES (11, 11, '2010-07-09', NULL);
INSERT INTO `employment_periods` VALUES (12, 12, '2018-07-13', NULL);
INSERT INTO `employment_periods` VALUES (13, 13, '2018-07-13', NULL);
INSERT INTO `employment_periods` VALUES (14, 14, '2010-07-13', NULL);
INSERT INTO `employment_periods` VALUES (15, 15, '2018-07-14', NULL);
INSERT INTO `employment_periods` VALUES (16, 16, '2026-08-01', NULL);
INSERT INTO `employment_periods` VALUES (17, 17, '2026-02-01', NULL);
INSERT INTO `employment_periods` VALUES (18, 18, '2026-02-08', NULL);

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
) ENGINE = InnoDB AUTO_INCREMENT = 18 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of equipment
-- ----------------------------
INSERT INTO `equipment` VALUES (1, 1, 7, 'Versor P/S 100k3/4/6', '15', '2013-01-01', NULL, 'strict_schedule', 0);
INSERT INTO `equipment` VALUES (2, 1, 7, 'Diana', '9', '2010-01-01', NULL, 'strict_schedule', 1);
INSERT INTO `equipment` VALUES (3, 1, 7, 'Bobst', '38', '2020-01-01', NULL, 'manual_only', 3);
INSERT INTO `equipment` VALUES (4, 1, 7, 'Versor Pasio', '44', '2026-06-01', NULL, 'strict_schedule', 2);
INSERT INTO `equipment` VALUES (5, 1, 7, 'L1000', '55', '2026-06-01', NULL, 'strict_schedule', 4);
INSERT INTO `equipment` VALUES (6, 1, 7, 'L1000 (2)', '45', '2026-06-01', NULL, 'strict_schedule', 5);
INSERT INTO `equipment` VALUES (7, 1, 10, 'Окна', '666', '2026-06-06', NULL, 'strict_schedule', 6);
INSERT INTO `equipment` VALUES (8, 2, 7, 'Bobst 102-1', '10', '2026-07-09', NULL, 'strict_schedule', 1);
INSERT INTO `equipment` VALUES (9, 2, 7, 'Bobst 102-2', '11', '2026-07-09', NULL, 'strict_schedule', 2);
INSERT INTO `equipment` VALUES (10, 5, 7, 'Сменный мастер', '', '2026-07-09', NULL, 'strict_schedule', 1);
INSERT INTO `equipment` VALUES (12, 5, 11, 'Производство', '', '2026-07-09', NULL, 'strict_schedule', 2);
INSERT INTO `equipment` VALUES (13, 1, 8, 'Diana Eye', '', '2026-07-13', NULL, 'strict_schedule', 7);
INSERT INTO `equipment` VALUES (14, 5, 1, 'Заместитель начальника', '', '2020-07-13', NULL, 'strict_schedule', 3);
INSERT INTO `equipment` VALUES (15, 5, 11, 'Участок вырубки и тиснения', '', '2024-07-13', NULL, 'strict_schedule', 4);
INSERT INTO `equipment` VALUES (16, 5, 11, 'Участок плоской печати', '', '2025-07-13', NULL, 'strict_schedule', 5);
INSERT INTO `equipment` VALUES (17, 5, 11, 'Планирование', '', '2010-07-13', NULL, 'strict_schedule', 6);

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
-- Records of equipment_daily_plan
-- ----------------------------
INSERT INTO `equipment_daily_plan` VALUES (1, 3, '2026-05-01', 1, 0);
INSERT INTO `equipment_daily_plan` VALUES (2, 2, '2026-05-01', 2, 1);

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
) ENGINE = InnoDB AUTO_INCREMENT = 22 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of equipment_schedule_history
-- ----------------------------
INSERT INTO `equipment_schedule_history` VALUES (1, 1, 7, '2026-06-06');
INSERT INTO `equipment_schedule_history` VALUES (2, 2, 7, '2010-01-01');
INSERT INTO `equipment_schedule_history` VALUES (3, 3, 7, '2010-01-01');
INSERT INTO `equipment_schedule_history` VALUES (5, 5, 7, '2026-06-01');
INSERT INTO `equipment_schedule_history` VALUES (6, 5, 8, '2025-06-06');
INSERT INTO `equipment_schedule_history` VALUES (7, 2, 7, '2025-06-06');
INSERT INTO `equipment_schedule_history` VALUES (8, 6, 7, '2026-06-01');
INSERT INTO `equipment_schedule_history` VALUES (9, 4, 7, '2026-06-02');
INSERT INTO `equipment_schedule_history` VALUES (10, 4, 7, '2026-06-06');
INSERT INTO `equipment_schedule_history` VALUES (11, 7, 10, '2026-06-06');
INSERT INTO `equipment_schedule_history` VALUES (12, 8, 7, '2026-07-09');
INSERT INTO `equipment_schedule_history` VALUES (13, 9, 7, '2026-07-09');
INSERT INTO `equipment_schedule_history` VALUES (14, 10, 7, '2026-07-09');
INSERT INTO `equipment_schedule_history` VALUES (15, 12, 11, '2026-07-09');
INSERT INTO `equipment_schedule_history` VALUES (16, 13, 8, '2026-07-13');
INSERT INTO `equipment_schedule_history` VALUES (17, 14, 6, '2020-07-13');
INSERT INTO `equipment_schedule_history` VALUES (18, 15, 11, '2024-07-13');
INSERT INTO `equipment_schedule_history` VALUES (19, 16, 11, '2025-07-13');
INSERT INTO `equipment_schedule_history` VALUES (20, 17, 11, '2010-07-13');
INSERT INTO `equipment_schedule_history` VALUES (21, 13, 7, '2026-08-20');

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
) ENGINE = InnoDB AUTO_INCREMENT = 23 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of equipment_staffing_history
-- ----------------------------
INSERT INTO `equipment_staffing_history` VALUES (1, 1, 'strict_schedule', '2020-01-03');
INSERT INTO `equipment_staffing_history` VALUES (2, 2, 'strict_schedule', '2010-01-01');
INSERT INTO `equipment_staffing_history` VALUES (3, 3, 'manual_only', '2010-01-01');
INSERT INTO `equipment_staffing_history` VALUES (7, 1, 'strict_schedule', '2021-05-05');
INSERT INTO `equipment_staffing_history` VALUES (8, 5, 'strict_schedule', '2026-06-01');
INSERT INTO `equipment_staffing_history` VALUES (9, 5, 'manual_only', '2025-06-06');
INSERT INTO `equipment_staffing_history` VALUES (10, 1, 'strict_schedule', '2026-06-06');
INSERT INTO `equipment_staffing_history` VALUES (11, 6, 'strict_schedule', '2026-06-01');
INSERT INTO `equipment_staffing_history` VALUES (12, 4, 'strict_schedule', '2026-06-03');
INSERT INTO `equipment_staffing_history` VALUES (13, 7, 'strict_schedule', '2026-06-06');
INSERT INTO `equipment_staffing_history` VALUES (14, 8, 'strict_schedule', '2026-07-09');
INSERT INTO `equipment_staffing_history` VALUES (15, 9, 'strict_schedule', '2026-07-09');
INSERT INTO `equipment_staffing_history` VALUES (16, 10, 'strict_schedule', '2026-07-09');
INSERT INTO `equipment_staffing_history` VALUES (17, 12, 'strict_schedule', '2026-07-09');
INSERT INTO `equipment_staffing_history` VALUES (18, 13, 'strict_schedule', '2026-07-13');
INSERT INTO `equipment_staffing_history` VALUES (19, 14, 'strict_schedule', '2020-07-13');
INSERT INTO `equipment_staffing_history` VALUES (20, 15, 'strict_schedule', '2024-07-13');
INSERT INTO `equipment_staffing_history` VALUES (21, 16, 'strict_schedule', '2025-07-13');
INSERT INTO `equipment_staffing_history` VALUES (22, 17, 'strict_schedule', '2010-07-13');

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
-- Records of positions
-- ----------------------------
INSERT INTO `positions` VALUES (1, 'Машинист продольно-склеивающего автомата', 'worker');
INSERT INTO `positions` VALUES (2, 'Печатник-тиснильщик', 'worker');
INSERT INTO `positions` VALUES (3, 'Сменный мастер', 'master');
INSERT INTO `positions` VALUES (4, 'Начальник производства', 'chief');
INSERT INTO `positions` VALUES (5, 'Диспетчер', 'chief');
INSERT INTO `positions` VALUES (6, 'Начальник участка', 'master');
INSERT INTO `positions` VALUES (7, 'Заместитель начальника', 'master');

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
-- Records of schedule_cycle_items
-- ----------------------------
INSERT INTO `schedule_cycle_items` VALUES (1, 1, 1);
INSERT INTO `schedule_cycle_items` VALUES (1, 2, 1);
INSERT INTO `schedule_cycle_items` VALUES (1, 3, 3);
INSERT INTO `schedule_cycle_items` VALUES (1, 4, 3);
INSERT INTO `schedule_cycle_items` VALUES (1, 5, 2);
INSERT INTO `schedule_cycle_items` VALUES (1, 6, 2);
INSERT INTO `schedule_cycle_items` VALUES (1, 7, 3);
INSERT INTO `schedule_cycle_items` VALUES (1, 8, 3);
INSERT INTO `schedule_cycle_items` VALUES (2, 1, 1);
INSERT INTO `schedule_cycle_items` VALUES (2, 2, 1);
INSERT INTO `schedule_cycle_items` VALUES (2, 3, 3);
INSERT INTO `schedule_cycle_items` VALUES (2, 4, 3);
INSERT INTO `schedule_cycle_items` VALUES (3, 1, 14);
INSERT INTO `schedule_cycle_items` VALUES (3, 2, 14);
INSERT INTO `schedule_cycle_items` VALUES (3, 3, 14);
INSERT INTO `schedule_cycle_items` VALUES (3, 4, 14);
INSERT INTO `schedule_cycle_items` VALUES (3, 5, 14);
INSERT INTO `schedule_cycle_items` VALUES (3, 6, 3);
INSERT INTO `schedule_cycle_items` VALUES (3, 7, 3);
INSERT INTO `schedule_cycle_items` VALUES (4, 1, 1);
INSERT INTO `schedule_cycle_items` VALUES (4, 1, 2);
INSERT INTO `schedule_cycle_items` VALUES (5, 1, 1);
INSERT INTO `schedule_cycle_items` VALUES (10, 1, 11);
INSERT INTO `schedule_cycle_items` VALUES (10, 1, 12);
INSERT INTO `schedule_cycle_items` VALUES (10, 1, 13);
INSERT INTO `schedule_cycle_items` VALUES (11, 1, 13);
INSERT INTO `schedule_cycle_items` VALUES (11, 2, 13);
INSERT INTO `schedule_cycle_items` VALUES (11, 3, 13);
INSERT INTO `schedule_cycle_items` VALUES (11, 4, 13);
INSERT INTO `schedule_cycle_items` VALUES (11, 5, 13);
INSERT INTO `schedule_cycle_items` VALUES (11, 6, 3);
INSERT INTO `schedule_cycle_items` VALUES (11, 7, 3);
INSERT INTO `schedule_cycle_items` VALUES (11, 8, 12);
INSERT INTO `schedule_cycle_items` VALUES (11, 9, 12);
INSERT INTO `schedule_cycle_items` VALUES (11, 10, 12);
INSERT INTO `schedule_cycle_items` VALUES (11, 11, 12);
INSERT INTO `schedule_cycle_items` VALUES (11, 12, 12);
INSERT INTO `schedule_cycle_items` VALUES (11, 13, 3);
INSERT INTO `schedule_cycle_items` VALUES (11, 14, 3);
INSERT INTO `schedule_cycle_items` VALUES (11, 15, 11);
INSERT INTO `schedule_cycle_items` VALUES (11, 16, 11);
INSERT INTO `schedule_cycle_items` VALUES (11, 17, 11);
INSERT INTO `schedule_cycle_items` VALUES (11, 18, 11);
INSERT INTO `schedule_cycle_items` VALUES (11, 19, 11);
INSERT INTO `schedule_cycle_items` VALUES (11, 20, 3);
INSERT INTO `schedule_cycle_items` VALUES (11, 21, 3);

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
) ENGINE = InnoDB AUTO_INCREMENT = 12 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of schedule_cycles
-- ----------------------------
INSERT INTO `schedule_cycles` VALUES (1, '2/2 (день, ночь)', 8);
INSERT INTO `schedule_cycles` VALUES (2, '2/2 (день)', 4);
INSERT INTO `schedule_cycles` VALUES (3, '5/2', 7);
INSERT INTO `schedule_cycles` VALUES (4, 'Круглосуточно (2 смены)', 1);
INSERT INTO `schedule_cycles` VALUES (5, 'Ежедневно', 1);
INSERT INTO `schedule_cycles` VALUES (10, 'Оборудование, 3 смены', 1);
INSERT INTO `schedule_cycles` VALUES (11, 'Три смены', 21);

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
) ENGINE = InnoDB AUTO_INCREMENT = 40 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of schedule_overrides
-- ----------------------------
INSERT INTO `schedule_overrides` VALUES (1, 3, '2026-05-01', 1, 3, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (3, 4, '2026-05-01', 2, 1, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (4, 1, '2026-05-05', 2, 1, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (5, 3, '2026-05-02', 2, 2, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (9, 4, '2026-05-02', 1, 1, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (10, 2, '2026-05-05', 2, 2, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (11, 4, '2026-05-07', 1, 2, 2, 'Нет работы', 0);
INSERT INTO `schedule_overrides` VALUES (12, 2, '2026-05-07', 1, 1, 0, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (15, 4, '2026-05-07', 2, 1, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (22, 3, '2026-05-02', 1, NULL, 2, 'Отмена мастером в карточке смены', 1);
INSERT INTO `schedule_overrides` VALUES (26, 16, '2026-09-04', 3, 6, 2, 'ввва', 0);
INSERT INTO `schedule_overrides` VALUES (30, 4, '2026-09-01', 2, 1, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (31, 6, '2026-09-01', 1, 1, 2, '🔄 Перевод с Diana', 0);
INSERT INTO `schedule_overrides` VALUES (33, 3, '2026-09-01', 1, 2, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (37, 1, '2026-09-07', 2, 2, 2, NULL, 0);
INSERT INTO `schedule_overrides` VALUES (38, 5, '2026-09-07', 2, NULL, 2, 'Отмена мастером в карточке смены', 1);

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
) ENGINE = InnoDB AUTO_INCREMENT = 15 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of schedule_templates
-- ----------------------------
INSERT INTO `schedule_templates` VALUES (1, 'Сменный 1', 1, '2009-12-31');
INSERT INTO `schedule_templates` VALUES (2, 'Сменный 2', 1, '2010-01-02');
INSERT INTO `schedule_templates` VALUES (3, 'Сменный 3', 1, '2010-01-04');
INSERT INTO `schedule_templates` VALUES (4, 'Сменный 4', 1, '2010-01-06');
INSERT INTO `schedule_templates` VALUES (5, 'Сменный 5', 2, '2010-01-02');
INSERT INTO `schedule_templates` VALUES (6, 'Сменный 6', 2, '2010-01-04');
INSERT INTO `schedule_templates` VALUES (7, 'Рабочее место (2 смены, круглосуточно)', 4, '2010-01-01');
INSERT INTO `schedule_templates` VALUES (8, 'День, 1 смена, 12 часов', 5, '2010-01-01');
INSERT INTO `schedule_templates` VALUES (10, '3 смены круглосуточно', 10, '2026-05-31');
INSERT INTO `schedule_templates` VALUES (11, 'Пятидневка', 3, '2009-12-28');
INSERT INTO `schedule_templates` VALUES (12, 'Три смены 1', 11, '2026-01-05');
INSERT INTO `schedule_templates` VALUES (13, 'Три смены 2', 11, '2026-01-12');
INSERT INTO `schedule_templates` VALUES (14, 'Три смены 3', 11, '2026-01-19');

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
-- Records of shift_definitions
-- ----------------------------
INSERT INTO `shift_definitions` VALUES (1, 1, 'Дневная смена', 'worker', '08:00:00', '20:00:00');
INSERT INTO `shift_definitions` VALUES (2, 2, 'Ночная смена', 'worker', '20:00:00', '08:00:00');
INSERT INTO `shift_definitions` VALUES (3, 0, 'Выходной', 'universal', '00:00:00', '00:00:00');
INSERT INTO `shift_definitions` VALUES (11, 1, '1 Смена', 'universal', '07:00:00', '15:30:00');
INSERT INTO `shift_definitions` VALUES (12, 2, '2 Смена', 'universal', '15:30:00', '23:30:00');
INSERT INTO `shift_definitions` VALUES (13, 3, '3 Смена', 'universal', '23:30:00', '07:00:00');
INSERT INTO `shift_definitions` VALUES (14, 1, 'Рабочий день', 'universal', '08:00:00', '16:30:00');

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
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of work_areas
-- ----------------------------
INSERT INTO `work_areas` VALUES (1, 'Участок склейки', 0);
INSERT INTO `work_areas` VALUES (2, 'Участок вырубки', 1);
INSERT INTO `work_areas` VALUES (4, 'Участок плоской печати', 3);
INSERT INTO `work_areas` VALUES (5, 'Управление производством', 4);

-- ----------------------------
-- Procedure structure for GetAvailableEmployeesForShift
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetAvailableEmployeesForShift`;
delimiter ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAvailableEmployeesForShift`(
    IN p_target_date DATE,
    IN p_target_shift_number INT
)
BEGIN
    WITH TargetShift AS (
        SELECT id, shift_number, start_time, end_time, name as target_shift_name,
               CAST(CONCAT(p_target_date, ' ', start_time) AS DATETIME) as target_start_dt,
               CAST(CONCAT(IF(end_time < start_time, p_target_date + INTERVAL 1 DAY, p_target_date), ' ', end_time) AS DATETIME) as target_end_dt
        FROM shift_definitions WHERE shift_number = p_target_shift_number
        LIMIT 1
    ),
    AllEmployeeActivities AS (
        -- 1. ПЛАНОВАЯ РАБОТА
        SELECT 
            e.id as emp_id, sd.id as shift_id, sd.shift_number, sd.name as shift_name, 
            eq.name as eq_name, NULL as ovr_id, 'План' as source_type,
            c.day_date as cal_date,
            CAST(CONCAT(c.day_date, ' ', sd.start_time) AS DATETIME) as start_dt,
            CAST(CONCAT(IF(sd.end_time < sd.start_time, c.day_date + INTERVAL 1 DAY, c.day_date), ' ', sd.end_time) AS DATETIME) as end_dt
        FROM (SELECT CAST(p_target_date - INTERVAL 1 DAY AS DATE) as day_date UNION SELECT CAST(p_target_date AS DATE) UNION SELECT CAST(p_target_date + INTERVAL 1 DAY AS DATE)) c
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
        WHERE ovr.override_date BETWEEN p_target_date - INTERVAL 1 DAY AND p_target_date + INTERVAL 1 DAY
          AND ovr.status = 2 AND ovr.is_cancellation = 0 
          AND sd_o.shift_number > 0
    )
    SELECT 
        e.id AS "EmployeeId",
        e.full_name AS "EmployeeName",
        
        -- Вытаскиваем OverrideId, если сотрудник ПЕРЕСЕКАЕТСЯ по времени с целевой сменой
        (SELECT a.ovr_id FROM AllEmployeeActivities a, TargetShift ts 
         WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt AND a.ovr_id IS NOT NULL LIMIT 1) AS "OverrideId",

        -- ТЕКУЩАЯ АКТИВНОСТЬ: Проверяем ЛЮБОЕ пересечение временных интервалов смен
        (SELECT CONCAT(IF(a.source_type = 'План', 'План: ', 'Назначение: '), a.shift_name, ' (', a.eq_name, ')') 
         FROM AllEmployeeActivities a, TargetShift ts 
         WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt LIMIT 1) AS "CurrentActivity",

        (SELECT CONCAT(DATE_FORMAT(a.cal_date, '%d.%m.%Y'), ' ', a.source_type, ': ', a.shift_name, ' (', a.eq_name, ')') 
         FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8 ORDER BY a.end_dt DESC LIMIT 1) AS "AdjacentBefore",

        (SELECT CONCAT(DATE_FORMAT(a.cal_date, '%d.%m.%Y'), ' ', a.source_type, ': ', a.shift_name, ' (', a.eq_name, ')') 
         FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8 ORDER BY a.start_dt ASC LIMIT 1) AS "AdjacentAfter",

        CASE 
            WHEN EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND p_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31'))
                THEN (SELECT CASE WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt AND a.source_type = 'Назначение') THEN CONCAT('⚠️ РАБОТАЕТ ПРИ: ', abt.name) ELSE CONCAT('❌ ', abt.name) END FROM absences abs2 JOIN absence_types abt ON abs2.type_id = abt.id WHERE abs2.employee_id = e.id AND p_target_date BETWEEN abs2.start_date AND COALESCE(abs2.end_date, '2099-12-31') LIMIT 1)
            
            -- ИСПРАВЛЕНО: Человек помечается как ЗАНЯТ, если интервалы его смен пересекаются по времени с целевой сменой
            WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) THEN '⛔ ЗАНЯТ'
            
            WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND ((a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8) OR (a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8))) THEN '🟡 ДОСТУПЕН (смежные)'
            ELSE '🟢 ДОСТУПЕН'
        END AS "CurrentStatus"

    FROM employees e
    WHERE EXISTS (SELECT 1 FROM employment_periods ep WHERE ep.employee_id = e.id AND p_target_date BETWEEN ep.hire_date AND COALESCE(ep.fire_date, '2099-12-31'))

    ORDER BY 
        (CASE 
            WHEN NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND p_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')) 
                 -- ИСПРАВЛЕНО: Условие сортировки свободных тоже переведено на пересечение временных интервалов
                 AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) 
                 AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND ((a.end_dt <= ts.target_start_dt AND TIMESTAMPDIFF(HOUR, a.end_dt, ts.target_start_dt) < 8) OR (a.start_dt >= ts.target_end_dt AND TIMESTAMPDIFF(HOUR, ts.target_end_dt, a.start_dt) < 8))) THEN 1
            WHEN NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND p_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')) 
                 AND NOT EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt) THEN 2
            WHEN NOT EXISTS (SELECT 1 FROM absences abs WHERE abs.employee_id = e.id AND p_target_date BETWEEN abs.start_date AND COALESCE(abs.end_date, '2099-12-31')) THEN 3
            WHEN EXISTS (SELECT 1 FROM AllEmployeeActivities a, TargetShift ts WHERE a.emp_id = e.id AND a.start_dt < ts.target_end_dt AND a.end_dt > ts.target_start_dt AND a.source_type = 'Назначение') THEN 4
            ELSE 5
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
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEquipmentShiftCard`(
    IN p_target_date DATE,
    IN p_target_shift_number INT,
    IN p_target_equipment_id INT
)
BEGIN
    WITH CurrentEquipmentSettings AS (
        SELECT 
            e.id, 
            e.name,
            -- 1. Определяем режим работы на дату
            COALESCE(
                (SELECT staffing_mode FROM equipment_staffing_history 
                 WHERE equipment_id = e.id AND valid_from <= p_target_date 
                 ORDER BY valid_from DESC LIMIT 1),
                e.staffing_mode
            ) AS active_staffing_mode,
            
            -- 2. Определяем шаблон графика станка на дату
            COALESCE(
                (SELECT template_id FROM equipment_schedule_history 
                 WHERE equipment_id = e.id AND valid_from <= p_target_date 
                 ORDER BY valid_from DESC LIMIT 1),
                e.template_id
            ) AS active_template_id
        FROM equipment e
        WHERE e.id = p_target_equipment_id
    ),

    TargetShiftDetails AS (
        SELECT 
            sd.id AS target_shift_id,
            sd.name AS target_shift_name,
            sd.start_time AS target_start_time,
            sd.end_time AS target_end_time,
            ces.id AS equipment_id,
            ces.name AS equipment_name,
            ces.active_staffing_mode,
            
            -- Проверяем, должен ли станок работать по плану в эту смену
            EXISTS (
                SELECT 1 
                FROM schedule_templates st
                JOIN schedule_cycles sc ON st.cycle_id = sc.id
                JOIN schedule_cycle_items sci ON sci.cycle_id = sc.id
                WHERE st.id = ces.active_template_id
                  AND sci.shift_id = sd.id
                  AND sci.day_number = (MOD(DATEDIFF(p_target_date, st.base_date) % sc.cycle_length + sc.cycle_length, sc.cycle_length) + 1)
            ) AS is_equipment_working_by_plan
        FROM CurrentEquipmentSettings ces
        -- Точечно связываем смену через шаблон графика станка, отсекая чужие смены с тем же номером
        JOIN schedule_templates st_target ON ces.active_template_id = st_target.id
        JOIN schedule_cycles sc_target ON st_target.cycle_id = sc_target.id
        JOIN schedule_cycle_items sci_target ON sci_target.cycle_id = sc_target.id
        JOIN shift_definitions sd ON sci_target.shift_id = sd.id
        WHERE sd.shift_number = p_target_shift_number
        LIMIT 1
    ),

    EmployeeStatus AS (
        SELECT 
            e.id AS employee_id,
            e.full_name,
            tsd.target_shift_id,
            
            -- Флаг: запланирован ли сотрудник на этот станок и этот номер смены
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
        CROSS JOIN TargetShiftDetails tsd
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
            AND ovr.shift_id = tsd.target_shift_id
        
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
        tsd.equipment_name,
        tsd.target_shift_id AS shift_id,
        tsd.target_shift_name AS shift_name,
        DATE_FORMAT(tsd.target_start_time, '%H:%i') AS time_start,
        DATE_FORMAT(tsd.target_end_time, '%H:%i') AS time_end,
        edp.id AS edp_id, 
        COALESCE(edp.is_cancelled, 0) AS is_equipment_cancelled,
        
        CASE 
            WHEN edp.is_cancelled = 1 THEN 'Не требуется (Остановка станка)'
            WHEN (SELECT active_count FROM ActiveWorkforce) > 0 THEN '✅ Укомплектовано'
            WHEN tsd.is_equipment_working_by_plan = 0 THEN 'Не требуется (Вне графика)'
            WHEN tsd.active_staffing_mode = 'manual_only' THEN '⚪ Ожидание назначения'
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
        
        CASE WHEN es.is_planned_here = 1 AND es.ovr_id IS NOT NULL THEN es.ovr_id END AS plan_override_id,

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

    FROM TargetShiftDetails tsd
    LEFT JOIN equipment_daily_plan edp ON edp.equipment_id = tsd.equipment_id 
        AND edp.plan_date = p_target_date 
        AND edp.shift_id = tsd.target_shift_id
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
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEquipmentStaffingReport`(IN p_start_date DATE,
    IN p_end_date DATE,
    IN p_target_area_id INT)
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
