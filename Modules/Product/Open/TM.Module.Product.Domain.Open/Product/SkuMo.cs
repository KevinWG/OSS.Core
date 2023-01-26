
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
///  Sku 对象实体 
/// </summary>
public class SkuMo : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    ///  产品id
    /// </summary>
    public long spu_id { get; set; }

    /// <summary>
    ///  标题
    /// </summary>
    public string title { get; set; } = string.Empty;


    /// <summary>
    ///  物料Id
    /// </summary>
    public long material_id { get; set; }

    /// <summary>
    ///  物料编号
    /// </summary>
    public string material_num { get; set; } = string.Empty;

    /// <summary>
    ///  单位
    /// </summary>
    public string unit { get; set; } = string.Empty;

    /// <summary>
    ///  单位单价
    /// </summary>
    public decimal price { get; set; }

    /// <summary>
    ///  销售规格属性
    /// </summary>
    public string sale_attrs { get; set; } = string.Empty;

}
