

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OSS.Core.WebSite.AppCodes.Filters;

namespace OSS.Core.WebSite.Controllers
{
    [AuthorizeUser]
    public class BaseController : Controller
    {
        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected string GetVolidMessage()
        {
            var dirs = GetVolidMsgDirs();
            var volidMessage = dirs.Select(d => d.Value).ToList();
            return string.Join("<br />", volidMessage);
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected List<string> GetVolidMsgs()
        {
            var dirs = GetVolidMsgDirs();
            return dirs.Select(d => string.Concat(d.Key, "-", d.Value)).ToList();
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> GetVolidMsgDirs()
        {
            var errordMsgs = new Dictionary<string, string>();

            foreach (var name in ModelState.Keys)
            {
                var modelState = ModelState[name];
                if (modelState.Errors.Count > 0)
                    errordMsgs.Add(name, modelState.Errors.First().ErrorMessage);
            }
            return errordMsgs;
        }
    }
}