using OSS.Core.Domain;

namespace TM.Module.Product;

/// <summary>
///   添加产品请求
/// </summary>
public class AddSpuReq
{
    // [Required]
    // public string name { get; set; }
}

/// <summary>
///  产品转化映射
/// </summary>
public static class AddProductReqMap
{
    /// <summary>
    ///  转化为产品对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static SpuMo MapToProductMo(this AddSpuReq req)
    {
        var mo = new SpuMo
        {
            // todo 添加实体映射
        };

        mo.FormatBaseByContext();
        return mo;
    }
}
