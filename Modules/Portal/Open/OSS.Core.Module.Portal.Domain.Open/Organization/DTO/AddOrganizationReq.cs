using System.ComponentModel.DataAnnotations;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

/// <summary>
///   添加组织机构请求
/// </summary>
public class AddOrganizationReq
{
    /// <summary>
    /// 组织机构名称
    /// </summary>
    [Required(ErrorMessage = "名称不能为空")]
    [StringLength(300,ErrorMessage = "名称不能超过300个字符")]
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///  组织机构名称
    /// </summary>
    public OrgType org_type { get; set; }

}

/// <summary>
///  组织机构转化映射
/// </summary>
public static class AddOrganizationReqMap
{
    /// <summary>
    ///  转化为组织机构对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static OrganizationMo MapToOrganizationMo(this AddOrganizationReq req)
    {
        var mo = new OrganizationMo
        {
            org_type = req.org_type,
            name = req.name,
        };

        mo.FormatBaseByContext();
        return mo;
    }
}
