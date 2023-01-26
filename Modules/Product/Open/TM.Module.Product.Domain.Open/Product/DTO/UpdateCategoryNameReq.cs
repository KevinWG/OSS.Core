using System.ComponentModel.DataAnnotations;

namespace TM.Module.Product;

/// <summary>
///  更新物料分组信息
/// </summary>
public class UpdateSCNameReq
{
    /// <summary>
    /// 类别名称
    /// </summary>
    [Required(ErrorMessage = "分组名称不能为空！")]
    [StringLength(30, ErrorMessage = "分组名称不能超过30个字符")]
    public string name { get; set; } = string.Empty;
}