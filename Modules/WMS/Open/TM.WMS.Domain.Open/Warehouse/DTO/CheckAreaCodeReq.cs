using System.ComponentModel.DataAnnotations;

namespace TM.WMS;

public class CheckAreaCodeReq
{
    /// <summary>
    /// 仓库Id
    /// </summary>
    public long w_id { get; set; }

    /// <summary>
    ///  库位/区 编码
    /// </summary>
    [Required(ErrorMessage = "编号不能为空")]
    public string code { get; set; } = string.Empty;
}