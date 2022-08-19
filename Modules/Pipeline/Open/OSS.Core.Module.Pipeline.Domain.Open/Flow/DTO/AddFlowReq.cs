using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 添加请求
/// </summary>
public class AddFlowReq
{
    // [Required]
    // public string name { get; set; }
}


public static class AddFlowReqMap
{
    public static FlowMo MapToFlowMo(this AddFlowReq req)
    {
        var mo = new FlowMo
        {
        };
        return mo;
    }
}
