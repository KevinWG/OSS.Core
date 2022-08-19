using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Link 添加请求
/// </summary>
public class AddLinkReq
{
    // [Required]
    // public string name { get; set; }
}


public static class AddLinkReqMap
{
    public static LinkMo MapToLinkMo(this AddLinkReq req)
    {
        var mo = new LinkMo
        {
        };
        return mo;
    }
}
