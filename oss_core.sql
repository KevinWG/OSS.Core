/*
Navicat MySQL Data Transfer

Source Server         : test
Source Server Version : 50718
Source Host           : 13.67.112.156:3306
Source Database       : oss_core

Target Server Type    : MYSQL
Target Server Version : 50718
File Encoding         : 65001

Date: 2017-09-15 15:07:54
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for admin_info
-- ----------------------------
DROP TABLE IF EXISTS `admin_info`;
CREATE TABLE `admin_info` (
  `Id` bigint(8) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `uid` bigint(8) DEFAULT NULL,
  `status` bigint(8) DEFAULT NULL,
  `create_time` bigint(8) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for user_info
-- ----------------------------
DROP TABLE IF EXISTS `user_info`;
CREATE TABLE `user_info` (
  `Id` bigint(8) NOT NULL AUTO_INCREMENT,
  `mobile` varchar(100) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `pass_word` varchar(64) DEFAULT NULL,
  `nick_name` varchar(255) DEFAULT NULL COMMENT '昵称',
  `app_source` varchar(300) DEFAULT NULL,
  `app_version` varchar(50) DEFAULT NULL,
  `status` int(4) DEFAULT NULL,
  `create_time` bigint(8) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;
SET FOREIGN_KEY_CHECKS=1;
