
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
///  Category 对象实体 
/// </summary>
public class CategoryMo : BaseOwnerAndStateMo<long>
{
    /// <summary>
    ///  分类名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///  父级Id
    /// </summary>
    public long parent_id { get; set; }

    /// <summary>
    ///   深度
    /// </summary>
    public int deep_level { get; set; }
}
