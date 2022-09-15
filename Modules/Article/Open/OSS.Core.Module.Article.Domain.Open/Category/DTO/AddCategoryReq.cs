using System.ComponentModel.DataAnnotations;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  Category 添加请求
/// </summary>
public class AddCategoryReq
{
    /// <summary>
    ///  分类名称
    /// </summary>
    [Required(ErrorMessage = "分类名称不能为空!")]
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///  父级Id
    /// </summary>
    public long parent_id { get; set; }
}


public static class AddCategoryReqMap
{
    public static CategoryMo MapToCategoryMo(this AddCategoryReq req)
    {
        var mo = new CategoryMo
        {
            name =  req.name,
            parent_id = req.parent_id,
        };

        mo.FormatBaseByContext();
        return mo;
    }
}
