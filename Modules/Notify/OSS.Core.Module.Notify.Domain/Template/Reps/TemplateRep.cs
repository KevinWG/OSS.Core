using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Core.Module.AppCenter;

namespace OSS.Core.Module.Notify;

public class TemplateRep : BaseNotifyRep<TemplateMo>
{
    public TemplateRep() : base("p_notify_template")
    {
    }

    /// <summary>
    ///  查询模板列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<TemplateMo>> SearchTemplates(SearchReq req)
    {
        return SimpleSearch(req);
    }

    protected override string BuildSimpleSearch_FilterItemSql(string key, string value,
                                                              Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "notify_channel":
                var channel = value.ToInt32();
                sqlParas.Add("@notify_channel", channel);
                return "notify_channel=@notify_channel";

            case "notify_type":
                var type = value.ToInt32();
                sqlParas.Add("@notify_type", type);
                return "notify_type=@notify_type";
        }

        return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
    }

    /// <summary>
    ///  修改通知模板信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<Resp> Update(long id, AddTemplateReq req)
    {
        return Update(u => new
        {
            u.notify_type,
            u.notify_channel,
            u.title,

            u.content,
            u.channel_sender,
            u.sign_name,
            u.is_html,

            u.channel_template_code
        }, w => w.id == id, req);
    }

    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<Resp> UpdateStatus(long id, CommonStatus status)
    {
        return Update(u => new {u.status}, w => w.id == id, new {status});
    }

}