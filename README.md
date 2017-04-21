# OSS.Core
一个完整的.Net Core 领域框架项目。
还在初级实现中，将会串通OSS.Social 和 OSS.PayCenter SDK，敬请期待！


## 文件结构介绍
**Infrastructure** --  基础类库，包含通用枚举实体等，通用验证签名配置帮助类等
**Layers**  --  核心业务结构层，提供外部访问接口
	OSS.Core.RepDapper -- 项目的数据仓储层（当前是Dapper+Mysql形式），主要实现OSS.Core.DomainMos层访问接口，为上层数据读写等提供支撑，可以替换 其他ORM+数据库 实现方式
	OSS.Core.DomainMos -- 包含项目核心领域模型和数据仓储层接口定义
	OSS.Core.Services -- 核心业务逻辑层，主要实现业务逻辑
	OSS.Core.WebApi -- 提供对外接口实现，完成应用授权验证
**Plugs** -- 系统插件层，实现缓存，日志等具体实现
	OSS.CachePlug.StackRedis -- StackOver实现的Redis操作类库
**FrontEnds** -- 前台交互层
	OSS.Core.Admin -- 后台管理层
	OSS.Core.WebSite -- 用户交互层
**Tests** -- 单元测试层
	OSS.Core.RepTests -- 数据库测试项目