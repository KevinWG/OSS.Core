using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线节点领域对象开放接口
/// </summary>
public  interface IPipeOpenService
{
    /// <summary>
    ///  添加开始节点
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> AddStart(AddPipeReq req);

    /// <summary>
    ///  添加结束节点
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> AddEnd(AddPipeReq req);



    /// <summary>
    ///  添加审核节点
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> AddAudit(AddPipeReq req);

    /// <summary>
    ///  设置审核节点执行扩展信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ext"></param>
    /// <returns></returns>
    Task<IResp> SetAuditExe(long id, AuditExt ext);



    /// <summary>
    ///  添加子流水线
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> AddSubPipeline(AddPipeReq req);

    /// <summary>
    ///  设置子流水线执行扩展信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ext"></param>
    /// <returns></returns>
    Task<IResp> SetSubPipelineExe(long id, SubPipeLineExt ext);


    /// <summary>
    ///  删除节点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp> Delete(long id);
}
