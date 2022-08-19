using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal;

/// <summary>
///  权限接口
/// </summary>
public class FuncController : BasePortalController, IFuncOpenService
{
    private static readonly IFuncOpenService _service = new FuncService();

    #region 权限码

    /// <summary>
    /// 判断权限码是否可用
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResp> CheckFuncCode(string code)
    {
        return _service.CheckFuncCode(code);
    }

    /// <summary>
    ///  添加权限项
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_func_operate)]
    public Task<IResp> AddFuncItem([FromBody] AddFuncItemReq req)
    {
        return _service.AddFuncItem(req);
    }

    /// <summary>
    /// 修改权限码
    /// </summary>
    /// <param name="code">权限编码</param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_func_operate)]
    public Task<IResp> ChangeFuncItem([Required]string code, [FromBody]ChangeFuncItemReq req)
    {
        return _service.ChangeFuncItem(code, req);
    }

  
    /// <summary>
    ///  获取系统所有权限项
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<ListResp<FuncMo>> GetAllFuncItems()
    {
        return _service.GetAllFuncItems();
    }

    #endregion
}