using Microsoft.AspNetCore.Mvc;
using OSS.Core.Context.Attributes;

namespace TM.WMS;

/// <summary>
///  仓储管理 WebApi基类
/// </summary>
[ModuleMeta(WMSConst.ModuleName)]
[Route($"{WMSConst.ModuleName}/[controller]/[action]")]
public class BaseWMSController : ControllerBase
{
}

