
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

namespace TM.WMS;

/// <summary>
///  仓库 对象实体
///     分两级
/// </summary>
public class WarehouseMo : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    /// 名称 （确定后不可修改）
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///  父级仓库Id
    /// </summary>
    public long parent_id { get; set; }
    
    /// <summary>
    /// 备注信息
    /// </summary>
    public string? remark { get; set; }

}



/// <summary>
/// 仓库实体（含父级名称）
/// </summary>
public class WarehouseFlatItem:WarehouseMo
{
    /// <summary>
    ///  父级名称
    /// </summary>
    public string parent_name { get; set; }


    /// <summary>
    ///  全称
    /// </summary>
    public string full_name => parent_id>0 ? $"{parent_name}/{name}" : name;
}


///// <summary>
///// 仓库实体（含子级列表）
///// </summary>
//public class WarehouseTreeItem : WarehouseMo //:IIndentWithChildren<WarehouseTreeItem>
//{
//    /// <summary>
//    /// 子仓库
//    /// </summary>
//    public List<WarehouseTreeItem> children { get; set; }
//}

public static class WarehouseMaps
{
    /// <summary>
    ///  转化为基础实体（含父级名称
    /// </summary>
    /// <param name="warehouse"></param>
    /// <returns></returns>
    public static WarehouseFlatItem ToItem<T>(this WarehouseMo warehouse)
    where T : WarehouseMo
    {
        var flatItem = new WarehouseFlatItem
        {
            id = warehouse.id,
            name = warehouse.name,
            parent_id = warehouse.parent_id,
            remark = warehouse.remark,
            add_time = warehouse.add_time,

            owner_uid = warehouse.owner_uid,
            status = warehouse.status,
            tenant_id = warehouse.tenant_id
        };
        return flatItem;
    }
}
