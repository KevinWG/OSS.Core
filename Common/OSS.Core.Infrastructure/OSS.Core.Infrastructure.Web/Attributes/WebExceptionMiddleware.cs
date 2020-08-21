using Microsoft.AspNetCore.Http;

namespace OSS.Core.Infrastructure.Web.Attributes
{
    public class WebExceptionMiddleware: ExceptionMiddleware
    {
        public WebExceptionMiddleware(RequestDelegate next):base(next)
        {
            p_IsWebSite = true;
        }
    }
}
