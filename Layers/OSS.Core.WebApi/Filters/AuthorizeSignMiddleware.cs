using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.Infrastructure.Utils;

namespace OSS.Core.WebApi.Filters
{
    public class AuthorizeSignMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeSignMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        private const string authorizeTicket = "t_id";

        public async Task Invoke(HttpContext context)
        {
            string auticketStr = context.Request.Headers[authorizeTicket];
            if (auticketStr == null)
            {
                await ResponseEnd(context, new ResultMo(ResultTypes.UnKnowSource, "未知应用来源"));
                return;
            }

            var sysInfo = new SysAuthorizeInfo();
            sysInfo.FromSignData(auticketStr);

            var secretKeyRes = ApiSourceKeyUtil.GetAppSecretKey(sysInfo.AppSource);
            if (!secretKeyRes.IsSuccess)
            {
                await ResponseEnd(context, secretKeyRes);
                return;
            }

            if (!sysInfo.CheckSign(secretKeyRes.Data))
            {
                await ResponseEnd(context, new ResultMo(ResultTypes.ParaNotMeet, "非法应用签名！"));
                return;
            }

            MemberShiper.SetAppAuthrizeInfo(sysInfo);
            await _next.Invoke(context);
        }


        private static async Task ResponseEnd(HttpContext context,ResultMo res)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync($"{{\"Ret\":{res.Ret},\"Message\":\"{res.Message}\"}}");
        }
    }


    internal static class AuthorizeSignMiddlewareExtention
    {
        internal static IApplicationBuilder UseAuthorizeSignMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthorizeSignMiddleware>();
        }
    }

}
