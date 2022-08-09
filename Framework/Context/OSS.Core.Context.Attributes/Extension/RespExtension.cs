using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    internal static class RespExtension
    {
        internal static string ToJson(this IResp res)
        {
            // ret  为了兼容老版本
            return $"{{\"sys_code\":{res.sys_code},\"code\":{res.code},\"ret\":{res.code},\"msg\":\"{res.msg}\"}}";
        }


        internal static IActionResult ToJsonResult(this IResp res)
        {
            return new ContentResult()
            {
                ContentType = "application/json; charset=utf-8",
                Content = res.ToJson()
            };

        }


    }
}
