# OSS.Core
一个完整的.Net Core 领域框架项目。
还在初级实现中，将会串通OSS.Social 和 OSS.PayCenter SDK，敬请期待！


## 文件结构介绍
**Common** --  基础类库，包含通用枚举实体等，通用验证签名配置帮助类等

OSS.Core.Context -- 系统上下文管理，主要包含：
	1. AppReqContext  
	   应用请求上下文管理，包含当前请求的 AppId，以及对应的相关租户Id，签名密钥等
	2. TenantContext 
		租户上下文信息，租户的基本信息
	3. UserContext 
		主要是当前请求的授权用户上下文信息

OSS.Core.Infrastructure
	OSSCore项目通用基础类库，主要包含基础实体，辅助类。

**Apis**  -- 接口服务层

	OSS.Core.RepDapper -- 项目的数据仓储层（当前使用的是 Dapper+Mysql 形式）。
		为上层数据读写等提供支撑，可以替换 其他ORM+数据库 实现方式		
	OSS.Core.Services -- 核心业务逻辑层，主要实现领域业务逻辑。
	OSS.Core.WebApi -- 提供对外接口实现，参数验证，应用授权验证等

	这一层主要是提供业务服务接口，单体解决方案下通过文件夹的形式，划分各服务领域模块，主要是两层文件结构：
	一. 首层结构根据业务关系划分：
		1. Basic 基础服务业务模块，也是全局业务模块，如相关的用户，权限等
		2. Core 核心服务业务模块，比如商品，订单
		3. Plugs 独立插件模块，和抽象后也具体业务无关的通用服务，如日志，文件等
	二. 首层下的第二层，主要为领域服务
		如 权限（permit），文件（File）等，领域之间相互独立。


**FrontEnds** -- 前台交互层

	Admin -- 后台管理相关项目
		OSS.Core.AdminSite -- 后台管理相关接口
		ClientApp -- 使用ant design pro搭建的后台页面，发布时挂在接口站点的ClientApp下即可。

**Tests** -- 单元测试层

**Docs** -- 辅助文档，初始化脚本


当前API项目中：

短信功实现使用的阿里云服务，密钥配置文件：/configs/plugs_notify_sms_ali.config
邮件发送，使用的默认邮件发送，配置文件:/configs/plugs_notify_email.config

在后台站点配置中：
后台的权限列表配置文件在：/configs/sys_func_list.config

日志默认写在 /logs 目录下，也可以同时配置邮件接收，接收人相关信息配置文件：/configs/plugs_log_receivers.config