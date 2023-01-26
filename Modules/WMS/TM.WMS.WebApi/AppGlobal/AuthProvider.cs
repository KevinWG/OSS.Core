﻿using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Tools.Config;
using OSS.Core.Context.Attributes;

namespace TM.WMS;

/// <summary>
///  应用访问秘钥信息提供者
/// </summary>
public class AppAccessProvider : IAppAccessProvider
{
    private static List<AppAccess>? _appAccessList = null;

    /// <summary>
    ///  根据客户端请求access_key值获取访问秘钥信息(用于模块间的签名验证
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<IResp<AppAccess>> GetByKey(string key)
    {
        if (_appAccessList == null)
        {
            _appAccessList = new List<AppAccess>();
            ConfigHelper.Configuration.GetSection("Access").Bind(_appAccessList);
        }

        var access = _appAccessList.FirstOrDefault(a => a.access_key == key);
        var res = access == null
            ? new Resp<AppAccess>().WithResp(SysRespCodes.NotAllowed, "非法的请求")
            : new Resp<AppAccess>(access);

        return Task.FromResult<IResp<AppAccess>>(res);
    }
}




/// <summary>
///  登录用户授权验证
/// </summary>
public class UserAuthProvider : IUserAuthProvider
{
    /// <inheritdoc />
    public Task<IResp<UserIdentity>> GetIdentity()
    {
        // 可自定义如下返回：
        //var identity = new UserIdentity()
        //{
        //    id = "1",
        //    name = "测试管理员",
        //    auth_type = PortalAuthorizeType.Admin
        //};

        //IResp<UserIdentity> res = new Resp<UserIdentity>(identity);
        //return Task.FromResult(res);


        // 或者部署通用模块 Portal，引用接口客户端SDK，如：
        // return PortalRemoteClient.Auth.GetIdentity();

        throw new NotImplementedException("请实现 UserAuthProvider 验证方法");
    }
}

/// <summary>
///  登录用户功能授权验证
/// </summary>
public class FuncAuthProvider : IFuncAuthProvider
{
    /// <inheritdoc />
    public Task<IResp<FuncDataLevel>> Authorize(string funcCode)
    {
        // 可以自定义实现,如：
        //IResp<FuncDataLevel> res = new Resp<FuncDataLevel>(FuncDataLevel.All);
        //return Task.FromResult(res);

        // 或者部署通用模块 Portal，引用接口客户端SDK，如：
        // return PortalRemoteClient.Grant.CheckPermit(funcCode);

        throw new NotImplementedException("请实现 FuncAuthProvider 验证方法");
    }
}


