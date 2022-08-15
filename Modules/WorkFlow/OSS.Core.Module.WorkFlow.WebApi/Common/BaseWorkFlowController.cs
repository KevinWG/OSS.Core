using Microsoft.AspNetCore.Mvc;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.WorkFlow;

[ModuleMeta(WorkFlowConst.ModuleName)]
[Route($"WorkFlowConst.ModuleName/[controller]/[action]")]
public class BaseWorkFlowController : ControllerBase
{
}

