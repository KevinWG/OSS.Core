
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
///  物料 对象实体 
/// </summary>
public class MaterialView : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///   物料编码
    /// </summary>
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///  物料目录Id
    /// </summary>
    public long c_id { get; set; }

    /// <summary>
    ///  物料形态
    /// </summary>
    public MaterialType type { get; set; }

    /// <summary>
    ///  单位(基础)
    /// </summary>
    public string basic_unit { get; set; } = string.Empty;

    /// <summary>
    ///  多单位扩展信息
    /// </summary>
    public IList<MultiUnitItem>? multi_units { get; set; } 

    /// <summary>
    ///  原厂型号
    /// </summary>
    public string? factory_serial { get; set; }

    /// <summary>
    ///  技术规格
    /// </summary>
    public string? tec_spec { get; set; }

    /// <summary>
    ///  技术规格
    /// </summary>
    public string? remark { get; set; }
}



/// <summary>
///  多单位项
/// </summary>
public class MultiUnitItem
{
    /// <summary>
    ///  单位
    /// </summary>
    public string unit { get; set; } = string.Empty;

    /// <summary>
    ///  相对基础单位系数
    /// </summary>
    public int ratio { get; set; }
}

/// <summary>
///  物料类型
/// </summary>
public enum MaterialType
{
    /// <summary>
    /// 原材料
    /// </summary>
    Raw = 0,

    /// <summary>
    ///  半成品
    /// </summary>
    Semi =100,

    /// <summary>
    ///  成品
    /// </summary>
    Standard=200
}

