using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.WorkFlow;

public class AddNodeRecordReq
{
    // [Required]
    // public string name { get; set; }
}


public static class AddNodeRecordReqMap
{
    public static NodeRecordMo MapToNodeRecordMo(this AddNodeRecordReq req)
    {
        var mo = new NodeRecordMo
        {
        };
        return mo;
    }
}
