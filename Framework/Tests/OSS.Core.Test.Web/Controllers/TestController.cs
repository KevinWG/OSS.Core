using Microsoft.AspNetCore.Mvc;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Tools.Config;

namespace OSS.Core.Test.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [AppMeta(AppAuthMode.None, AppType.Single)]
    [ModuleMeta("test")]
    public class TestController : BaseController
    {
        [HttpGet]
        [AppMeta(AppAuthMode.AppSign, AppType.Single)]
        public IEnumerable<AppAccess> TestConfig()
        {
            List<AppAccess> _appAccessList =new List<AppAccess>();
            ConfigHelper.Configuration.GetSection("Access").Bind(_appAccessList);
            return _appAccessList;
        }
    }
    [AppMeta(AppAuthMode.AppSign,AppType.SystemManager)]
    public class BaseController : ControllerBase
    {
       
    }

}