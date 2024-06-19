# OSS.Core

基于.NetCore的积木化服务框架，主要将常规解决方案进行进一步的抽象下沉形成相关基础可选框架单元（在Framework 目录），并在此基础上实现常规系统模块（在Modules  目录），如用户管理，权限管理等。<br>
当前项目，目的是为了提供一个思路，而不是模板。除了 Framework 下提供的基础框架，OSSCore还有其他多个独立的中间件类库，见文档下方。

### 已实现常规模块

1. Portal 用户门户<br>
用户登录注册，支持以下方式自由组合：<br>
>用户类型：管理员，用户<br>
>账号类型：手机号，邮箱，第三方（待完善）<br>
>登录方式：密码，动态码，扫码（进行中），Oauth（待完善），小程序（待完善）

同时还包括:用户/管理员管理，权限码管理，角色管理，以及登录动态码相关模板配置。<br>

2. Notify 通知服务<br>
	主要包含通知模板管理，通知渠道（已接通华为云短信服务，阿里云短信服务进行中，邮箱使用stmp协议）<br>
	同时为方便调试，提供系统测试通道，发送后会返回具体发送内容到交互端
	

### 安装调试

1. 数据库脚本见 **Docs/oss.core.sql**，默认登录账号：admin@osscore.com    111111 
2. 服务接口层 <br>
	正常微服务模块相互之间可以独立部署，为了方便学习，提供了单点入口项目 OSS.Core.Module.All.WebApi ，直接运行即可（如果需要独立模块处理的，修改对应其他模块的client通过Http访问即可。）<br>
		在仓储层因为使用了读写分离，配置 appsettings.json 的连接串时配置如下节点（或者配合仓储层自行定义）：<br>
```
"ConnectionStrings": {
	"WriteConnection": 写连接串,
	"ReadConnection": 读连接串
}
```
3. 交互层-管理端 （在 ** FrontEnds\AdminSite ** 目录）<br>
	本地调试请配置 /config/proxy.ts 对应的代理接口地址。当前项目使用AntDesignPro框架，不熟悉如何调试运行的需要先行学习。
		
### 系统解决方案框架：
这里主要是介绍服务层解决方案框架，在Framework文件夹。通过目录结构展示如下：

>Context 上下文
>>OSS.Core.Context 	    全局上下文（App，Tenant，User），通过全局 CoreContext 静态类访问。<br>
>>OSS.Core.Context.Attributes   	  上下文请求拦截中间件扩展处理

>Extension 
>>OSS.Core.Extension.Cache      针对全局IResp接口的缓存方法扩展<br>
>>OSS.Core.Extension.PassToken    全局行级数据安全通行码扩展方法<br>

>>Captcha （验证码）
>>>>OSS.Core.Extension.Mvc.Captcha   验证码请求拦截中间件扩展（人机校验）<br>
>>>>OSS.Core.Extension.Mvc.Captcha.Ali   阿里云验证码请求拦截中间件扩展实现

>Core  核心模块
>>OSS.Core.Domain  核心框架 - 领域实体（根）基础类库<br>
>>OSS.Core.Service 核心框架 - 领域服务逻辑层基础类库<br>
>>OSS.Core.WebApi  核心框架 - 领域协议层（WebApi）基础类库

>>Repository 仓储
>>>OSS.Core.Rep.Dapper 仓储层基础封装（基于开源Dapper类库）<br>
>>>OSS.Core.Rep.Dapper.Mysql   基于Mysql的仓储层进一步封装

>>Opened
>>>OSS.Core.Domain.Opened 核心框架 - 领域实体（根）基础类库的全局公共部分类库

>Component
>>OSS.Core.Comp.DirConfig.Mysql 基于Mysql的字典配置管理组件。

### 新增模块
	为了减少创建新项目的复杂度，同步提供了 dotnet tool 本地工具（osscore），一键创建项目并建立相关引用
	工具安装命令: dotnet tool intall -g OSSCore

	完成本地工具安装之后，可在项目所在文件夹执行以下命令：
	
	osscore new moduleA （创建名称为 moduleA 的模块解决方案）
    	可选参数提示：
        --pre=xxx, 指定解决方案前缀
        --display=xxx, 指定模块名称
        --dbtype=SqlServer|MySql, 指定数据库类型，默认MySql

        --mode=normal|simple|full, 指定解决方案结构模型
              full： 接口层  服务层 领域层 仓储层 完全独立
            normal： 接口层  服务层 领域层（包含 仓储、领域）
            simple： 接口层  领域层      （包含 仓储、服务、领域）

	osscore add entityName (创建领域对象名为entityName的各模块文件)
    	可选参数：
        --display=xxx, 指定领域对象名称

### 其他相关独立开源组件

除了以上核心的解决方案框架，本系统在底层已经使用，或将来会使用：
1. [OSS.Tools](https://gitee.com/KevinW/OSS.Tools)，通用工具中间件，分别包含：缓存，配置，日志，定时器，网络请求 中间件
2. [OSS.DataFlow](https://gitee.com/KevinW/oss.dataflow)， 异步消息中间件
3. [OSS.PipeLine](https://gitee.com/KevinW/OSS.PipeLine)， 流程引擎框架
3. [OSS.Clients.Pay](https://gitee.com/KevinW/OSS.Clients.Pay), 支付相关客户端SDK
4. [OSS.Clients.SNS](https://gitee.com/KevinW/OSS.Clients.SNS), 社交相关客户端SDK