using System.ComponentModel.DataAnnotations;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///   添加专题请求
/// </summary>
public class AddTopicReq
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required(ErrorMessage = "专题名称不能为空")]
    [StringLength(80, ErrorMessage = "专题名称不能超过80个字符")]
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///   专题logo
    /// </summary>
    public string? avatar { get; set; }

    /// <summary>
    ///  专题简介
    /// </summary>
    [StringLength(200, ErrorMessage = "专题名称不能超过200个字符")]
    public string brief { get; set; }
}

/// <summary>
///  专题转化映射
/// </summary>
public static class AddTopicReqMap
{
    /// <summary>
    ///  转化为专题对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static TopicMo MapToTopicMo(this AddTopicReq req)
    {
        var mo = new TopicMo
        {
            name = req.name,
            avatar = req.avatar,
            brief = req.brief,
        };

        mo.FormatBaseByContext();
        return mo;
    }
}
