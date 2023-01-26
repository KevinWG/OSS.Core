using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal;

/// <summary>
///  组织机构 开放 WebApi 
/// </summary>
public class OrganizationController : BasePortalController, IOrganizationOpenService
{
    private static readonly IOrganizationOpenService _service = new OrganizationService();

    /// <summary>
    ///  查询组织机构列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_org)]
    public Task<TokenPageListResp<OrganizationMo>> MSearch([FromBody] SearchReq req)
    {
        return _service.MSearch(req);
    }

    /// <summary>
    ///  组织机构通用搜索列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<PageListResp<OrganizationMo>> ComSearch([FromBody] SearchReq req)
    {
        return _service.ComSearch(req);
    }

    /// <summary>
    ///  通过id获取组织机构详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<OrganizationMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///   通过id获取 有效可用的 组织机构 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<OrganizationMo>> GetUseable(long id)
    {
        return _service.GetUseable(id);
    }

    /// <summary>
    ///  设置组织机构可用状态
    /// </summary>
    /// <param name="pass_token"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_org_manage)]
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        return _service.SetUseable(pass_token, flag);
    }



    /// <summary>
    ///  添加组织机构对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_org_manage)]
    public Task<LongResp> Add([FromBody] AddOrganizationReq req)
    {
        return _service.Add(req);
    }
}
