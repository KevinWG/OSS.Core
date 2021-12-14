using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.Core.CoreApi.Controllers
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected Resp GetInvalidResp()
        {
            return new Resp(RespTypes.ParaError, GetInvalidMsg());
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected Resp<T> GetInvalidResp<T>()
        {
            return new Resp<T>().WithResp(GetInvalidResp());
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected string GetInvalidMsg()
        {
            if (!ModelState.Keys.Any()) 
                return "请求参数错误！";

            var strMsgBuilder = new StringBuilder();
            foreach (var name in ModelState.Keys)
            {
                var modelState = ModelState[name];
                if (modelState.Errors.Count > 0)
                {
                    strMsgBuilder.Append("[").Append(name).Append("]")
                        .Append(modelState.Errors.First().ErrorMessage).Append(",");
                }
            }
            var erMsg= strMsgBuilder.ToString().TrimEnd(',');
            return string.IsNullOrEmpty(erMsg) ? "请求参数错误！" : erMsg;
        }
    }

}
