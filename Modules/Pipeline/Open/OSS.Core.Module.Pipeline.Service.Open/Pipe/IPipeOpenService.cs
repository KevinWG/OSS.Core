using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线节点领域对象开放接口
/// </summary>
public interface IPipeOpenService
{
    /// <summary>
    ///  添加节点
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp<long>> Add(AddPipeReq req);

    /// <summary>
    ///  删除节点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp> Delete(long id);
}
