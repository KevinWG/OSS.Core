using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using System.Text;

namespace OSS.Core.Context.Attributes
{
    public class ModelValidationAttribute:  Attribute,IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var checkRes = CheckIsValid(context);

            if (!checkRes.IsSuccess())
            {
                context.Result = checkRes.ToJsonResult();
                return;
            }

            await next();
        }

        public static IResp CheckIsValid(ActionExecutingContext actionContext)
        {
            var modelState = actionContext.ModelState;
            if (modelState.IsValid) 
                return Resp.DefaultSuccess;
            
            var errStr = new StringBuilder();

            foreach (var key in modelState.Keys)
            {
                var state = modelState[key];
                if (!state.Errors.Any()) 
                    continue;

                errStr.Append(key).Append(':');
                errStr.Append(string.Join("|", state.Errors.Select(e => e.ErrorMessage))).Append(',');
            }

            return new Resp(RespCodes.ParaError, errStr.ToString().TrimEnd(','));
        }
    }
}
