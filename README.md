## OSS.Core
一个完整的.Net Core 领域模块化框架项目。
系统层级上分为：
1. 前段交互层（在 FrontEnds 文件夹），提供用户交互界面
2. 模块服务层（在 Modules 文件夹），提供业务逻辑接口

#### 实现模块

1. Portal 用户门户
	用户登录注册，支持以下方式自由组合：
		用户类型：管理员，用户
		账号类型：手机号，邮箱，第三方（待完善）
		登录方式：密码，动态码，扫码（进行中），Oauth（待完善），小程序（待完善）
	同时还包括:用户/管理员管理，权限码管理，角色管理，以及登录动态码相关模板配置。

2. Notify 通知服务
	主要包含通知模板管理，通知渠道（已接通华为云短信服务，阿里云短信服务进行中，邮箱使用stmp协议，同时为方便调试，提供系统测试通道，发送后会返回具体发送内容到交互端）
	通知发送记录（待完善）

#### 安装

1. 数据库脚本见 **Docs/oss.core.sql**
2. 服务接口层 
	正常微服务模块相互之间可以独立部署，为了方便学习，提供了单点入口项目 OSS.Core.Module.All.WebApi ，直接运行即可（如果需要独立模块处理的，修改对应其他模块的client通过Http访问即可。）
	在仓储层因为使用了读写分离，配置 appsettings.json 的连接串时配置如下节点：
	 "ConnectionStrings": {
		"WriteConnection": 写连接串,
		"ReadConnection": 读连接串
	  },
3. 交互层-管理端 （在 ** FrontEnds\AdminSite ** 目录）
	本地调试请配置 /config/proxy.ts 对应的代理接口地址。当前项目使用AntDesignPro框架，不熟悉如何调试运行的需要先行学习。

默认登录账号：admin@osscore.com    111111


#### 系统解决方案框架：
 这里主要是服务层解决方案框架，在Framework文件夹。通过目录结构展示如下：

--Context
----OSS.Core.Context 	全局上下文（App，Tenant，User），通过全局 CoreContext 静态类访问。
----OSS.Core.Context.Attributes   	上下文请求拦截中间件扩展处理


--Extension 
----OSS.Core.Extension.Cache  针对全局IResp接口的缓存方法扩展（在OSS.Tools.Cache全局中间件的基础上完成）
----OSS.Core.Extension.PassToken  全局行级数据安全通行码扩展方法
----Mvc
------OSS.Core.Extension.Mvc.Configuration   Config配置全局扩展
------Captcha
--------OSS.Core.Extension.Mvc.Captcha   验证码请求拦截中间件扩展（人机校验）
--------OSS.Core.Extension.Mvc.Captcha.Ali   阿里云验证码请求拦截中间件扩展实现


--Core
----OSS.Core.Domain  核心框架 - 领域实体（根）基础类库
----OSS.Core.Service 核心框架 - 领域服务逻辑层基础类库
----OSS.Core.WebApi  核心框架 - 领域协议层（WebApi）基础类库
----Repository 仓储
------OSS.Core.Rep.Dapper 仓储层基础封装（基于开源Dapper类库）
------OSS.Core.Rep.Dapper.Mysql   基于Mysql的仓储层进一步封装
----Opened
------OSS.Core.Domain.Opened 核心框架 - 领域实体（根）基础类库的全局公共部分类库
--Component
----OSS.Core.Comp.DirConfig.Mysql 基于Mysql的字典配置管理组件。

#### 独立开源组件
	除了以上核心的解决方案框架，本系统在底层已经使用，或将来会使用：
	1. [OSS.Tools](https://gitee.com/KevinW/OSS.Tools)，通用工具中间件，分别包含：缓存，配置，日志，定时器，网络请求 中间件
	2. [OSS.DataFlow](https://gitee.com/KevinW/oss.dataflow)， 异步消息中间件
	3. [OSS.PipeLine](https://gitee.com/KevinW/OSS.PipeLine)， 流程引擎框架
	3. [OSS.Clients.Pay](https://gitee.com/KevinW/OSS.Clients.Pay), 支付相关客户端SDK
	4. [OSS.Clients.SNS](https://gitee.com/KevinW/OSS.Clients.SNS), 社交相关客户端SDK

