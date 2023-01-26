using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

/// <summary>
///  Organization 领域对象开放接口
/// </summary>
public interface IOrganizationOpenService
{
    /// <summary>
    ///  组织机构管理搜索列表（附带通过Id生成的PassToken）
    /// </summary>
    /// <returns></returns>
    Task<TokenPageListResp<OrganizationMo>> MSearch(SearchReq req);

    /// <summary>
    ///  组织机构通用搜索列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<OrganizationMo>> ComSearch(SearchReq req);


    /// <summary>
    ///  通过id获取组织机构详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<OrganizationMo>> Get(long id);

    /// <summary>
    ///  通过id获取有效可用状态的组织机构详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<OrganizationMo>> GetUseable(long id);

    /// <summary>
    ///  设置组织机构可用状态
    /// </summary>
    /// <param name="pass_token">通过Id生成的通行码</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<Resp> SetUseable(string pass_token, ushort flag);

    /// <summary>
    ///  添加组织机构对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddOrganizationReq req);
}
