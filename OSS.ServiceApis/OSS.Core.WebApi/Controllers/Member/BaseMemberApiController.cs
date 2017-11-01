using Microsoft.AspNetCore.Mvc;
using OSS.Core.WebApi.Filters;

namespace OSS.Core.WebApi.Controllers.Member
{
    [Area("member")]
    [AuthorizeMember]
    public class BaseMemberApiController : BaseController
    {

    }
}
