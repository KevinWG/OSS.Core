using Microsoft.AspNetCore.Mvc;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipeline WebApi基类
/// </summary>
[ModuleMeta(PipelineConst.ModuleName)]
[Route($"{PipelineConst.ModuleName}/[controller]/[action]")]
public class BasePipelineController : ControllerBase
{
}

