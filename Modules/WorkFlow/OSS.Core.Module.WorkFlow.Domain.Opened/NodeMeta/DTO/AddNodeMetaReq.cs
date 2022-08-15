using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.WorkFlow;

public class AddNodeMetaReq
{
    // [Required]
    // public string name { get; set; }
}


public static class AddNodeMetaReqMap
{
    public static NodeMetaMo MapToNodeMetaMo(this AddNodeMetaReq req)
    {
        var mo = new NodeMetaMo
        {
        };
        return mo;
    }
}
