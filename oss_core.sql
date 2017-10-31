/*
Navicat MySQL Data Transfer

Source Server         : osscore_test
Source Server Version : 50718
Source Host           : 13.67.112.156:3306
Source Database       : oss_core

Target Server Type    : MYSQL
Target Server Version : 50718
File Encoding         : 65001

Date: 2017-10-31 23:30:45
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
  `status` bigint(8) NOT NULL DEFAULT '0',
  `create_time` bigint(8) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for oauth_user_info
-- ----------------------------
DROP TABLE IF EXISTS `oauth_user_info`;
CREATE TABLE `oauth_user_info` (
  `Id` bigint(8) NOT NULL AUTO_INCREMENT,
  `user_id` bigint(8) NOT NULL DEFAULT '0' COMMENT '用户Id',
  `tenant_id` bigint(8) DEFAULT NULL COMMENT '租户Id',
  `app_source` varchar(255) DEFAULT NULL COMMENT '应用来源',
  `status` tinyint(2) DEFAULT '0' COMMENT '当前状态',
  `app_user_id` varchar(20) DEFAULT NULL,
  `app_union_id` varchar(255) DEFAULT NULL COMMENT '第三方平台下的多应用唯一用户Id',
  `access_token` varchar(255) DEFAULT NULL COMMENT '授权AccessToken',
  `refresh_token` varchar(255) DEFAULT NULL COMMENT '刷新Token信息',
  `expire_date` bigint(8) DEFAULT '0',
  `platform` int(4) DEFAULT NULL COMMENT '第三方平台来源类型',
  `nick_name` varchar(255) DEFAULT NULL,
  `sex` tinyint(2) DEFAULT NULL COMMENT '性别',
  `head_img` varchar(500) DEFAULT NULL COMMENT '头像',
  `create_time` bigint(8) DEFAULT '0' COMMENT '创建时间戳',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for user_info
-- ----------------------------
DROP TABLE IF EXISTS `user_info`;
CREATE TABLE `user_info` (
  `id` bigint(8) NOT NULL AUTO_INCREMENT,
  `mobile` varchar(100) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `pass_word` varchar(64) DEFAULT NULL,
  `tenant_id` bigint(8) unsigned NOT NULL DEFAULT '0',
  `status` int(4) NOT NULL DEFAULT '0',
  `nick_name` varchar(255) DEFAULT NULL COMMENT '昵称',
  `head_img` varchar(255) DEFAULT NULL,
  `app_source` varchar(300) DEFAULT NULL,
  `app_version` varchar(50) DEFAULT NULL,
  `create_time` bigint(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;
SET FOREIGN_KEY_CHECKS=1;
