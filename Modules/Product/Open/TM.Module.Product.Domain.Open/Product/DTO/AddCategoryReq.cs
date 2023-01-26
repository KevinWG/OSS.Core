namespace TM.Module.Product;

/// <summary>
/// 添加物料分类请求
/// </summary>
public class AddSpuCategoryReq : UpdateSCNameReq
{
    /// <summary>
    /// 父节点Id
    /// </summary>
    public long parent_id { get; set; }

    /// <summary>
    ///  排序编号
    /// </summary>
    public int order { get; set; }
}

