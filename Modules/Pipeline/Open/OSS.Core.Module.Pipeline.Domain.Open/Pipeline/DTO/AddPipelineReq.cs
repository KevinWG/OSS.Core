using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线 添加请求
/// </summary>
public class AddPipelineReq
{
    /// <summary>
    /// 管道节点名称
    /// </summary>
    [Required(ErrorMessage = "流水线名称不能为空")]
    [StringLength(200, ErrorMessage = "流水线名称不能超过200个字符")]
    public string name { get; set; } = default!;
}
