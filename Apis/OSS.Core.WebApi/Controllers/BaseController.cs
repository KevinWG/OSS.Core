using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos.Resp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.Core.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected static Resp ParaErrorResp { get; }= new Resp(RespTypes.ParaError, "请求参数错误!");

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected Resp GetInvalidResp()
        {
            return new Resp(RespTypes.ParaError,GetInvalidMsg());
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected string GetInvalidMsg()
        {
            var strMsgBuilder=new StringBuilder();
            foreach (var name in ModelState.Keys)
            {
                var modelState = ModelState[name];
                if (modelState.Errors.Count > 0)
                {
                    strMsgBuilder.Append("[").Append(name).Append("]")
                        .Append(modelState.Errors.First().ErrorMessage).Append(",");
                } 
            }

            return strMsgBuilder.ToString().TrimEnd(',');
        }

     
     
    }
  
}
