using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线节点领域对象开放接口
/// </summary>
public interface IPipeOpenService
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
    ///  删除节点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp> Delete(long id);


}
