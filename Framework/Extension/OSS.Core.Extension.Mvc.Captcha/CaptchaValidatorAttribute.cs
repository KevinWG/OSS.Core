using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Extension.Mvc.Captcha
{
    /// <summary>
    ///    浏览器端行为验证码
    ///     如果 来源应用（SourceMode） 是 AppSourceMode.AppSign 则可以不用验证
    /// </summary>
    public class CaptchaValidatorAttribute : BaseAuthorizeAttribute
    {
        public CaptchaValidatorAttribute()
        {
        }

        /// <summary>
        ///  行为校验码校验
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<IResp> Authorize(AuthorizationFilterContext context)
        {
            var validator   = GetValidator(context.HttpContext);
            var appIdentity = CoreContext.App.Identity;

            if (appIdentity.source_mode == AppSourceMode.AppSign)
                return Resp.DefaultSuccess;

            return await validator.Validate(context.HttpContext);
        }

        private static ICaptchaValidator GetValidator(HttpContext context)
        {
            var provider = CaptchaHelper.ValidatorProvider;

            var validator = provider != null ? provider.GetValidator(context) : CaptchaHelper.DefaultValidator;

            if (validator == null)
            {
                throw new NotImplementedException("未能发现 CaptchaValidator 具体实现!");
            }

            return validator;
        }
    }
}
