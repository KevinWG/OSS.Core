using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Infrastructure.Web.Extensions
{
    public static class ModelStateExtension
    {
        /// <summary>
        ///  获取参数验证错误响应实体
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static Resp ToValidErrorResp(this ModelStateDictionary modelState)
        {
            return new Resp(RespTypes.ParaError, GetValidHtmlMsg(modelState));
        }


        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        public static string GetValidHtmlMsg(this ModelStateDictionary modelState)
        {
            var dirs         = GetValidMsgDirs(modelState);
            var volidMessage = dirs.Select(d => d.Value).ToList();
            return string.Join("<br />", volidMessage);
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        public static List<string> GetValidMsgs(this ModelStateDictionary modelState)
        {
            var dirs = GetValidMsgDirs(modelState);
            return dirs.Select(d => string.Concat(d.Key, "-", d.Value)).ToList();
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetValidMsgDirs(this ModelStateDictionary modelState)
        {
            var errordMsgs = new Dictionary<string, string>();
            foreach (var name in modelState.Keys)
            {
                var m = modelState[name];
                if (m.Errors.Count > 0)
                    errordMsgs.Add(name, m.Errors.First().ErrorMessage);
            }

            return errordMsgs;
        }
    }
}
