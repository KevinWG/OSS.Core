using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Common.Extension;

namespace OSS.Core.Module.Article;

/// <summary>
///  专题 服务
/// </summary>
public class TopicService : ITopicOpenService
{
    private static readonly TopicRep _TopicRep = new();

    /// <inheritdoc />
    public async Task<TokenPageListResp<TopicMo>> MSearch(SearchReq req)
    {
        var pageList = await _TopicRep.Search(req);
        return pageList.ToTokenPageRespWithIdToken();
    }

    /// <inheritdoc />
    public async Task<IResp<TopicMo>> Get(long id)
    {
        var  getRes = await _TopicRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<TopicMo>().WithResp(getRes,"未能找到专题信息");
    }

    /// <inheritdoc />
    public async Task<IResp<TopicMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;
        
        return getRes.data.status >= 0 
            ? getRes
            : new Resp<TopicMo>().WithResp(RespCodes.OperateObjectNull,"未能找到有效可用的专题信息");
    }

    /// <inheritdoc />
    public Task<IResp> SetUseable(string pass_token, ushort flag)
    {
        var id = PassTokenHelper.GetData(pass_token).ToInt64();

        return _TopicRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddTopicReq req)
    {
        var mo = req.MapToTopicMo();

        await _TopicRep.Add(mo);
        return new LongResp(mo.id);
    }
}
