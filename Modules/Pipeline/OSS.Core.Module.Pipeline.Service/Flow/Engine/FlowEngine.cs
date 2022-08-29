using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

internal static class FlowEngine
{
    #region 普通节点


    #endregion

    public static Task<IResp> Start(StartReq req)
    {
        return Task.FromResult(Resp.DefaultSuccess);
    }


    //public static void Start()
    //{
    //    //// 主流程
    //    //ApplyEvent
    //    //    .AppendMsgConverter(resId => resId.data)
    //    //    .Append(SubmitEvent)
    //    //    .AppendMsgConverter(flowId => new InitialNextReq() {flow_id = flowId})
    //    //    .Append(_initialNextEvent)
    //    //    .AppendMsgEnumerator("ForeachNextNode")
    //    //    .Append(_nodeDispatchor); // 下级节点列表循环

    //    //// 下级节点 - 手动审核分支
    //    //_nodeDispatchor
    //    //    .Append(AuditEvent)
    //    //    .AppendMsgConverter(auditRes => auditRes.data)
    //    //    .Append(_nodeResultDispatchor);

    //    //// 下级节点 - 订单退款分支（特殊）
    //    //_nodeDispatchor
    //    //    .Append(Refund)
    //    //    .AppendMsgSubscriber<RefundMo>(DataFlowMsgKeys.Pay_Refund_Call)
    //    //    .Append(RefundCall)
    //    //    .AppendMsgConverter(res => res.data)
    //    //    .Append(_nodeResultDispatchor);




    //    //// 节点结果处理分支 - 初始化后续节点分支
    //    //_nodeResultDispatchor
    //    //    .AppendMsgConverter(aReqPara => new InitialNextReq() {node_id = aReqPara.id}, _initialConvertorPipeCode)
    //    //    .Append(_initialNextEvent);

    //    //// 节点结果处理分支 - 结束节点分支
    //    //_nodeResultDispatchor
    //    //    .Append(_finishEvent)
    //    //    .AppendMsgPublisher(a => string.Concat(DataFlowMsgKeys.Audit_Finish_Call, a.flow_code));
    //}


    //private static bool NodeDispatchFilter(AuditNodeMo branchContext, IPipeMeta branch)
    //{
    //    // 关闭节点
    //    if (branch.PipeCode == nameof(ManualAuditEvent) &&
    //        (branchContext.ins_type == NodeInsType.Manual_Admin ||
    //         branchContext.ins_type == NodeInsType.Manual_User))
    //    {
    //        return true;
    //    }
    //    else if (branch.PipeCode == nameof(AuditRefundEvent) && branchContext.ins_type == NodeInsType.OrderRefund)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private static bool NodeResultDispatchFilter(AuditNodeMo auditedNode, IPipeMeta branch)
    //{
    //    if (branch.PipeCode == nameof(FinishEvent))
    //    {
    //        if (auditedNode.node_type == NodeType.FinalNode || auditedNode.status != AuditNodeStatus.Passed)
    //        {
    //            return true;
    //        }

    //        return false;
    //    }
    //    else if (branch.PipeCode == _initialConvertorPipeCode && auditedNode.status == AuditNodeStatus.Passed)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}