using OSS.Common;

namespace TM.Module.Product.Client;

/// <summary>
/// Product 模块接口客户端
/// </summary>
public static class ProductRemoteClient //: IProductClient
{
    /// <summary>
    ///  Sku 接口
    /// </summary>
    public static ISkuOpenService Sku {get; } = SingleInstance<SkuHttpClient>.Instance;
    /// <summary>
    ///  Product 接口
    /// </summary>
    public static ISpuOpenService Spu {get; } = SingleInstance<SpuHttpClient>.Instance;
    /// <summary>
    ///  ProductCategory 接口
    /// </summary>
    public static ISpuCategoryOpenService SpuCategory {get; } = SingleInstance<SpuCategoryHttpClient>.Instance;
}








