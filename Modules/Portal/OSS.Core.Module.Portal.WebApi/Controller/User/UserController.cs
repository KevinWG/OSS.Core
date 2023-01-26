#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 成员相关接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal;

/// <summary>
/// 用户模块
/// </summary>
public class UserController : BasePortalController,IUserOpenService
{
    private static readonly UserService _service = new();
    
    /// <summary>
    ///  修改会员自己基础信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Resp> ChangeMyBasic([FromBody] UpdateUserBasicReq req)
    {
        return _service.ChangeMyBasic(req);
    }

    /// <summary>
    /// 获取当前租户平台下的用户列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_user_list)]
    public async Task<PageListResp<UserBasicMo>> SearchUsers([FromBody] SearchReq req)
    {
        return await _service.SearchUsers(req);
    }

    /// <summary>
    ///  锁定用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_user_lock)]
    public async Task<Resp> Lock(long id)
    {
        return await _service.Lock(id);
    }

    /// <summary>
    ///  解锁用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_user_unlock)]
    public  Task<Resp> UnLock(long id)
    {
        return _service.UnLock(id);
    }


    /// <summary>
    ///  获取用户信息(包含手机号和邮箱信息，管理后台才能使用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [UserFuncMeta(PortalAuthorizeType.Admin)]
    public Task<Resp<UserBasicMo>> Get(long id)
    {
        return _service.Get(id);
    }
}