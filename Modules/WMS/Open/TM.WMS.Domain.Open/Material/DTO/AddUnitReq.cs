using System.ComponentModel.DataAnnotations;

namespace TM.WMS;

/// <summary>
///   添加单位请求
/// </summary>
public class AddUnitReq
{
    [Required(ErrorMessage = "单位名称不能为空!")]
    [StringLength(30,ErrorMessage = "单位名称不能超过30个字符")]
    public string name { get; set; } = string.Empty;
}

