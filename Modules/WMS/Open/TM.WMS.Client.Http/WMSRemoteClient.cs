using OSS.Common;

namespace TM.WMS.Client;

/// <summary>
/// WMS 模块接口客户端
/// </summary>
public static class WMSRemoteClient //: IWMSClient
{
    /// <summary>
    ///  BatchCode 接口
    /// </summary>
    public static IBatchOpenService BatchCode {get; } = SingleInstance<BatchHttpClient>.Instance;
 
    /// <summary>
    ///  StockApply 接口
    /// </summary>
    public static IStockApplyOpenService StockApply {get; } = SingleInstance<StockApplyHttpClient>.Instance;

    /// <summary>
    ///  MaterialCategory 接口
    /// </summary>
    public static IMCategoryOpenService MCategory { get; } = SingleInstance<MCategoryHttpClient>.Instance;

    /// <summary>
    ///  Material 接口
    /// </summary>
    public static IMaterialOpenService Material { get; } = SingleInstance<MaterialHttpClient>.Instance;

    /// <summary>
    ///  Unit 接口
    /// </summary>
    public static IUnitOpenService Unit { get; } = SingleInstance<UnitHttpClient>.Instance;



    /// <summary>
    ///  Stock 接口
    /// </summary>
    public static IStockOpenService Stock {get; } = SingleInstance<StockHttpClient>.Instance;

    

    /// <summary>
    ///  Warehouse 接口
    /// </summary>
    public static IWarehouseOpenService Warehouse {get; } = SingleInstance<WarehouseHttpClient>.Instance;

}














