using System.ComponentModel.DataAnnotations;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///   添加仓库请求
/// </summary>
public class AddWarehouseReq: UpdateWarehouseReq
{
    /// <summary>
    ///  父级仓库Id
    /// </summary>
    public long parent_id { get; set; }
}


/// <summary>
///  修改仓库请求
/// </summary>
public class UpdateWarehouseReq
{
    /// <summary>
    /// 名称 （确定后不可修改）
    /// </summary>
    [Required(ErrorMessage = "仓库名称不能为空!")]
    [StringLength(30, ErrorMessage = "仓库名称不能超过30个字符")]
    public string name { get; set; } = string.Empty;

    /// <summary>
    /// 备注信息
    /// </summary>
    [StringLength(300, ErrorMessage = "仓库备注不能超过300个字符")] 
    public string? remark { get; set; }
}


/// <summary>
///  仓库转化映射
/// </summary>
public static class AddWarehouseReqMap
{
    /// <summary>
    ///  转化为仓库对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <param name="id">仓库Id</param>
    /// <returns></returns>
    public static WarehouseMo MapToWarehouseMo(this AddWarehouseReq req, long id)
    {
        var mo = new WarehouseMo
        {
            name = req.name,
            parent_id = req.parent_id,
            remark = req.remark,
            id = id
        };

        mo.FormatBaseByContext();
        return mo;
    }
}
