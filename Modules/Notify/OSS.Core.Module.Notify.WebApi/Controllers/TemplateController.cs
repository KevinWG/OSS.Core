using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Notify;

/// <summary>
///  通知模板对外WebApi
/// </summary>
public class TemplateController : BaseNotifyController, ITemplateOpenService
{
    private static readonly ITemplateOpenService _service = new TemplateService();

    /// <inheritdoc />
    [HttpPost]
    [UserFuncMeta(NotifyConst.FuncCodes.Notify_Template_List)]
    public Task<PageListResp<TemplateMo>> Search([FromBody] SearchReq req)
    {
        return _service.Search(req);
    }

    /// <summary>
    /// 获取模板详情
    /// </summary>
    /// <param name="id">模板id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResp<TemplateMo>> Get(long id)
    {
        return _service.Get(id);
    }
    
 
    /// <summary>
    ///  更新模板信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(NotifyConst.FuncCodes.Notify_Template_Update)]
    public Task<IResp> Update(long id, [FromBody] AddTemplateReq req)
    {
        return _service.Update(id, req);
    }

    /// <summary>
    ///  设置可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(NotifyConst.FuncCodes.Notify_Template_Update)]
    public async Task<IResp> SetUseable(long id, ushort flag)
    {
        return await _service.SetUseable(id, flag);
    }

    /// <inheritdoc />
    [HttpPost]
    [UserFuncMeta(NotifyConst.FuncCodes.Notify_Template_Add)]
    public Task<IResp> Add([FromBody] AddTemplateReq req)
    {
        return _service.Add(req);
    }
}