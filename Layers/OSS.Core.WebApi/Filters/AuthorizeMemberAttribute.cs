using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.Services.Members;

namespace OSS.Core.WebApi.Filters
{
    public class AuthorizeMemberAttribute : Attribute, IAuthorizationFilter
    {
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
                if (!identityRes.IsSuccess)
                {
                    context.Result = new JsonResult(identityRes);
                    return;
                }
                identity = identityRes.Data;
            }
            if (infoType == MemberInfoType.Info)
            {
                if (!GetIdentityMemberInfo(identity).IsSuccess)
                {
                    context.Result = new JsonResult(new ResultMo(ResultTypes.UnAuthorize, "未发现授权用户信息"));
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
            if (identity.AuthenticationType == (int) MemberAuthorizeType.Admin)
            {
                var memRes = service.GetAdminInfo(identity.Id);
                if (!memRes.IsSuccess)
                    return memRes;
                identity.MemberInfo = memRes.Data;
            }
            else
            {
                var memRes = service.GetUserInfo(identity.Id);
                if (!memRes.IsSuccess)
                    return memRes;
                identity.MemberInfo = memRes.Data;
            }
            return new ResultMo();
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


            var secreateKeyRes = ApiSourceKeyUtil.GetAppSecretKey(sysInfo.AppSource);
            if (!secreateKeyRes.IsSuccess)
                return secreateKeyRes.ConvertToResultOnly<MemberIdentity>();


            var tokenStr = MemberShiper.GetTokenDetail(secreateKeyRes.Data, sysInfo.Token);
            var tokenSplit = tokenStr.Split('|');

            var identity = new MemberIdentity
            {
                AuthenticationType = tokenSplit[1].ToInt32(),
                Id = tokenSplit[0].ToInt64()
            };
            return new ResultMo<MemberIdentity>(identity);
        }



    }

    public static class MemberTokenUtil
    {
        public static (long id,int authType,string name) SplitToken(string tokenDetail)
        {
            var tokenSplit = tokenDetail.Split('|');

            return (tokenSplit[0].ToInt64(), tokenSplit[1].ToInt32(), tokenSplit[2]);
        }
    }
}
