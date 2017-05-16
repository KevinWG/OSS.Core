using System;
using Microsoft.AspNetCore.Mvc;
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
        private static readonly MemberService service=new MemberService();
        private readonly MemberInfoType infoType;
        public AuthorizeMemberAttribute(MemberInfoType mInfoType=MemberInfoType.OnlyId)
        {
            infoType = mInfoType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sysInfo = MemberShiper.AppAuthorize;
            if (string.IsNullOrEmpty(sysInfo.Token))
            {
                context.Result = new JsonResult(new ResultMo(ResultTypes.UnAuthorize,"用户未登录！"));
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

            var identity=new MemberIdentity();

            identity.AuthorizedType = tokenSplit[1].ToInt32();
            identity.Id = tokenSplit[0].ToInt64();
            if (infoType==MemberInfoType.Info)
            {
                //todo 设置MemberInfo
                if ((MemberAuthorizeType)identity.AuthorizedType==MemberAuthorizeType.Admin)
                {
                    // todo 获取后台账号信息
                }
                else
                {
                    identity.MemberInfo = service.GetUserInfo(identity.Id);
                }
            }

            

        }
    }

 
}
