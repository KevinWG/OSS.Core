
#region Copyright (C)  Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 实体对象
*
*　　	创建人： osscore
*    	创建日期：
*       
*****************************************************************************/

#endregion

using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  文章 对象实体 
/// </summary>
public class ArticleMo : BaseOwnerAndStateMo<long>
{
    /// <summary>
    ///  文章名称
    /// </summary>
    public string title { get; set; } = string.Empty;

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
