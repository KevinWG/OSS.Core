using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 添加请求
/// </summary>
public class AddPipeReq
{
    /// <summary>
    /// 管道节点名称
    /// </summary>
    [Required(ErrorMessage = "流水线名称不能为空")]
    [StringLength(200, ErrorMessage = "流水线名称不能超过200个字符")]
    public string name { get; set; } = default!;


    /// <summary>
    /// 管道类型
    /// </summary>
    public PipeType type { get; set; }
    
    /// <summary>
    /// 父级 Pipeline id
    /// </summary>
    public long parent_id { get; set; }

}
