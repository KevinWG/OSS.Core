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
            if (identity!=null)
            {
                var sysInfo = MemberShiper.AppAuthorize;
                if (string.IsNullOrEmpty(sysInfo.Token))
                {
                    context.Result = new JsonResult(new ResultMo(ResultTypes.UnAuthorize, "用户未登录！"));
                    return;
                }
                var secreateKeyRes = ApiSourceKeyUtil.GetAppSecretKey(sysInfo.AppSource);
                if (!secreateKeyRes.IsSuccess)
                {
                    context.Result = new JsonResult(secreateKeyRes);
                    return;
                }

                var tokenStr = MemberShiper.GetTokenDetail(secreateKeyRes.Data, sysInfo.Token);
                var tokenSplit = tokenStr.Split('|');

                identity = new MemberIdentity
                {
                    AuthenticationType = tokenSplit[1].ToInt32(),
                    Id = tokenSplit[0].ToInt64()
                };
            }


            if ((MemberAuthorizeType) identity.AuthenticationType == MemberAuthorizeType.Admin)
            {
                // todo 获取后台账号信息
            }
            else
            {
                identity.MemberInfo = service.GetUserInfo(identity.Id);
            }
            MemberShiper.SetIdentity(identity);
        }
    }


}
