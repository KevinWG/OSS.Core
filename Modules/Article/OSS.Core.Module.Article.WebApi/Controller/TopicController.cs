using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Article;

/// <summary>
///  专题 开放 WebApi 
/// </summary>
public class TopicController : BaseArticleController, ITopicOpenService
{
    private static readonly ITopicOpenService _service = new TopicService();

    /// <summary>
    ///  查询专题列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<TokenPageListResp<TopicMo>> MSearch([FromBody] SearchReq req)
    {
        return _service.MSearch(req);
    }

    /// <summary>
    ///  通过id获取专题详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<TopicMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///   通过id获取 有效可用的 专题 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<TopicMo>> GetUseable(long id)
    {
        return _service.GetUseable(id);
    }

    /// <summary>
    ///  设置专题可用状态
    /// </summary>
    /// <param name="pass_token"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        return _service.SetUseable(pass_token, flag);
    }



    /// <summary>
    ///  添加专题对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<LongResp> Add([FromBody] AddTopicReq req)
    {
        return _service.Add(req);
    }
}
