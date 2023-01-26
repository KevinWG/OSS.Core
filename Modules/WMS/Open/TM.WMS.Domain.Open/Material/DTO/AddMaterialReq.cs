using System.ComponentModel.DataAnnotations;

namespace TM.WMS;

/// <summary>
///   添加物料请求
/// </summary>
public class AddMaterialReq: UpdateMaterialReq
{
    /// <summary>
    ///   物料编码
    /// </summary>
    [StringLength(80, ErrorMessage = "物料编码不能超过80个字符")]
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///  单位(基础)
    /// </summary>
    [StringLength(30, ErrorMessage = "单位名称不能超过30个字符")]
    public string basic_unit { get; set; } = string.Empty;
}

/// <summary>
///  修改物料请求信息
/// </summary>
public class UpdateMaterialReq
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [StringLength(180, ErrorMessage = "物料名称不能超过100个字符")]
    public string name { get; set; } = string.Empty;
    
    /// <summary>
    ///  物料目录Id
    /// </summary>
    public long c_id { get; set; }

    /// <summary>
    ///  物料形态
    /// </summary>
    public MaterialType type { get; set; }
    
    /// <summary>
    ///  原厂型号
    /// </summary>
    [StringLength(200, ErrorMessage = "型号不能超过200个字符")]
    public string? factory_serial { get; set; }

    /// <summary>
    ///  规格参数
    /// </summary>
    [StringLength(200, ErrorMessage = "规格参数不能超过200个字符")]
    public string? tec_spec { get; set; }

    /// <summary>
    ///  备注
    /// </summary>
    public string? remark { get; set; }

    /// <summary>
    ///  多单位信息
    /// </summary>
    public List<MultiUnitItem>? multi_units { get; set; }
}

