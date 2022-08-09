#region Copyright (C) 2020 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore 服务层 —— 接口层（前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2020-6-1 (儿童节快乐!)
*       
*****************************************************************************/

#endregion

using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal;

/// <summary>
///  管理员对外WebApi
/// </summary>
public class AdminController : BasePortalController
{
    private static readonly AdminService _service = new();

    /// <summary>
    ///  创建管理员
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_admin_create)]
    public Task<Resp<long>> Create([FromBody] AddAdminReq req)
    {
        return _service.AddAdmin(req.MapToAdminInfo());
    }

    /// <summary>
    /// 管理员列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_admin_list)]
    public Task<PageListResp<AdminInfoMo>> SearchAdmins([FromBody] SearchReq req)
    {
        return _service.SearchAdmins(req);
    }

    #region 修改自己的信息

    /// <summary>
    ///  管理员修改自己的头像地址
    /// </summary>
    /// <param name="avatar"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> ChangeMyAvatar([FromQuery] string avatar)
    {
        return _service.ChangeMyAvatar(avatar);
    }

    /// <summary>
    ///   管理员修改自己的名称
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> ChangeMyName([FromQuery] string name)
    {
        return  _service.ChangeMyName(name);
    }

    #endregion

    #region 修改其他管理员信息

    /// <summary>
    ///  锁定用户
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_admin_lock)]
    public Task<IResp> Lock(long uid)
    {
        return _service.ChangeLockStatus(uid, true);
    }

    /// <summary>
    ///  解锁用户
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_admin_unlock)]
    public Task<IResp> UnLock(long uid)
    {
        return  _service.ChangeLockStatus(uid, false);
    }

    /// <summary>
    ///  设置管理员类型
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="admin_type"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_admin_settype)]
    public Task<IResp> SetAdminType(long uid, AdminType admin_type)
    {
        if (uid <= 0 || !Enum.IsDefined(typeof(AdminType), admin_type))
        {
            return Task.FromResult((IResp)new Resp(RespCodes.ParaError,"参数异常"));
        }

        return _service.SetAdminType(uid, admin_type);
    }

    #endregion

}