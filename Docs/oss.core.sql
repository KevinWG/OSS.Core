CREATE DATABASE  IF NOT EXISTS `oss.core` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `oss.core`;
-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: localhost    Database: oss.core
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `b_permit_func`
--

DROP TABLE IF EXISTS `b_permit_func`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `b_permit_func` (
  `id` bigint NOT NULL DEFAULT '0',
  `code` varchar(100) NOT NULL COMMENT '角色名称',
  `parent_code` varchar(100) DEFAULT '',
  `title` varchar(200) NOT NULL COMMENT '角色名称',
  `status` int NOT NULL COMMENT '角色状态',
  `owner_uid` bigint NOT NULL COMMENT ' 【创建/归属】用户Id',
  `add_time` bigint NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='功能权限表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `b_permit_func`
--

LOCK TABLES `b_permit_func` WRITE;
/*!40000 ALTER TABLE `b_permit_func` DISABLE KEYS */;
INSERT INTO `b_permit_func` VALUES (587798735089664,'notify',NULL,'通知中心管理',0,1,1658241550),(587976798056448,'notify_template_add','notify_template_list','添加模板',0,1,1658285022),(587802191515648,'notify_template_list','notify','通知模板列表',0,1,1658242394),(587977192275968,'notify_template_update','notify_template_list','编辑模板',0,1,1658285119),(57,'portal',NULL,'用户管理',0,1,1629774504),(591261296566272,'portal_admin_create','portal_admin_list','新增管理员',0,1,1659086902),(591261093355520,'portal_admin_list','portal','管理员列表',0,1,1659086852),(591261385818112,'portal_admin_lock','portal_admin_list','锁定管理员',0,1,1659086924),(591262922158080,'portal_admin_settype','portal_admin_list','设置/取消超管权限',0,1,1659087299),(591261753872384,'portal_admin_unlock','portal_admin_list','解锁管理员',0,1,1659087014),(587793488457728,'portal_func_operate','portal_permit','权限项管理',0,1,1658240269),(593024106070016,'portal_grant_role_change','portal_role_list','修改角色权限项',0,1,1659517275),(593023862124544,'portal_grant_role_permits','portal_role_list','查看角色权限项',0,1,1659517216),(46,'portal_permit','portal','权限管理',0,1,1629774504),(592308063207424,'portal_role_active','portal_role_list','【作废/启用】角色',0,1,1659342460),(592307961278464,'portal_role_add','portal_role_list','【添加/修改】角色',0,1,1659342435),(592309084315648,'portal_role_bind_delete','portal_user_roles','删除角色绑定',0,1,1659342710),(592309006045184,'portal_role_bind_user','portal_user_roles','添加角色绑定',0,1,1659342690),(592307848298496,'portal_role_list','portal_permit','角色管理',0,1,1659342408),(588417913917440,'portal_setting_auth','portal','授权登录配置',0,1,1658392717),(590258058792960,'portal_user_add','portal_user_list','添加用户',0,1,1658841971),(590257993277440,'portal_user_list','portal','用户列表',0,1,1658841955),(590258133041152,'portal_user_lock','portal_user_list','锁定用户',0,1,1658841989),(592308906213376,'portal_user_roles','portal_permit','用户角色绑定管理',0,1,1659342666),(590258185162752,'portal_user_unlock','portal_user_list','解锁用户',0,1,1658842002);
/*!40000 ALTER TABLE `b_permit_func` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `b_permit_role`
--

DROP TABLE IF EXISTS `b_permit_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `b_permit_role` (
  `id` bigint NOT NULL COMMENT '角色Id',
  `name` varchar(256) NOT NULL COMMENT '角色名称',
  `status` int NOT NULL COMMENT '角色状态',
  `owner_uid` bigint NOT NULL COMMENT ' 【创建/归属】用户Id',
  `add_time` bigint NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='角色表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `b_permit_role`
--

LOCK TABLES `b_permit_role` WRITE;
/*!40000 ALTER TABLE `b_permit_role` DISABLE KEYS */;
INSERT INTO `b_permit_role` VALUES (592680459898880,'测试角色名称',0,1,1659433377);
/*!40000 ALTER TABLE `b_permit_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `b_permit_role_func`
--

DROP TABLE IF EXISTS `b_permit_role_func`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `b_permit_role_func` (
  `id` bigint NOT NULL COMMENT '关联主键Id',
  `role_id` bigint NOT NULL COMMENT '角色Id',
  `func_code` varchar(200) NOT NULL COMMENT '权限码',
  `data_level` int DEFAULT NULL COMMENT '数据权限',
  `status` int NOT NULL COMMENT '状态（通用）',
  `add_time` bigint NOT NULL COMMENT '创建时间',
  `owner_uid` bigint NOT NULL COMMENT '【创建/归属】用户Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='角色权限关联表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `b_permit_role_func`
--

LOCK TABLES `b_permit_role_func` WRITE;
/*!40000 ALTER TABLE `b_permit_role_func` DISABLE KEYS */;
/*!40000 ALTER TABLE `b_permit_role_func` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `b_permit_role_user`
--

DROP TABLE IF EXISTS `b_permit_role_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `b_permit_role_user` (
  `id` bigint NOT NULL,
  `u_id` bigint DEFAULT NULL COMMENT '用户Id',
  `role_id` bigint DEFAULT NULL COMMENT '角色Id',
  `status` int DEFAULT NULL COMMENT '状态',
  `add_time` bigint NOT NULL COMMENT '创建时间',
  `owner_uid` bigint NOT NULL COMMENT '  【创建/归属】用户Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='角色和用户关联表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `b_permit_role_user`
--

LOCK TABLES `b_permit_role_user` WRITE;
/*!40000 ALTER TABLE `b_permit_role_user` DISABLE KEYS */;
/*!40000 ALTER TABLE `b_permit_role_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `b_portal_admin`
--

DROP TABLE IF EXISTS `b_portal_admin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `b_portal_admin` (
  `id` bigint NOT NULL,
  `admin_name` varchar(300) NOT NULL COMMENT '管理员名称',
  `avatar` varchar(300) DEFAULT NULL COMMENT '管理员头像',
  `admin_type` tinyint NOT NULL DEFAULT '0' COMMENT '是否超级管理员',
  `status` int NOT NULL COMMENT '管理员状态',
  `owner_uid` bigint NOT NULL COMMENT '【创建/归属】用户id',
  `add_time` bigint NOT NULL COMMENT '创建时间戳',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='管理员表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `b_portal_admin`
--

LOCK TABLES `b_portal_admin` WRITE;
/*!40000 ALTER TABLE `b_portal_admin` DISABLE KEYS */;
INSERT INTO `b_portal_admin` VALUES (1,'OSS.Admin','',100,0,1,1522118183),(590201613512704,'测试管理员',NULL,0,-100,1,1659085878);
/*!40000 ALTER TABLE `b_portal_admin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `b_portal_user`
--

DROP TABLE IF EXISTS `b_portal_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `b_portal_user` (
  `id` bigint NOT NULL,
  `mobile` varchar(100) DEFAULT NULL COMMENT '登录手机号',
  `email` varchar(255) DEFAULT NULL COMMENT '登录邮箱',
  `pass_word` varchar(64) DEFAULT NULL COMMENT '登录密码',
  `nick_name` varchar(255) DEFAULT NULL COMMENT '昵称',
  `avatar` varchar(255) DEFAULT NULL COMMENT '头像',
  `status` int NOT NULL DEFAULT '0' COMMENT '用户状态',
  `owner_uid` bigint DEFAULT '0' COMMENT '【创建/归属】用户Id',
  `add_time` bigint unsigned NOT NULL DEFAULT '0' COMMENT '创建时间',
  PRIMARY KEY (`id`),
  KEY `user_mobile_index` (`mobile`),
  KEY `user_email_index` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='用户表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `b_portal_user`
--

LOCK TABLES `b_portal_user` WRITE;
/*!40000 ALTER TABLE `b_portal_user` DISABLE KEYS */;
INSERT INTO `b_portal_user` VALUES (1,'','admin@osscore.com','96e79218965eb72c92a549dd5a330112','OSSCore',NULL,0,0,1618910709),(590201613512704,NULL,'1@qq.com',NULL,'testAdmin',NULL,-100,1,1658828190);
/*!40000 ALTER TABLE `b_portal_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `p_notify_template`
--

DROP TABLE IF EXISTS `p_notify_template`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `p_notify_template` (
  `id` bigint NOT NULL COMMENT '模板编号',
  `notify_type` int NOT NULL COMMENT '通知类型',
  `notify_channel` int NOT NULL COMMENT '模板编号',
  `title` varchar(200) NOT NULL COMMENT '模板编号',
  `channel_template_code` varchar(200) NOT NULL COMMENT '模板编号',
  `channel_sender` varchar(200) DEFAULT NULL COMMENT '发送业务账号或通道账号',
  `content` text NOT NULL COMMENT '配置的值',
  `is_html` tinyint NOT NULL COMMENT '状态',
  `sign_name` text,
  `status` int NOT NULL COMMENT '状态',
  `owner_uid` bigint NOT NULL DEFAULT '0' COMMENT '【创建/归属】用户Id',
  `add_time` bigint unsigned NOT NULL DEFAULT '0' COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='通知模板表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `p_notify_template`
--

LOCK TABLES `p_notify_template` WRITE;
/*!40000 ALTER TABLE `p_notify_template` DISABLE KEYS */;
INSERT INTO `p_notify_template` VALUES (1,20,2,'注册登录验证短信','','','验证码:{code}，请注意不要泄露给其他人',0,'',0,1,1658492646),(2,10,2,'注册登录邮件验证码','','','你的验证码为：{code}，请不要告诉其他人',0,'',0,1,1658493562);
/*!40000 ALTER TABLE `p_notify_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_dir_config`
--

DROP TABLE IF EXISTS `sys_dir_config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sys_dir_config` (
  `id` varchar(200) NOT NULL,
  `module` varchar(200) NOT NULL,
  `config_val` text NOT NULL COMMENT '配置的值',
  `owner_uid` bigint NOT NULL DEFAULT '0' COMMENT '【创建/归属】用户Id',
  `add_time` bigint unsigned NOT NULL DEFAULT '0' COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='系统配置表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_dir_config`
--

LOCK TABLES `sys_dir_config` WRITE;
/*!40000 ALTER TABLE `sys_dir_config` DISABLE KEYS */;
INSERT INTO `sys_dir_config` VALUES ('portal_auth_setting','','{\"SmsTemplateId\":\"1\",\"EmailTemplateId\":\"2\",\"BindSmsTemplateId\":\"\",\"BindEmailTemplateId\":\"\"}',1,1657974198);
/*!40000 ALTER TABLE `sys_dir_config` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-08-08 22:43:00
