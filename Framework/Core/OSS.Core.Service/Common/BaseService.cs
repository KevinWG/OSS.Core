using OSS.Common.Resp;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OSS.Core.Service
{
    public class BaseService
    {
       

        /// <summary>
        /// 获取验证失败列表信息
        /// </summary>
        /// <returns></returns>
        protected IResp ValidateReq(object? objectData)
        {
            if (objectData == null)
            {
                return new Resp(RespCodes.OperateObjectNull, "参数错误!");
            }
            var vaContext = new ValidationContext(objectData);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(objectData, vaContext, validationResults);

            if (isValid)
                return new Resp();

            var strMsgBuilder = new StringBuilder();
            foreach (var vres in validationResults)
            {
                if (vres.MemberNames.Any())
                {
                    strMsgBuilder.Append("[").Append(vres.MemberNames.FirstOrDefault()).Append("]")
                        .Append(vres.ErrorMessage).Append(",");
                }
            }

            var erMsg = strMsgBuilder.ToString().TrimEnd(',');
            return new Resp(RespCodes.ParaError, erMsg);
        }

        protected static readonly Resp ParaErrorResp = new Resp(RespCodes.ParaError, "参数错误!");
    }

}
