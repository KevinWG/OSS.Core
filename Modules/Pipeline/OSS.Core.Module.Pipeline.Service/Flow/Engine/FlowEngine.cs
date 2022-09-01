using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

internal static class FlowEngine
{
    private static readonly CreateActivity _createFlow = new();

    private static readonly CreateDispatcher _createDispatcher = new();

    private static readonly StartActivity       _startFlow = new();
    private static readonly InitialNextActivity _next      = new();

    private static readonly ExecutingActivity  _executing          = new();
    private static readonly ExecutedDispatcher _executedDispatcher = new();

    private static readonly FeedActivity _feed    = new();
    private static readonly EndActivity  _endFlow = new();

    internal const string CreateToNextConvertor = "CreateToNextConvertor";

    static FlowEngine()
    {
        _createFlow.Append(_createDispatcher);

        _createDispatcher.Append(_startFlow).Append(_next);
        _createDispatcher.AppendMsgConverter(c => c.flow, CreateToNextConvertor).Append(_next);

        _next.AppendMsgEnumerator().Append(_executing).Append(_executedDispatcher);

        _executedDispatcher.AppendCondition(CheckToNext, _next);
        _executedDispatcher.AppendCondition(CheckToFeed, _feed);
        _executedDispatcher.AppendCondition(CheckToEnd, _endFlow);

        _feed.Append(_executedDispatcher);
    }

    private static bool CheckToNext(FlowNodeMo node) => node.status == ProcessStatus.Completed && node.pipe_type != PipeType.End;
    private static bool CheckToEnd(FlowNodeMo node) => node.status == ProcessStatus.Abandon || (node.status == ProcessStatus.Completed && node.pipe_type == PipeType.End);
    private static bool CheckToFeed(FlowNodeMo node) => node.status is ProcessStatus.Waiting or ProcessStatus.Processing;

    /// <summary>
    ///  创建业务流程
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static Task<LongResp> Create(CreateReq req)
    {
        return _createFlow.Execute(req);
    }

    /// <summary>
    ///  启动流程
    /// </summary>
    /// <param name="flowId"></param>
    /// <returns></returns>
    public static Task<IResp> Start(long flowId)
    {
        return _startFlow.Execute(flowId);
    }

    /// <summary>
    ///   回调投递结果
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static Task<IResp> Feed(FeedReq req)
    {
        return _feed.Execute(req);
    }
}