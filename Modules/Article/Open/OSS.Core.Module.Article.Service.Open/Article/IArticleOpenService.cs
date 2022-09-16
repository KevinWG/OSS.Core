using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Article;

/// <summary>
///  文章 领域对象开放接口
/// </summary>
public interface IArticleOpenService
{
    /// <summary>
    ///  文章管理查询列表
    /// </summary>
    /// <returns></returns>
    Task<TokenPageListResp<ArticleMo>> MSearch(SearchReq req);

    /// <summary>
    ///  文章查询列表
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
    ///  删除
    /// </summary>
    /// <param name="pass_token"></param>
    /// <returns></returns>
    Task<IResp> Delete(string pass_token);

    /// <summary>
    ///  编辑文章
    /// </summary>
    /// <param name="pass_token"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Edit(string pass_token, AddArticleReq req);

    /// <summary>
    ///  添加Article对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddArticleReq req);
}
