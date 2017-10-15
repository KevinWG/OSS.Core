using Microsoft.AspNetCore.Mvc;
using OSS.Core.WebApi.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.Core.WebApi.Controllers.CoreApi
{
    [Area("core")]
    [AuthorizeMember]
    public class BaseCoreApiController : BaseController
    {
    }
}
