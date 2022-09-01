using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 对象仓储
/// </summary>
public class FlowNodeRep : BasePipelineRep<FlowNodeMo, long>
{
    /// <inheritdoc />
    public FlowNodeRep() : base("Flow")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<FlowNodeMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }


    /// <summary>
    /// 修改节点处理人信息
    /// </summary>
    /// <param name="nodeId"></param>
    /// <param name="processorId"></param>
    /// <param name="processorName"></param>
    /// <returns></returns>
    public Task<IResp> UpdateProcessor(long nodeId, long processorId, string processorName)
    {
        return Update(u => new
        {
            processor_id = processorId,
            processor_name = processorName
        }, w => w.id == nodeId);
    }


    /// <summary>
    /// 修改节点 执行开始 状态
    /// </summary>
    /// <param name="nodeId"></param>
    /// <param name="startTime"></param>
    /// <returns></returns>
    public Task<IResp> UpdateStart(long nodeId, long startTime)
    {
        return Update(u => new
        {
            status   = ProcessStatus.Processing,
            start_time = startTime
        }, w => w.id == nodeId);
    }

    /// <summary>
    /// 修改节点  执行结束  状态
    /// </summary>
    /// <param name="nodeId"></param>
    /// <param name="status"></param>
    /// <param name="endTime"></param>
    /// <param name="remark"></param>
    /// <returns></returns>
    public Task<IResp> UpdateEnd(long nodeId, ProcessStatus status, long endTime, string remark)
    {
        return Update(u => new
        {
            status   = status,
            remark   = remark,
            end_time = endTime
        }, w => w.id == nodeId);
    }

    /// <summary>
    ///   通过父级Id结束节点
    /// </summary>
    /// <param name="parentId">父级流Id</param>
    /// <param name="endTime"></param>
    /// <param name="remark"></param>
    /// <returns></returns>
    public Task<IResp> AbandonChildrenByParent(long parentId, long endTime, string remark)
    {
        return Update(u => new
        {
            status   = ProcessStatus.Abandon,
            remark   = remark,
            end_time = endTime
        }, w => w.parent_id == parentId && (w.status == ProcessStatus.Processing || w.status == ProcessStatus.Waiting));
    }

}
