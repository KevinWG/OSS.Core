using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Notify;

public interface ITemplateOpenService
{
    /// <summary>
    ///  查询模板列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<TemplateMo>> Search(SearchReq req);

    /// <summary>
    ///  通过id获取模板信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<TemplateMo>> Get(long id);

    /// <summary>
    /// 修改模板信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Update(long id, AddTemplateReq req);

    /// <summary>
    ///  设置可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<IResp> SetUseable(long id, ushort flag);

    /// <summary>
    ///  添加模板信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Add(AddTemplateReq req);
}