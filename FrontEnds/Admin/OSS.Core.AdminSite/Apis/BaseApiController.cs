using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Attributes;

namespace OSS.CorePro.TAdminSite.Apis
{  
    /// <summary>
    /// 接口控制器基类
    /// </summary>
    [WebApiAjax]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected Resp GetParaErrorResp()
        {
            return new Resp(RespTypes.ParaError, GetInvalidMsg());
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected string GetInvalidMsg()
        {
            var dirs = GetInvalidMsgDirs();
            var volidMessage = dirs.Select(d => d.Value).ToList();
            return string.Join(",", volidMessage);
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected List<string> GetInvalidMsgList()
        {
            var dirs = GetInvalidMsgDirs();
            return dirs.Select(d => string.Concat(d.Key, "-", d.Value)).ToList();
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> GetInvalidMsgDirs()
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

    #region 系统配置信息

    ///// <summary>
    ///// 系统基础信息
    ///// </summary>
    //[Route("api/[controller]/[action]")]
    //public class SysController : ControllerBase
    //{
    //    /// <summary>
    //    ///   获取系统版本
    //    /// </summary>
    //    /// <returns></returns>
    //    [HttpGet]
    //    public string Opv()
    //    {
    //        return AppInfoHelper.AppVersion;
    //    }
    //}

    #endregion
}
