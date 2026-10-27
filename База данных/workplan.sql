/*
 Navicat Premium Dump SQL

 Source Server         : Local
 Source Server Type    : MySQL
 Source Server Version : 80040 (8.0.40)
 Source Host           : localhost:3309
 Source Schema         : workplan

 Target Server Type    : MySQL
 Target Server Version : 80040 (8.0.40)
 File Encoding         : 65001

 Date: 27/10/2026 23:15:26
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
) ENGINE = InnoDB AUTO_INCREMENT = 17 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of absences
-- ----------------------------
INSERT INTO `absences` VALUES (1, 4, 1, '2026-05-08', '2026-05-20');
INSERT INTO `absences` VALUES (2, 2, 2, '2026-05-07', '2026-05-21');
INSERT INTO `absences` VALUES (4, 1, 2, '2026-07-19', '2026-08-01');
INSERT INTO `absences` VALUES (5, 3, 2, '2026-09-03', '2026-09-16');
INSERT INTO `absences` VALUES (6, 2, 1, '2026-09-01', '2026-09-05');
INSERT INTO `absences` VALUES (7, 6, 2, '2026-09-09', '2026-09-25');
INSERT INTO `absences` VALUES (8, 23, 2, '2026-09-14', '2026-09-20');
INSERT INTO `absences` VALUES (9, 1, 1, '2026-09-14', '2026-09-25');
INSERT INTO `absences` VALUES (10, 22, 1, '2026-08-26', '2026-10-05');
INSERT INTO `absences` VALUES (11, 24, 2, '2026-09-25', '2026-09-30');
INSERT INTO `absences` VALUES (12, 1, 3, '2026-10-03', '2026-10-04');
INSERT INTO `absences` VALUES (13, 7, 3, '2026-10-09', '2026-10-10');
INSERT INTO `absences` VALUES (14, 1, 1, '2026-10-06', '2026-10-14');
INSERT INTO `absences` VALUES (15, 22, 1, '2026-10-09', '2026-10-20');
INSERT INTO `absences` VALUES (16, 19, 3, '2026-10-31', '2026-11-01');

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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB AUTO_INCREMENT = 28 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of employee_equipment_assignments
-- ----------------------------
INSERT INTO `employee_equipment_assignments` VALUES (1, 1, 1, '2014-08-14');
INSERT INTO `employee_equipment_assignments` VALUES (2, 2, 1, '2010-01-01');
INSERT INTO `employee_equipment_assignments` VALUES (3, 3, 1, '2010-01-01');
INSERT INTO `employee_equipment_assignments` VALUES (4, 4, 1, '2010-01-01');
INSERT INTO `employee_equipment_assignments` VALUES (5, 5, 8, '2010-01-01');
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
INSERT INTO `employee_equipment_assignments` VALUES (19, 17, 4, '2019-09-13');
INSERT INTO `employee_equipment_assignments` VALUES (20, 18, 4, '2025-10-01');
INSERT INTO `employee_equipment_assignments` VALUES (21, 19, 2, '2024-09-13');
INSERT INTO `employee_equipment_assignments` VALUES (22, 20, 2, '2026-03-13');
INSERT INTO `employee_equipment_assignments` VALUES (23, 21, 4, '2026-02-13');
INSERT INTO `employee_equipment_assignments` VALUES (24, 22, 3, '2022-09-13');
INSERT INTO `employee_equipment_assignments` VALUES (25, 23, 3, '2024-09-13');
INSERT INTO `employee_equipment_assignments` VALUES (26, 24, 5, '2021-09-15');
INSERT INTO `employee_equipment_assignments` VALUES (27, 25, 5, '2022-09-15');

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
) ENGINE = InnoDB AUTO_INCREMENT = 26 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of employee_position_assignments
-- ----------------------------
INSERT INTO `employee_position_assignments` VALUES (1, 1, 1, '2014-08-14');
INSERT INTO `employee_position_assignments` VALUES (2, 2, 1, '2014-08-14');
INSERT INTO `employee_position_assignments` VALUES (3, 3, 1, '2016-01-01');
INSERT INTO `employee_position_assignments` VALUES (4, 4, 1, '2015-01-01');
INSERT INTO `employee_position_assignments` VALUES (5, 5, 2, '2020-01-01');
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
INSERT INTO `employee_position_assignments` VALUES (17, 17, 1, '2019-09-13');
INSERT INTO `employee_position_assignments` VALUES (18, 18, 1, '2025-10-01');
INSERT INTO `employee_position_assignments` VALUES (19, 19, 1, '2024-09-13');
INSERT INTO `employee_position_assignments` VALUES (20, 20, 1, '2026-03-13');
INSERT INTO `employee_position_assignments` VALUES (21, 21, 1, '2026-02-13');
INSERT INTO `employee_position_assignments` VALUES (22, 22, 1, '2022-09-13');
INSERT INTO `employee_position_assignments` VALUES (23, 23, 1, '2024-09-13');
INSERT INTO `employee_position_assignments` VALUES (24, 24, 1, '2021-09-15');
INSERT INTO `employee_position_assignments` VALUES (25, 25, 1, '2022-09-15');

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
) ENGINE = InnoDB AUTO_INCREMENT = 31 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
INSERT INTO `employee_schedule_assignments` VALUES (18, 16, 1, '2026-08-01');
INSERT INTO `employee_schedule_assignments` VALUES (19, 17, 3, '2019-09-13');
INSERT INTO `employee_schedule_assignments` VALUES (20, 18, 1, '2025-10-01');
INSERT INTO `employee_schedule_assignments` VALUES (21, 19, 3, '2024-09-13');
INSERT INTO `employee_schedule_assignments` VALUES (22, 20, 4, '2026-03-13');
INSERT INTO `employee_schedule_assignments` VALUES (23, 21, 4, '2026-02-13');
INSERT INTO `employee_schedule_assignments` VALUES (24, 22, 4, '2022-09-13');
INSERT INTO `employee_schedule_assignments` VALUES (25, 23, 3, '2024-09-13');
INSERT INTO `employee_schedule_assignments` VALUES (26, 24, 6, '2021-09-15');
INSERT INTO `employee_schedule_assignments` VALUES (27, 25, 5, '2022-09-15');
INSERT INTO `employee_schedule_assignments` VALUES (28, 23, 1, '2026-08-31');
INSERT INTO `employee_schedule_assignments` VALUES (29, 8, 4, '2026-10-25');
INSERT INTO `employee_schedule_assignments` VALUES (30, 21, 2, '2026-10-25');

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
) ENGINE = InnoDB AUTO_INCREMENT = 26 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
INSERT INTO `employees` VALUES (17, 'Обозный', 'Алексей', '', 'Обозный А.', 'worker');
INSERT INTO `employees` VALUES (18, 'Мазаник', 'Сергей', 'Николаевич', 'Мазаник С. Н.', 'worker');
INSERT INTO `employees` VALUES (19, 'Гридчин', 'Артем', '', 'Гридчин А.', 'worker');
INSERT INTO `employees` VALUES (20, 'Сеньковец', 'Александр', '', 'Сеньковец А.', 'worker');
INSERT INTO `employees` VALUES (21, 'Казак', 'Андрей', '', 'Казак А.', 'worker');
INSERT INTO `employees` VALUES (22, 'Алехин', 'Вадим', '', 'Алехин В.', 'worker');
INSERT INTO `employees` VALUES (23, 'Борисик', 'Роман', '', 'Борисик Р.', 'worker');
INSERT INTO `employees` VALUES (24, 'Нажмудинов', 'Джиеншо', 'Изатулоевич', 'Нажмудинов Д. И.', 'worker');
INSERT INTO `employees` VALUES (25, 'Хмельницкий', 'Дмитрий', '', 'Хмельницкий Д.', 'worker');

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
) ENGINE = InnoDB AUTO_INCREMENT = 28 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
INSERT INTO `employment_periods` VALUES (17, 17, '2019-09-13', NULL);
INSERT INTO `employment_periods` VALUES (18, 18, '2025-10-01', NULL);
INSERT INTO `employment_periods` VALUES (19, 19, '2024-09-13', NULL);
INSERT INTO `employment_periods` VALUES (20, 20, '2026-03-13', '2026-09-24');
INSERT INTO `employment_periods` VALUES (21, 21, '2026-02-13', NULL);
INSERT INTO `employment_periods` VALUES (22, 22, '2022-09-13', NULL);
INSERT INTO `employment_periods` VALUES (23, 23, '2024-09-13', NULL);
INSERT INTO `employment_periods` VALUES (24, 24, '2021-09-15', NULL);
INSERT INTO `employment_periods` VALUES (25, 25, '2022-09-15', NULL);
INSERT INTO `employment_periods` VALUES (26, 8, '2026-09-26', NULL);
INSERT INTO `employment_periods` VALUES (27, 20, '2026-09-26', NULL);

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
) ENGINE = InnoDB AUTO_INCREMENT = 23 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of equipment
-- ----------------------------
INSERT INTO `equipment` VALUES (1, 1, 7, 'Versor P/S 100k3/4/6', '15', '2013-01-01', NULL, 'strict_schedule', 0);
INSERT INTO `equipment` VALUES (2, 1, 7, 'Jagenberg Diana 105.1', '9', '2010-01-01', NULL, 'strict_schedule', 1);
INSERT INTO `equipment` VALUES (3, 1, 7, 'Bobst Pharma 36', '38', '2020-01-01', NULL, 'manual_only', 3);
INSERT INTO `equipment` VALUES (4, 1, 7, 'Versor Pasio', '43', '2026-06-01', NULL, 'strict_schedule', 2);
INSERT INTO `equipment` VALUES (5, 1, 7, 'L1000', '41', '2026-06-01', NULL, 'strict_schedule', 5);
INSERT INTO `equipment` VALUES (6, 1, 7, 'L1000 (2)', '42', '2026-06-01', NULL, 'strict_schedule', 6);
INSERT INTO `equipment` VALUES (7, 1, 10, 'Вклейка окошек', '16', '2026-06-06', NULL, 'strict_schedule', 4);
INSERT INTO `equipment` VALUES (8, 2, 7, 'Bobst 102-1', '10', '2026-07-09', NULL, 'strict_schedule', 1);
INSERT INTO `equipment` VALUES (9, 2, 7, 'Bobst 102-2', '11', '2026-07-09', NULL, 'strict_schedule', 2);
INSERT INTO `equipment` VALUES (10, 5, 7, 'Сменный мастер', '', '2026-07-09', NULL, 'strict_schedule', 1);
INSERT INTO `equipment` VALUES (12, 5, 11, 'Производство', '', '2026-07-09', NULL, 'strict_schedule', 2);
INSERT INTO `equipment` VALUES (13, 1, 8, 'Diana Eye', '56', '2026-07-13', NULL, 'strict_schedule', 7);
INSERT INTO `equipment` VALUES (14, 5, 1, 'Заместитель начальника', '', '2020-07-13', NULL, 'strict_schedule', 3);
INSERT INTO `equipment` VALUES (15, 5, 11, 'Участок вырубки и тиснения', '', '2024-07-13', NULL, 'strict_schedule', 4);
INSERT INTO `equipment` VALUES (16, 5, 11, 'Участок плоской печати', '', '2025-07-13', NULL, 'strict_schedule', 5);
INSERT INTO `equipment` VALUES (17, 5, 11, 'Планирование', '', '2010-07-13', NULL, 'strict_schedule', 6);
INSERT INTO `equipment` VALUES (18, 6, 7, 'Hibis', '35', '2021-09-27', NULL, 'strict_schedule', 1);
INSERT INTO `equipment` VALUES (19, 4, 8, 'Heidelberg SP 102-2-P', '45', '2026-01-30', NULL, 'strict_schedule', 1);
INSERT INTO `equipment` VALUES (20, 4, 7, 'Rolland R 706 3B', '2', '2014-01-01', NULL, 'strict_schedule', 2);
INSERT INTO `equipment` VALUES (21, 4, 7, 'Rolland 706', '37', '2014-01-01', NULL, 'strict_schedule', 3);
INSERT INTO `equipment` VALUES (22, 2, 7, ' Heidelberg SBD', '19', '2006-10-27', NULL, 'strict_schedule', 3);

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
) ENGINE = InnoDB AUTO_INCREMENT = 170 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of equipment_daily_plan
-- ----------------------------
INSERT INTO `equipment_daily_plan` VALUES (1, 3, '2026-05-01', 1, 0);
INSERT INTO `equipment_daily_plan` VALUES (2, 2, '2026-05-01', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (109, 15, '2026-09-05', 14, 0);
INSERT INTO `equipment_daily_plan` VALUES (110, 1, '2026-09-05', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (112, 12, '2026-09-05', 14, 0);
INSERT INTO `equipment_daily_plan` VALUES (114, 3, '2026-09-12', 1, 0);
INSERT INTO `equipment_daily_plan` VALUES (115, 1, '2026-09-13', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (116, 2, '2026-09-13', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (117, 2, '2026-09-14', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (118, 4, '2026-09-14', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (119, 3, '2026-09-15', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (120, 3, '2026-09-16', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (121, 3, '2026-09-14', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (122, 3, '2026-09-13', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (124, 1, '2026-09-15', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (126, 5, '2026-09-26', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (127, 4, '2026-09-26', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (128, 13, '2026-09-25', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (130, 3, '2026-10-07', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (131, 3, '2026-10-12', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (132, 3, '2026-10-12', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (133, 3, '2026-10-11', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (134, 13, '2026-10-12', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (135, 13, '2026-10-15', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (136, 13, '2026-10-16', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (137, 13, '2026-10-17', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (139, 13, '2026-10-19', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (142, 3, '2026-10-19', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (143, 3, '2026-10-20', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (144, 3, '2026-10-21', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (145, 3, '2026-10-22', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (146, 3, '2026-10-23', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (147, 3, '2026-10-24', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (148, 3, '2026-10-16', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (150, 13, '2026-10-20', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (151, 13, '2026-10-21', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (152, 13, '2026-10-22', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (155, 13, '2026-10-23', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (158, 13, '2026-10-25', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (159, 3, '2026-10-25', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (160, 13, '2026-10-26', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (161, 13, '2026-10-27', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (163, 3, '2026-10-28', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (164, 3, '2026-10-31', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (165, 3, '2026-10-31', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (166, 3, '2026-11-01', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (167, 3, '2026-11-01', 2, 1);
INSERT INTO `equipment_daily_plan` VALUES (168, 13, '2026-10-29', 1, 1);
INSERT INTO `equipment_daily_plan` VALUES (169, 13, '2026-10-30', 1, 1);

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
) ENGINE = InnoDB AUTO_INCREMENT = 27 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
INSERT INTO `equipment_schedule_history` VALUES (11, 7, 7, '2026-06-06');
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
INSERT INTO `equipment_schedule_history` VALUES (22, 18, 10, '2021-09-27');
INSERT INTO `equipment_schedule_history` VALUES (23, 19, 8, '2026-01-30');
INSERT INTO `equipment_schedule_history` VALUES (24, 20, 7, '2014-01-01');
INSERT INTO `equipment_schedule_history` VALUES (25, 21, 7, '2014-01-01');
INSERT INTO `equipment_schedule_history` VALUES (26, 22, 7, '2006-10-27');

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
) ENGINE = InnoDB AUTO_INCREMENT = 28 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of equipment_staffing_history
-- ----------------------------
INSERT INTO `equipment_staffing_history` VALUES (1, 1, 'strict_schedule', '2020-01-03');
INSERT INTO `equipment_staffing_history` VALUES (2, 2, 'strict_schedule', '2010-01-01');
INSERT INTO `equipment_staffing_history` VALUES (3, 3, 'strict_schedule', '2010-01-01');
INSERT INTO `equipment_staffing_history` VALUES (7, 1, 'strict_schedule', '2021-05-05');
INSERT INTO `equipment_staffing_history` VALUES (8, 5, 'strict_schedule', '2026-06-01');
INSERT INTO `equipment_staffing_history` VALUES (9, 5, 'manual_only', '2025-06-06');
INSERT INTO `equipment_staffing_history` VALUES (10, 1, 'strict_schedule', '2026-06-06');
INSERT INTO `equipment_staffing_history` VALUES (11, 6, 'strict_schedule', '2026-06-01');
INSERT INTO `equipment_staffing_history` VALUES (12, 4, 'strict_schedule', '2026-06-03');
INSERT INTO `equipment_staffing_history` VALUES (13, 7, 'manual_only', '2026-06-06');
INSERT INTO `equipment_staffing_history` VALUES (14, 8, 'strict_schedule', '2026-07-09');
INSERT INTO `equipment_staffing_history` VALUES (15, 9, 'strict_schedule', '2026-07-09');
INSERT INTO `equipment_staffing_history` VALUES (16, 10, 'strict_schedule', '2026-07-09');
INSERT INTO `equipment_staffing_history` VALUES (17, 12, 'strict_schedule', '2026-07-09');
INSERT INTO `equipment_staffing_history` VALUES (18, 13, 'strict_schedule', '2026-07-13');
INSERT INTO `equipment_staffing_history` VALUES (19, 14, 'strict_schedule', '2020-07-13');
INSERT INTO `equipment_staffing_history` VALUES (20, 15, 'strict_schedule', '2024-07-13');
INSERT INTO `equipment_staffing_history` VALUES (21, 16, 'strict_schedule', '2025-07-13');
INSERT INTO `equipment_staffing_history` VALUES (22, 17, 'strict_schedule', '2010-07-13');
INSERT INTO `equipment_staffing_history` VALUES (23, 18, 'strict_schedule', '2021-09-27');
INSERT INTO `equipment_staffing_history` VALUES (24, 19, 'strict_schedule', '2026-01-30');
INSERT INTO `equipment_staffing_history` VALUES (25, 20, 'strict_schedule', '2014-01-01');
INSERT INTO `equipment_staffing_history` VALUES (26, 21, 'strict_schedule', '2014-01-01');
INSERT INTO `equipment_staffing_history` VALUES (27, 22, 'strict_schedule', '2006-10-27');

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
-- Records of schedule_cycles
-- ----------------------------
INSERT INTO `schedule_cycles` VALUES (1, '2/2 (день, ночь)', 8);
INSERT INTO `schedule_cycles` VALUES (2, '2/2 (день)', 4);
INSERT INTO `schedule_cycles` VALUES (3, '5/2', 7);
INSERT INTO `schedule_cycles` VALUES (4, 'Круглосуточно (2 смены)', 1);
INSERT INTO `schedule_cycles` VALUES (5, 'Ежедневно', 1);
INSERT INTO `schedule_cycles` VALUES (10, 'Станок 3 смены', 1);

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
) ENGINE = InnoDB AUTO_INCREMENT = 249 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of schedule_overrides
-- ----------------------------
INSERT INTO `schedule_overrides` VALUES (1, 3, '2026-05-01', 1, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (3, 4, '2026-05-01', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (4, 1, '2026-05-05', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (5, 3, '2026-05-02', 2, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (9, 4, '2026-05-02', 1, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (10, 2, '2026-05-05', 2, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (11, 4, '2026-05-07', 1, 2, 2, 'Нет работы', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (12, 2, '2026-05-07', 1, 1, 0, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (15, 4, '2026-05-07', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (22, 3, '2026-05-02', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (26, 16, '2026-09-04', 3, 6, 2, 'ввва', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (35, 8, '2026-09-03', 1, 2, 2, '🔄 Перевод с Versor Pasio', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (37, 6, '2026-09-03', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (39, 6, '2026-09-01', 1, 1, 2, '🔄 Перевод с Diana', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (41, 3, '2026-09-01', 1, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (51, 7, '2026-09-11', 1, 2, 2, '🔄 Перевод с Diana Eye', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (67, 14, '2026-09-05', 14, 15, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (70, 11, '2026-09-05', 14, 12, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (71, 6, '2026-09-04', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (76, 18, '2026-09-14', 2, 1, 2, '🔄 Перевод с Versor Pasio', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (83, 22, '2026-09-16', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (86, 22, '2026-09-19', 2, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (91, 1, '2026-09-15', 2, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (101, 4, '2026-09-15', 2, 2, 2, '🔄 Перевод с Versor P/S 100k3/4/6', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (111, 20, '2026-09-17', 1, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (112, 20, '2026-09-18', 1, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (113, 20, '2026-09-21', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (114, 23, '2026-09-21', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (115, 23, '2026-09-21', 2, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (116, 1, '2026-09-21', 2, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (117, 8, '2026-09-26', 1, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (118, 23, '2026-09-25', 2, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (119, 23, '2026-09-26', 2, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (120, 23, '2026-09-25', 1, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (121, 23, '2026-09-26', 1, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (122, 23, '2026-09-27', 1, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (123, 17, '2026-09-26', 2, 3, 2, '🔄 Перевод с Versor Pasio', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (125, 6, '2026-09-27', 1, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (126, 17, '2026-09-28', 1, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (127, 1, '2026-09-26', 2, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (128, 18, '2026-09-28', 2, 5, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (129, 7, '2026-09-25', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (130, 20, '2026-09-29', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (131, 7, '2026-09-28', 2, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (132, 1, '2026-09-29', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (133, 1, '2026-09-29', 2, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (134, 1, '2026-09-30', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (135, 7, '2026-09-30', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (136, 1, '2026-09-30', 2, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (137, 23, '2026-09-29', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (138, 23, '2026-09-30', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (139, 23, '2026-09-29', 2, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (140, 23, '2026-09-30', 2, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (141, 4, '2026-09-30', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (142, 18, '2026-10-06', 2, 6, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (143, 7, '2026-10-03', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (144, 7, '2026-10-04', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (145, 1, '2026-10-09', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (146, 1, '2026-10-10', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (147, 1, '2026-10-07', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (148, 1, '2026-10-08', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (149, 6, '2026-10-07', 2, 1, 2, '🔄 Перевод с Jagenberg Diana 105.1', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (153, 23, '2026-10-07', 2, 2, 2, '🔄 Перевод с Bobst Pharma 36', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (155, 24, '2026-10-07', 1, 3, 2, '🔄 Перевод с L1000', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (157, 22, '2026-10-09', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (158, 22, '2026-10-10', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (159, 22, '2026-10-09', 2, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (160, 22, '2026-10-10', 2, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (161, 24, '2026-10-08', 1, 3, 2, '🔄 Перевод с L1000', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (163, 19, '2026-10-09', 1, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (164, 6, '2026-10-08', 2, 1, 2, '🔄 Перевод с Jagenberg Diana 105.1', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (166, 6, '2026-10-12', 1, 1, 2, '🔄 Перевод с Jagenberg Diana 105.1', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (168, 23, '2026-10-12', 1, 2, 2, '🔄 Перевод с Bobst Pharma 36', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (170, 23, '2026-10-11', 1, 2, 2, '🔄 Перевод с Bobst Pharma 36', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (172, 6, '2026-10-11', 1, 1, 2, '🔄 Перевод с Jagenberg Diana 105.1', 0, NULL);
INSERT INTO `schedule_overrides` VALUES (174, 1, '2026-10-11', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (175, 1, '2026-10-12', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (176, 1, '2026-10-14', 1, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (183, 1, '2026-10-16', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (184, 7, '2026-10-16', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (190, 23, '2026-10-15', 2, 2, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (191, 1, '2026-10-15', 2, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (192, 3, '2026-10-19', 1, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (198, 22, '2026-10-21', 1, 3, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (199, 22, '2026-10-22', 1, 3, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (200, 23, '2026-10-23', 1, 3, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (201, 23, '2026-10-24', 1, 3, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (203, 23, '2026-10-16', 2, 2, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (204, 6, '2026-10-16', 2, 1, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (205, 6, '2026-10-15', 2, 1, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (206, 7, '2026-10-20', 1, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (208, 1, '2026-10-24', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (210, 7, '2026-10-25', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (211, 7, '2026-10-24', 2, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (212, 23, '2026-10-21', 1, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (218, 1, '2026-10-23', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (222, 6, '2026-10-25', 2, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (223, 19, '2026-10-26', 2, 2, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (224, 22, '2026-10-25', 2, 3, 2, NULL, 0, 1);
INSERT INTO `schedule_overrides` VALUES (225, 22, '2026-10-26', 2, 3, 2, NULL, 0, 1);
INSERT INTO `schedule_overrides` VALUES (227, 19, '2026-10-24', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (228, 1, '2026-10-25', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (229, 23, '2026-10-26', 1, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (230, 2, '2026-10-27', 2, 22, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (231, 1, '2026-10-27', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (232, 3, '2026-10-27', 1, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (233, 2, '2026-10-28', 2, 22, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (234, 8, '2026-10-28', 2, 1, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (235, 6, '2026-10-28', 1, 1, 2, NULL, 0, 1);
INSERT INTO `schedule_overrides` VALUES (236, 24, '2026-10-28', 1, 2, 2, NULL, 0, 1);
INSERT INTO `schedule_overrides` VALUES (237, 1, '2026-10-28', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (238, 7, '2026-10-28', 1, 13, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (239, 22, '2026-10-29', 1, 3, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (240, 22, '2026-10-30', 1, 3, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (241, 1, '2026-10-29', 2, 3, 2, NULL, 0, NULL);
INSERT INTO `schedule_overrides` VALUES (242, 19, '2026-10-31', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (244, 19, '2026-11-01', 1, NULL, 2, 'Отмена мастером в карточке смены', 1, NULL);
INSERT INTO `schedule_overrides` VALUES (245, 23, '2026-11-01', 1, 2, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (246, 23, '2026-10-31', 1, 2, 2, NULL, 0, 2);
INSERT INTO `schedule_overrides` VALUES (247, 7, '2026-10-29', 1, 2, 2, NULL, 0, 1);
INSERT INTO `schedule_overrides` VALUES (248, 7, '2026-10-30', 1, 2, 2, NULL, 0, 1);

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
) ENGINE = InnoDB AUTO_INCREMENT = 12 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
INSERT INTO `schedule_templates` VALUES (8, 'Дневные, 12 часов', 5, '2010-01-01');
INSERT INTO `schedule_templates` VALUES (10, '3 смены круглосуточно', 10, '2026-05-31');
INSERT INTO `schedule_templates` VALUES (11, 'Пятидневка', 3, '2009-12-28');

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
-- Records of status_color_palette
-- ----------------------------
INSERT INTO `status_color_palette` VALUES (0, 'Не требуется (Отмена)', '#F2F2F2', '#808080');
INSERT INTO `status_color_palette` VALUES (1, 'Не требуется (Вне плана)', '#F9F9F9', '#A0A0A0');
INSERT INTO `status_color_palette` VALUES (2, 'Укомплектовано', '#E8F5E9', '#2E7D32');
INSERT INTO `status_color_palette` VALUES (3, 'ТРЕБУЕТСЯ ПЕРСОНАЛ', '#FFEBEE', '#C62828');
INSERT INTO `status_color_palette` VALUES (4, 'НУЖНО НАЗНАЧЕНИЕ', '#FFF3E0', '#EF6C00');

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
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of work_areas
-- ----------------------------
INSERT INTO `work_areas` VALUES (1, 'Участок склейки', '{\"stamp\": true, \"colors\": false, \"element\": false, \"duration\": true, \"material\": false, \"order_name\": true, \"tech_notes\": false, \"date_time_end\": true, \"planned_output\": true, \"sheet_trimming\": false, \"date_time_start\": true}', 0);
INSERT INTO `work_areas` VALUES (2, 'Участок вырубки', '{\"stamp\": true, \"colors\": false, \"element\": true, \"duration\": true, \"material\": false, \"order_name\": true, \"tech_notes\": false, \"date_time_end\": true, \"planned_output\": true, \"sheet_trimming\": false, \"date_time_start\": true}', 1);
INSERT INTO `work_areas` VALUES (4, 'Участок плоской печати', '{\"stamp\": false, \"colors\": true, \"element\": true, \"duration\": true, \"material\": true, \"order_name\": true, \"tech_notes\": true, \"date_time_end\": true, \"planned_output\": true, \"sheet_trimming\": true, \"date_time_start\": true}', 3);
INSERT INTO `work_areas` VALUES (5, 'Управление производством', '{\"stamp\": false, \"colors\": true, \"element\": true, \"duration\": true, \"material\": true, \"order_name\": true, \"tech_notes\": true, \"date_time_end\": true, \"planned_output\": true, \"sheet_trimming\": false, \"date_time_start\": true}', 4);
INSERT INTO `work_areas` VALUES (6, 'Участок лакировки', '{\"stamp\": false, \"colors\": false, \"element\": true, \"duration\": true, \"material\": false, \"order_name\": true, \"tech_notes\": false, \"date_time_end\": true, \"planned_output\": true, \"sheet_trimming\": false, \"date_time_start\": true}', 5);

-- ----------------------------
-- Procedure structure for GetAvailableEmployeesForShift
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetAvailableEmployeesForShift`;
delimiter ;;
CREATE PROCEDURE `GetAvailableEmployeesForShift`(IN p_target_date DATE,
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
CREATE PROCEDURE `GetEquipmentShiftCard`(IN p_target_date DATE,
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
CREATE PROCEDURE `GetEquipmentStaffingForPeriod`(IN p_start_date DATE,
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
CREATE PROCEDURE `GetEquipmentStaffingReport`(IN p_start_date DATE,
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
