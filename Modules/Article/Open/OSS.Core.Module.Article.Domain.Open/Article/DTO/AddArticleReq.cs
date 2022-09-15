using System.ComponentModel.DataAnnotations;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  Article 添加请求
/// </summary>
public class AddArticleReq
{
    /// <summary>
    ///  文章名称
    /// </summary>
    [Required]
    public string title { get; set; } = default!;

    /// <summary>
    ///  分类id
    /// </summary>
    public long category_id { get; set; }

    /// <summary>
    ///  文章内容
    /// </summary>
    [Required]
    public string body { get; set; } = default!;

    /// <summary>
    /// 附件信息列表
    /// </summary>
    public string? attaches { get; set; }

    /// <summary>
    /// 标签 多个标签 “|” 分割
    /// </summary>
    public string? tags { get; set; }
}

/// <summary>
/// 转化映射
/// </summary>
public static class AddArticleReqMap
{
    /// <summary>
    ///  转化为文章对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static ArticleMo MapToArticleMo(this AddArticleReq req)
    {
        var mo = new ArticleMo
        {
            title = req.title,
            category_id = req.category_id,

            body = req.body,
            attaches = req.attaches,
            tags = req.tags,
        };
        mo.FormatBaseByContext();
        return mo;
    }
}
