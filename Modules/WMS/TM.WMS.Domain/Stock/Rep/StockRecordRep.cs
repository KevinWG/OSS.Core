using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  库位/区 对象仓储
/// </summary>
public class StockRecordRep : BaseWMSRep<StockRecordMo,long> 
{
    /// <inheritdoc />
    public StockRecordRep() : base("wms_stock_record")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<StockRecordMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }
    
}
