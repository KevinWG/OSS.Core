using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Notify;

/// <summary>
///  获取
/// </summary>
public class TemplateService : ITemplateCommonService
{
    private static readonly TemplateRep _templateRep = new();


    /// <inheritdoc />
    public async Task<PageListResp<TemplateMo>> Search(SearchReq req)
    {
        return new PageListResp<TemplateMo>(await _templateRep.SearchTemplates(req));
    }

    /// <inheritdoc />
    public Task<IResp<TemplateMo>> Get(long id) => _templateRep.GetById(id);

    /// <inheritdoc />
    public Task<IResp> Update(long id, AddTemplateReq req)
    {
        return _templateRep.Update(id, req);
    }

    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _templateRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<IResp> Add(AddTemplateReq req)
    {
        var template = req.ToTemplate();
        template.FormatBaseByContext();

        await _templateRep.Add(template);

        return Resp.DefaultSuccess;
    }
}