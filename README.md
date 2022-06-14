# Build an Independent Audit Logging Service with CQRS Pattern

Logs are playing an important role for any modern commercial applications. With logs,
- 市场营销部门可以分析了解客户行为模式并预测市场风向
- 客户服务部门可以针对投诉查询相关历史信息
- 技术人员可以更快速定位甚至提前了解系统缺陷

Furtunately, 几乎每种开发语言都有自己的日志库；而且，几乎所有云服务提供商都提供有基于云端的日志系统和在线分析系统。使用这些已有的开发库和云服务，开发者可以轻松输出日志。

However, 但现实总是很复杂的。有的团队使用云端日志系统，却因为配置不当导致巨额开销或者系统性能下降；有的团队使用本地日志文件，却不得不登录到每个服务器上查找；有的使用本地数据库，却因为数据量太大导致业务数据库快速增长，或者后台数据查询影响了核心业务的响应速度。

Therefore, 在决定采用何种方案之前，最重要的事情是了解自身系统的需求。否则，要么花钱买了不需要的服务，要么自己造轮子做了一堆花点小钱就能解决的事情。

I'm not going to talk about any actual requirements and the relevant solutions. Actually, I just want to focus techniques. In other words, this is just trying some new architecture patterns which probably could be used in other systems in future:
-	CQRS pattern
-	Event Sourcing pattern


### Expected Features


### Scope
- Storage
-	Collection

### Not In Scope
-	Analysis


