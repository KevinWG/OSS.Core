namespace TM.WMS;

public class AreaStockChangeReq
{
    public bool is_add { get; set; }

    /// <summary>
    ///    添加项目
    /// </summary>
    public AreaStockMo add_item { get; set; }

    /// <summary>
    ///  记录更新信息
    /// </summary>
    public AreaStockUpdateReq update_item { get; set; }
}



public class AreaStockUpdateReq
{
    /// <summary>
    ///  对应库存Id
    /// </summary>
    public long stock_id { get; set; }


    /// <summary>
    /// 可用变化数量
    /// </summary>
    public decimal change_basic_count { get; set; }
}



//internal class StockAndRecordChange
//{
//    /// <summary>
//    ///    库存变动信息
//    /// </summary>
//    public AreaStockChange stock_change { get; set; }

//    /// <summary>
//    ///    库存变动的明细项
//    /// </summary>
//    public StockRecordMo record { get; set; }

//}

