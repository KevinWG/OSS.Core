#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 成员登录授权验证 Attribute
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Core.Domains;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.Services.Members;

namespace OSS.Core.WebApi.Filters
{
    public class AuthorizeMemberAttribute : Attribute, IAuthorizationFilter
    {
        // ReSharper disable once InconsistentNaming
        private static readonly MemberService service = new MemberService();
        private readonly MemberInfoType infoType;

        public AuthorizeMemberAttribute(MemberInfoType mInfoType = MemberInfoType.OnlyId)
        {
            infoType = mInfoType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(filter => filter is IAllowAnonymousFilter))
                return;
            //  防止cotroller层和action的或者成员信息需求不一致时 的去重验证
            if (infoType == MemberInfoType.OnlyId && MemberShiper.IsAuthenticated
                || infoType == MemberInfoType.Info && MemberShiper.Identity.MemberInfo != null)
                return;

            var identity = MemberShiper.Identity;
            if (identity == null)
            {
                var identityRes = GetIndentityId();
                if (!identityRes.IsSuccess())
                {
                    context.Result = new JsonResult(identityRes);
                    return;
                }
                identity = identityRes.data;
            }
            if (infoType == MemberInfoType.Info)
            {
                var memRes = GetIdentityMemberInfo(identity);
                if (!memRes.IsSuccess())
                {
                    context.Result = new JsonResult(memRes);
                    return;
                }
            }
           
            MemberShiper.SetIdentity(identity);
        }

        /// <summary>
        ///   获取identity id对应的成员信息
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        private static ResultMo GetIdentityMemberInfo(MemberIdentity identity)
        {
            BaseAutoMo memInfo = null;
            if (identity.AuthenticationType == (int) MemberAuthorizeType.Admin)
            {
                var memRes = service.GetAdminInfo(identity.Id).WaitResult();
                if (!memRes.IsSuccess())
                    return new ResultMo(ResultTypes.UnAuthorize, "未发现授权用户信息");
                memInfo = memRes.data;
            }
            else
            {
                var memRes = service.GetUserInfo(identity.Id).WaitResult();
                if (!memRes.IsSuccess())
                    return new ResultMo(ResultTypes.UnAuthorize, "未发现授权用户信息");
                memInfo = memRes.data;
            }
            identity.MemberInfo = memInfo;

            var checkRes = PortalService.CheckMemberStatus(memInfo.status);
            return !checkRes.IsSuccess() ? checkRes : new ResultMo();
        }

        /// <summary>
        ///  通过授权token获取对应的成员Id等信息
        /// </summary>
        /// <returns></returns>
        private static ResultMo<MemberIdentity> GetIndentityId()
        {
            var sysInfo = MemberShiper.AppAuthorize;
            if (string.IsNullOrEmpty(sysInfo.Token))
                return new ResultMo<MemberIdentity>(ResultTypes.UnAuthorize, "用户未登录！");

            var tokenRes = MemberTokenUtil.GetTokenDetail(sysInfo.AppSource, sysInfo.Token);
            if (!tokenRes.IsSuccess())
                return tokenRes.ConvertToResultOnly<MemberIdentity>();

            var identity = new MemberIdentity
            {
                AuthenticationType = tokenRes.data.authType,
                Id = tokenRes.data.id
            };
            return new ResultMo<MemberIdentity>(identity);
        }
    }

    public static class MemberTokenUtil
    {
        public static ResultMo<(long id,int authType)> GetTokenDetail(string appSource,string tokenStr)
        {
            var secreateKeyRes = ApiSourceKeyUtil.GetAppSecretKey(appSource);
            if (!secreateKeyRes.IsSuccess())
                return secreateKeyRes.ConvertToResultOnly<(long id, int authType)>();

            var tokenDetail = MemberShiper.GetTokenDetail(secreateKeyRes.data, tokenStr);

            var tokenSplit = tokenDetail.Split('|');
            return new ResultMo<ValueTuple<long, int>>((tokenSplit[0].ToInt64(), tokenSplit[1].ToInt32()));
        }

        public static ResultMo<string> AppendToken(string appSource,long id, MemberAuthorizeType authType)
        {
            var secreateKeyRes = ApiSourceKeyUtil.GetAppSecretKey(appSource);
            if (!secreateKeyRes.IsSuccess())
                return secreateKeyRes.ConvertToResultOnly<string>();

            var tokenCon=string.Concat(id, "|", (int)authType, "|", DateTime.Now.ToUtcSeconds());
            return new ResultMo<string>(MemberShiper.GetToken(secreateKeyRes.data, tokenCon));
        }
    }
}
