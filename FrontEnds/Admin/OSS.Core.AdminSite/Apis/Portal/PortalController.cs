using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.BasicMos.Enums;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Extensions;
using OSS.CorePro.AdminSite.AppCodes;
using OSS.CorePro.TAdminSite.Apis.Portal.Helpers;
using OSS.CorePro.TAdminSite.Apis.Portal.Reqs;

namespace OSS.CorePro.TAdminSite.Apis.Portal
{
    /// <summary>
    ///   �û���¼ģ��
    /// </summary>
    [AllowAnonymous]
    public class PortalController : ProxyApiController
    {
        /// <summary>
        /// �������û�ʱУ���ֻ��������Ƿ���������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<IActionResult> CheckIfCanReg()
        {
            return PostReqApi("/b/portal/CheckIfCanReg");
        }


        #region �û���¼

        /// <summary>
        ///  ������֤��
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Resp> SendCode([FromBody] LoginNameReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new UserRegLoginResp().WithResp(stateRes); // stateRes.ConvertToResult<UserRegLoginResp>();

            return await RestApiHelper.PostApi<Resp>("/b/portal/sendcode", req);
        }

        [HttpPost]
        public Task<UserRegLoginResp> AdminCodeLogin([FromBody]CodeLoginReq req)
        {
            return AdminLogin(req, "/b/portal/codeadminlogin");
        }

        [HttpPost]
        public Task<UserRegLoginResp> AdminPasswordLogin([FromBody] PwdLoginReq req)
        {
            return AdminLogin(req, "/b/portal/PwdAdminLogin");
        }

        private async Task<UserRegLoginResp> AdminLogin(LoginNameReq req,string apiUrl)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new UserRegLoginResp().WithResp(stateRes);

            var loginRes = await RestApiHelper.PostApi<UserRegLoginResp>(apiUrl, req);
            if (!loginRes.IsSuccess()) return loginRes;

            Response.Cookies.Append(CoreConstKeys.UserCookieName, loginRes.token,
                new CookieOptions() { HttpOnly = true, Expires = DateTimeOffset.Now.AddDays(30) });

            loginRes.token = string.Empty;// д��cookie�������Ĵ��ݵ�js
            return loginRes;
        }

        /// <summary>
        ///   ������¼ʱ����֤ʵ�����
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private Resp CheckLoginModelState(LoginNameReq req)
        {
            if (!ModelState.IsValid)
                return new Resp(RespTypes.ParaError, ModelState.GetValidHtmlMsg());

            if (!Enum.IsDefined(typeof(RegLoginType), req.type))
                return new Resp(RespTypes.ParaError, "δ֪���˺����ͣ�");

            var validator = new DataTypeAttribute(req.type == RegLoginType.Mobile
                    ? DataType.PhoneNumber : DataType.EmailAddress);

            return !validator.IsValid(req.name)
                ? new Resp(RespTypes.ParaError, "��������ȷ���ֻ������䣡")
                : new Resp();
        }

        #endregion

        /// <summary>
        ///  �˳���¼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Resp> Quit()
        {

            Response.Cookies.Delete(CoreConstKeys.UserCookieName);

            var userRes =await AdminHelper.GetAuthAdmin();
            if (!userRes.IsSuccess())
               return new Resp();

            return  await AdminHelper.LogOut(userRes.data);
       
        }

        /// <summary>
        ///  ��ȡ��ǰ�û�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<Resp<UserIdentity>> GetAuthAdmin()
        {
            return AdminHelper.GetAuthAdmin();
        }
    }
}