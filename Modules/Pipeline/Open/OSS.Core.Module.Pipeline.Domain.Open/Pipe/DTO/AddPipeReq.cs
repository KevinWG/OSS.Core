using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 添加请求
/// </summary>
public class AddPipeReq
{
    // [Required]
    // public string name { get; set; }
}


public static class AddPipeReqMap
{
    public static PipeMo MapToPipeMo(this AddPipeReq req)
    {
        var mo = new PipeMo
        {
        };
        return mo;
    }
}
