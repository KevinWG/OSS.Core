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
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal;

/// <summary>
/// 用户模块
/// </summary>
public class SocialUserController : BasePortalController
{
    private static readonly SocialUserService _service = new();

    /// <summary>
    /// 获取当前租户平台下的外部平台用户列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_user_list)]
    public Task<PageListResp<SocialUserSmallMo>> SearchSocialUsers([FromBody] SearchReq req)
    {
        return _service.SearchSocialUsers(req);
    }

}