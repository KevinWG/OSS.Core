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
    [Required(ErrorMessage = "标题不能为空！")]
    public string title { get; set; } = default!;

    /// <summary>
    ///  内容摘要
    /// </summary>
    public string brief { get; set; } = string.Empty;

    /// <summary>
    ///  内容头图
    /// </summary>
    public string head_img { get; set; } = string.Empty;

    /// <summary>
    ///  作者
    /// </summary>
    public string author { get; set; } = string.Empty;

    /// <summary>
    ///  分类id
    /// </summary>
    public long category_id { get; set; }

    /// <summary>
    ///  文章内容
    /// </summary>
    [Required(ErrorMessage = "内容不能为空！")]
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
            title    = req.title,
            brief    = req.brief,
            author   = req.author,
            head_img = req.head_img,

            category_id = req.category_id,

            body     = req.body,
            attaches = req.attaches,
            tags     = req.tags,
        };
        mo.FormatBaseByContext();
        return mo;
    }
}
