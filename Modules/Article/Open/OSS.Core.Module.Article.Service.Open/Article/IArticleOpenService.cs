using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Article;

/// <summary>
///  Article 领域对象开放接口
/// </summary>
public interface IArticleOpenService
{
    /// <summary>
    ///  查询Article列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<ArticleMo>> Search(SearchReq req);

    /// <summary>
    ///  通过id获取Article详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<ArticleMo>> Get(long id);

    /// <summary>
    ///  通过id获取有效可用状态的Article详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<ArticleMo>> GetUseable(long id);

    /// <summary>
    ///  设置Article可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<IResp> SetUseable(long id, ushort flag);

    /// <summary>
    ///  添加Article对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Add(AddArticleReq req);
}
