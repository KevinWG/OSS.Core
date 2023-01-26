
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

namespace TM.Module.Product;

/// <summary>
///  ProductCategory 对象实体 
/// </summary>
public class SpuCategoryMo : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    ///  分类名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///  排序编号
    /// </summary>
    public int order { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int level { get; set; }

    /// <summary>
    /// 父级编号
    /// </summary>
    public long parent_id { get; set; }
}


public class SpuCategoryTreeItem//:IIndentWithChildren<SpuCategoryTreeItem>
{
    /// <summary>
    ///  id
    /// </summary>
    public long id { get; set; }

    /// <summary>
    ///  分类名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///  排序编号
    /// </summary>
    public int order { get; set; }

    /// <summary>
    /// 父级编号
    /// </summary>
    public long parent_id { get; set; }

    /// <summary>
    ///  子级
    /// </summary>
    public IList<SpuCategoryTreeItem> children { get; set; }
}


/// <summary>
/// 
/// </summary>
public static class SpuCategoryTreeItemMap
{
    /// <summary>
    ///  转化为属性对象
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public static SpuCategoryTreeItem ToTreeItem(this SpuCategoryMo category)
    {
        return new SpuCategoryTreeItem()
        {
            name      = category.name,
            order     = category.order,
            parent_id = category.parent_id,
            id        = category.id,
        };
    }
}