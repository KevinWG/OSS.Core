using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Core.DomainMos.Members.Mos;
using OSS.Core.Services.Members;
using OSS.Core.WebApi.Controllers.Member.Reqs;
using OSS.Core.WebApi.Filters;

namespace OSS.Core.WebApi.Controllers.Member
{
    [AuthorizeMember]
    public class MemberController : Controller
    {
        private static readonly MemberService service=new MemberService();

        // GET: api/values
        [HttpPost]
        public UserRegisteResp Registe(UserRegisteReq req)
        {
            if (ModelState.IsValid)
            {
                var regRes = service.RegisteUser(req.value, req.pass_code, req.reg_type, MemberShiper.AppAuthorize);
                if (!regRes.IsSuccess)
                    return regRes.ConvertToResult<UserRegisteResp>();

                var token=MemberShiper.GetToken()
            }

        }

        // GET: api/values
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<string>> GetAsync()
        {
            await Task.Delay(1000);
            return new string[] { "value1", "value2" };
        }
    }
}
