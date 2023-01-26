using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace TM.WMS;

/// <summary>
///  批次号 开放 WebApi 
/// </summary>
public class BatchController : BaseWMSController, IBatchOpenService
{
    private static readonly IBatchOpenService _service = new BatchService();

    /// <summary>
    ///  查询批次号列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_batch_msearch)]
    public Task<TokenPageListResp<BatchMo>> MSearch([FromBody] SearchReq req)
    {
        return _service.MSearch(req);
    }



    /// <summary>
    ///  通过id获取批次号详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<BatchMo>> Get(long id)
    {
        return _service.Get(id);
    }

    
    /// <summary>
    ///  设置批次号可用状态
    /// </summary>
    /// <param name="pass_token">id对应的通行码</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_batch_modify)]
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        return _service.SetUseable(pass_token, flag);
    }



    /// <summary>
    ///  添加批次号对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_batch_modify)]
    public Task<LongResp> Add([FromBody] AddBatchCodeReq req)
    {
        return _service.Add(req);
    }
}
