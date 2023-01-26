using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
/// 添加物料分类请求
/// </summary>
public class AddCategoryReq : UpdateMCategoryNameReq
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


/// <summary>
///  物料转化映射
/// </summary>
public static class AddCategoryReqMap
{
    /// <summary>
    ///  转化为物料对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static MCategoryMo MapToCategoryMo(this AddCategoryReq req)
    {
        var mo = new MCategoryMo()
        {
            parent_id = req.parent_id,
            name      = req.name,
        };

        mo.FormatBaseByContext();
        return mo;
    }
}
