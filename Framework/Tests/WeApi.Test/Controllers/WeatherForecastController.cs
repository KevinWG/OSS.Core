using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Extension.Mvc.Captcha;

namespace WeApi.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        //[CaptchaValidator]
        public IResp Get()
        {
            var app    = CoreContext.App.Identity;
            var tenant = CoreContext.Tenant.Identity;

            return Resp.DefaultSuccess;
        }
    }
}