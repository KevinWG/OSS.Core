using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Link 领域对象开放接口
/// </summary>
public interface ILinkOpenService
{
    /// <summary>
    ///  查询Link列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<LinkMo>> Search(SearchReq req);

    /// <summary>
    ///  通过id获取Link详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<LinkMo>> Get(long id);

    /// <summary>
    ///  设置Link可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<IResp> SetUseable(long id, ushort flag);

    /// <summary>
    ///  添加Link对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Add(AddLinkReq req);
}
