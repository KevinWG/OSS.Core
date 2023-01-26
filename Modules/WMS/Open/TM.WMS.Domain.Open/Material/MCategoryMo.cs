using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
/// 物料目录
/// </summary>
public class MCategoryMo : BaseTenantOwnerAndStateMo<long>// BaseTenantOwnerAndStateMo<long>
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


public class MCategoryTreeItem //:IIndentWithChildren<SpuCategoryTreeItem>
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
    public IList<MCategoryTreeItem> children { get; set; }
}


/// <summary>
/// 
/// </summary>
public static class MCategoryTreeItemMap
{
    /// <summary>
    ///  转化为属性对象
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public static MCategoryTreeItem ToTreeItem(this MCategoryMo category)
    {
        return new MCategoryTreeItem()
        {
            name      = category.name,
            order     = category.order,
            parent_id = category.parent_id,
            id        = category.id,
        };
    }
}