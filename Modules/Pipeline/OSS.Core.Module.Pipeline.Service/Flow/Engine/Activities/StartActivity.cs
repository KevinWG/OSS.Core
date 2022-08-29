namespace OSS.Core.Module.Pipeline;

/// <summary>
///  审核申请提交事件
/// </summary>
internal class StartActivity // : BaseActivity<long, IResp>
{
    ///// <inheritdoc />
    //public StartActivity() : base(nameof(StartActivity))
    //{
    //}

    ///// <inheritdoc />
    //protected override async Task<TrafficSignal<IResp>> Executing(long flowId)
    //{
    //    IResp submitRes;
    //    try
    //    {
    //        submitRes = await SubmitApply(flowId);
    //    }
    //    catch (Exception e)
    //    {
    //        submitRes = new Resp<long>().WithResp(SysRespCodes.AppError, "审核申请异常!");
    //        LogHelper.Error($"审核申请异常 ({e.Message})，详情：{e.StackTrace}", PipeCode);
    //    }

    //    return new TrafficSignal<IResp>(submitRes.IsSuccess() ? SignalFlag.Green_Pass : SignalFlag.Red_Block, submitRes,
    //        submitRes.msg);
    //}

    ///// <summary>
    /////  提交审核申请
    ///// </summary>
    ///// <param name="flowId"></param>
    ///// <returns></returns>
    //private static Task<IResp> SubmitApply(long flowId)
    //{
    //    return AuditFlowRep.Instance.Submit(flowId);
    //}
}