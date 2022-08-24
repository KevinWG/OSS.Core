using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线管道节点添加请求
/// </summary>
public class AddPipeReq
{
    /// <summary>
    /// 管道节点名称
    /// </summary>
    [Required(ErrorMessage = "流水线名称不能为空")]
    [StringLength(200, ErrorMessage = "流水线名称不能超过200个字符")]
    public string name { get; set; } = default!;
    
    ///// <summary>
    ///// 管道类型
    ///// </summary>
    //public PipeType type { get; set; }

    /// <summary>
    /// 父级流水线 id
    /// </summary>
    [Range(1,long.MaxValue, ErrorMessage = "流水线Id不能为空！")]
    public long parent_id { get; set; }
}
