using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Article;

/// <summary>
///  Topic 领域对象开放接口
/// </summary>
public interface ITopicOpenService
{
    /// <summary>
    ///  专题管理搜索列表（附带通过Id生成的PassToken）
    /// </summary>
    /// <returns></returns>
    Task<TokenPageListResp<TopicMo>> MSearch(SearchReq req);

    /// <summary>
    ///  通过id获取专题详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<TopicMo>> Get(long id);

    /// <summary>
    ///  通过id获取有效可用状态的专题详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<TopicMo>> GetUseable(long id);

    /// <summary>
    ///  设置专题可用状态
    /// </summary>
    /// <param name="pass_token">通过Id生成的通行码</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<IResp> SetUseable(string pass_token, ushort flag);

    /// <summary>
    ///  添加专题对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddTopicReq req);
}
