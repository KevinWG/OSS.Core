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
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal;

/// <summary>
/// 用户模块
/// </summary>
public class AuthBindingController : BasePortalController,IAuthBindingOpenService
{
    private static readonly IAuthBindingOpenService  _service = new AuthBindingService();

    /// <summary>
    /// 添加用户
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_user_add)]
    public Task<Resp<long>> AddUser([FromBody] AddUserReq req)
    {
        return _service.AddUser(req);
    }

    /// <summary>
    ///  获取用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<UserBasicMo>> GetCurrentUser()
    {
        return _service.GetCurrentUser();
    }
    
    #region 绑定新账号处理

    /// <summary>
    ///  发送旧账号动态码
    /// </summary>
    /// <param name="type">账号类型</param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> SendOldCode(PortalNameType type)
    {
        return _service.SendOldCode(type);
    }


    /// <summary>
    ///  获取绑定信息的令牌
    /// </summary>
    /// <param name="old_code"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetBindTokenResp> GetBindToken(string old_code, PortalNameType type)
    {
        return _service.GetBindToken(old_code, type);
    }



    /// <summary>
    ///  发送新账号动态码
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> SendNewCode([FromBody] PortalNameReq req)
    {
        return _service.SendNewCode(req);
    }

    /// <summary>
    ///  绑定信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> Bind([FromBody] BindByPassCodeReq req)
    {
        var checkRes = req.CheckNameType();

        if (!checkRes.IsSuccess())
            return Task.FromResult(checkRes);

        return _service.Bind(req);
    }

    #endregion
}