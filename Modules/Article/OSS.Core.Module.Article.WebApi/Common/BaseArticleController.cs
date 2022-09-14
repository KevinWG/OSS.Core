using Microsoft.AspNetCore.Mvc;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Article;

/// <summary>
///  Article WebApi基类
/// </summary>
[ModuleMeta(ArticleConst.ModuleName)]
[Route($"{ArticleConst.ModuleName}/[controller]/[action]")]
public class BaseArticleController : ControllerBase
{
}

