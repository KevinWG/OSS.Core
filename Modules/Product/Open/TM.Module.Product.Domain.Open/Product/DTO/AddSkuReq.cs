using OSS.Core.Domain;

namespace TM.Module.Product;

/// <summary>
///   添加Sku请求
/// </summary>
public class AddSkuReq
{
    // [Required]
    // public string name { get; set; }
}

/// <summary>
///  Sku转化映射
/// </summary>
public static class AddSkuReqMap
{
    /// <summary>
    ///  转化为Sku对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static SkuMo MapToSkuMo(this AddSkuReq req)
    {
        var mo = new SkuMo
        {
            // todo 添加实体映射
        };

        mo.FormatBaseByContext();
        return mo;
    }
}
