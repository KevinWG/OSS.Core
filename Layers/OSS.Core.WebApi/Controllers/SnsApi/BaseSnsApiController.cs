using Microsoft.AspNetCore.Mvc;
using OSS.Core.WebApi.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.Core.WebApi.Controllers.SnsApi
{
    [Area("sns")]
    public class BaseSnsApiController : BaseController
    {
        [AllowNoSign]
        public string GetTest()
        {
            return "test";
        }
    }
}
