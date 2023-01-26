using Microsoft.AspNetCore.Mvc;
using OSS.Core.Context.Attributes;

namespace TM.Module.Product;

/// <summary>
///  产品 WebApi基类
/// </summary>
[ModuleMeta(ProductConst.ModuleName)]
[Route($"{ProductConst.ModuleName}/[controller]/[action]")]
public class BaseProductController : ControllerBase
{
}

