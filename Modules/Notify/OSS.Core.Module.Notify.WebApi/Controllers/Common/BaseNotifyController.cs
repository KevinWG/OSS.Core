using Microsoft.AspNetCore.Mvc;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Notify;

[ModuleMeta(NotifyConst.ModuleName)]
[Route("notify/[controller]/[action]")]
public class BaseNotifyController : ControllerBase
{
}
