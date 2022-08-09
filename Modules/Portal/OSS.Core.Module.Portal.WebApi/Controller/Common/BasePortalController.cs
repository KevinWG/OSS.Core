using Microsoft.AspNetCore.Mvc;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  门户基类
    /// </summary>
    [ModuleMeta(PortalConst.ModuleName)]
    [Route("portal/[controller]/[action]")]
    public class BasePortalController:ControllerBase
    {
    }
}
