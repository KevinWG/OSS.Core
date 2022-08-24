namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipeline 模块静态声明
/// </summary>
public static class PipelineConst
{
    public const string ModuleName = "Pipeline";

    public static class CacheKeys
    {
        // 放置相关缓存Key
        // 涉及相关动态参数，建议以 ByPara 结尾，例如： PipelineDetailById = "PipelineDetail_"
    }

    public static class DataMsgKeys
    {
        // 放置 发布订阅/异步 消息key
    }

    public static class FuncCodes
    {
        // 放置权限码
    }

    /// <summary>
    ///  仓储层表名
    /// </summary>
    public static class RepTables
    {
        /// <summary>
        ///  管道表名
        /// </summary>
        public const string Pipe = "m_pipe";

        /// <summary>
        /// 管道流水线元数据表名
        /// </summary>
        public const string PipelineMeta = "m_pipeline_meta";

        /// <summary>
        /// 管道流水线版本表
        /// </summary>
        public const string PipelinePart = "m_pipeline_part";
        
    }
}
