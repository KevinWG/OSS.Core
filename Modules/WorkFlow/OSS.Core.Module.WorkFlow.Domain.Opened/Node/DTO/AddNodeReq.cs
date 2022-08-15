using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.WorkFlow;

public class AddNodeReq
{
    // [Required]
    // public string name { get; set; }
}


public static class AddNodeReqMap
{
    public static NodeMo MapToNodeMo(this AddNodeReq req)
    {
        var mo = new NodeMo
        {
        };
        return mo;
    }
}
