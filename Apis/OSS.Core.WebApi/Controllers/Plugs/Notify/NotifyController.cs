using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Plugs.Notify;
using OSS.Core.Services.Plugs.Notify.Mos;
using OSS.Core.Services.Plugs.Notify.NotifyAdapters.EmailHandlers.Mos;

namespace OSS.Core.CoreApi.Controllers.Plugs.Notify
{
    [ModuleMeta(CoreModuleNames.Notify)]
    [Route("p/[controller]/[action]")]
    public class NotifyController : BaseController
    {
        private static readonly NotifyService _service = new NotifyService();
        
        /// <summary>
        ///   获取邮箱账号配置 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UserMeta(CoreFuncCodes.Notify_Account_Manage)]
        public  Task<Resp<EmailSmtpConfig>> GetEmailConfig()
        {
            return _service.GetEmailConfig();
        }

        /// <summary>
        /// 设置邮箱账号配置信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserMeta(CoreFuncCodes.Notify_Account_Manage)]
        public Task<Resp> SetEmailConfig([FromBody] EmailSmtpConfig config)
        {
            if (config == null)
            {
                return Task.FromResult(GetInvalidResp());
            }
            return _service.SetEmailConfig(config);
        }

        /// <summary>
        ///   获取配置模板code字典 
        /// </summary>
        /// <returns>{code:模板名称}</returns>
        [HttpGet]
        [UserMeta(CoreFuncCodes.Notify_Template_Manage)]
        public Resp<Dictionary<string, string>> GetTemplateDirs()
        {
            return _service.GetTemplateDirs();
        }

        /// <summary>
        ///   设置通知模板 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UserMeta(CoreFuncCodes.Notify_Template_Manage)]
        public Task<Resp<NotifyTemplateConfig>> GetTemplateConfig(string t_code)
        {
            if (string.IsNullOrEmpty(t_code))
            {
                return Task.FromResult(new Resp<NotifyTemplateConfig>().WithResp(GetInvalidResp()));
            }
            return _service.GetTemplateConfig(t_code);
        }

        /// <summary>
        ///  获取通知模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserMeta(CoreFuncCodes.Notify_Template_Manage)]
        public Task<Resp> SetTemplateConfig(string t_code, [FromBody] NotifyTemplateConfig req)
        {
            if (string.IsNullOrEmpty(t_code)|| req==null)
            {
                return Task.FromResult(GetInvalidResp());
            }
            return _service.SetTemplateConfig(t_code, req);
        }

    }
}
