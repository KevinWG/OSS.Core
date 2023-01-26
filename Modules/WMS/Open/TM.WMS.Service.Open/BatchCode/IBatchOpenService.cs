using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  BatchCode 领域对象开放接口
/// </summary>
public interface IBatchOpenService
{
    /// <summary>
    ///  批次号管理搜索列表（附带通过Id生成的PassToken）
    /// </summary>
    /// <returns></returns>
    Task<TokenPageListResp<BatchMo>> MSearch(SearchReq req);

    /// <summary>
    ///  通过id获取批次号详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<BatchMo>> Get(long id);

    /// <summary>
    ///  设置批次号可用状态
    /// </summary>
    /// <param name="pass_token">Id对应的通行码</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<Resp> SetUseable(string pass_token, ushort flag);

    /// <summary>
    ///  添加批次号对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddBatchCodeReq req);
}
