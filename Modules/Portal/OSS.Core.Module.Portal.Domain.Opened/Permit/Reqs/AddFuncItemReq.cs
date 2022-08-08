using System.ComponentModel.DataAnnotations;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

public class AddFuncItemReq: ChangeFuncItemReq
{
    /// <summary>
    ///  权限编码
    /// </summary>
    [Required(ErrorMessage = "权限编码不能为空!")]
    [MaxLength(100,ErrorMessage = "权限编码不能超过100个字符!")]
    public string code { get; set; } = default!;


    /// <summary>
    ///  父级权限编码
    /// </summary>
    [MaxLength(100, ErrorMessage = "父级权限编码不能超过100个字符!")]
    public string? parent_code { get; set; }
}


public class ChangeFuncItemReq
{
    /// <summary>
    ///  权限名称
    /// </summary>
    [Required(ErrorMessage = "权限名称不能为空!")]
    [MaxLength(200, ErrorMessage = "权限名称不能超过200个字符!")]
    public string title { get; set; } = default!;

}

public static class AddFuncItemReqExtension
{
    public static FuncMo ToMo(this AddFuncItemReq req)
    {
        var mo =new FuncMo()
        {
            code        = req.code,
            title       = req.title,
            parent_code = req.parent_code
        };

        mo.FormatBaseByContext();
        return mo;
    }

}