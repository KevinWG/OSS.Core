using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSS.Core.Service
{
    public class BaseService
    {
        protected Resp ValidateReq<T>(IList<T> listItems)
        {
            if (listItems == null)
            {
                return ParaErrorResp;
            }

            foreach (var item in listItems)
            {
                var res = ValidateReq(item);
                if (!res.IsSuccess())
                {
                    return res;
                }
            }

            return new Resp();
        }

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected Resp ValidateReq(object objectData)
        {
            if (objectData == null)
            {
                return new Resp(RespTypes.OperateObjectNull, "参数错误!");
            }
            var vaContext = new ValidationContext(objectData);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(objectData, vaContext, validationResults);

            if (isValid)
                return new Resp();

            var strMsgBuilder = new StringBuilder();
            foreach (var vres in validationResults)
            {
                if (vres.MemberNames.Count() > 0)
                {
                    strMsgBuilder.Append("[").Append(vres.MemberNames.FirstOrDefault()).Append("]")
                        .Append(vres.ErrorMessage).Append(",");
                }
            }

            var erMsg = strMsgBuilder.ToString().TrimEnd(',');
            return new Resp(RespTypes.ParaError, erMsg);
        }

        protected static readonly Resp ParaErrorResp = new Resp(RespTypes.ParaError, "参数错误!");
    }

}
