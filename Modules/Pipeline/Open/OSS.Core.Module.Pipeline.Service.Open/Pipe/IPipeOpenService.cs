using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 领域对象开放接口
/// </summary>
public interface IPipeOpenService
{
    /// <summary>
    ///  查询Pipe列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<PipeMo>> Search(SearchReq req);

    /// <summary>
    ///  通过id获取Pipe详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<PipeMo>> Get(long id);

    /// <summary>
    ///  设置Pipe可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<IResp> SetUseable(long id, ushort flag);

    /// <summary>
    ///  添加Pipe对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Add(AddPipeReq req);
}
