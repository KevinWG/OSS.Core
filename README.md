## OSS.Core
一个完整的.Net Core 领域框架项目。
还在初级实现中，将会串通OSS.Social 和 OSS.PayCenter SDK，敬请期待！


### 架构介绍

整体项目划分三层
交互层==用户交互端
服务层==服务接口，核心业务逻辑实现及接口
平台层==公用基础支撑，提供系统延展提升

#### 交互层

 在**FrontEnds**文件夹中
	Admin 管理端交互界面
		ClientApp -- 使用ant design pro搭建的后台页面，发布时挂在接口站点的ClientApp下即可。


#### 接口服务层

这一层和核心的业务逻辑层，以及对外的接口提供。
水平业务方向：
    以领域模型为基础，做水平层面的切割，单体解决方案下通过文件夹的形式，划分各服务领域模块，主要是两层文件结构：
	一. 首层结构根据业务关系划分：
		1. Basic 基础服务业务模块，也是全局业务模块，如相关的用户，权限等
		2. Core 核心服务业务模块，比如商品，订单
		3. Plugs 独立插件模块，和抽象后也具体业务无关的通用服务，如日志，文件等
	二. 首层下的第二层，主要为领域服务
		如 权限（permit），文件（File）等，领域之间相互独立。

垂直系统方向结构和职能如下：
	OSS.Core.RepDapper -- 项目的数据仓储层（当前使用的是 Dapper+Mysql 形式）。
		为上层数据读写等提供支撑，可以替换 其他ORM+数据库 实现方式		
	OSS.Core.Services -- 核心业务逻辑层，主要实现领域业务逻辑。
	OSS.Core.WebApi -- 提供对外接口实现，参数验证，应用授权验证等

	OSS.Core.Context -- 系统上下文管理，主要包含：
		1. CoreAppContext  
		   应用请求上下文管理，包含当前请求的 AppId，以及对应的相关租户Id，签名密钥等
		2. CoreTenantContext 
			租户上下文信息，租户的基本信息
		3. CoreUserContext 
			主要是当前请求的授权用户上下文信息

	OSS.Core.Infrastructure
		项目通用基础类库，主要包含基础实体，辅助类。

其他文件夹：
**Tests** -- 单元测试层
**Docs** -- 辅助文档，初始化脚本

权限列表配置文件在：/configs/sys_func_list.config

#### 平台层

来自于通用功能的下沉抽象，如：<https://github.com/KevinWG/OSS.Tools>

#### 部署

1. 数据库脚本在**Docment**文件夹下
	InitialDataBase.txt -- 初始化数据库
	TestDataSql.txt  --  初始化默认账号信息
2. 接口层
	因为使用了读写分离，需配置 appsettings.json 文件夹下：
	 "ConnectionStrings": {
		"WriteConnection": 写连接串,
		"ReadConnection": 读连接串
	  },
3. 交互层-管理端
	本地非模拟调试请配置/Admin/ClientApp/config/proxy.ts对应的代理接口地址。
	默认登录账号：1@osscore.cn    111111