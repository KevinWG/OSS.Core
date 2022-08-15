namespace OSS.Core.Module.WorkFlow;

public static class WorkFlowConst
{
    public const string ModuleName = "WorkFlow";

    public static class CacheKeys
    {
        // 放置相关缓存Key
        // 涉及相关动态参数，建议以 ByPara 结尾，例如： WorkFlowDetailById = "WorkFlowDetail_"
    }

    public static class DataMsgKeys
    {
        // 放置 发布订阅/异步 消息key
    }

    public static class FuncCodes
    {
        // 放置权限码
    }
}
