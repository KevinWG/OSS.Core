using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  MaterialCategory 领域对象开放接口
/// </summary>
public interface IMCategoryOpenService
{
    /// <summary>
    ///  获取当前物料目录信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<MCategoryMo>> Get(long id);

    /// <summary>
    ///  物料所有目录
    /// </summary>
    /// <returns></returns>
    Task<ListResp<MCategoryTreeItem>> AllCategories();

    /// <summary>
    /// 添加物料目录
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddCategoryReq req);

    /// <summary>
    ///  设置物料目录可用状态
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<Resp> SetUseable(long id, ushort flag);

    /// <summary>
    /// 编辑物料目录
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> UpdateName(long id, UpdateMCategoryNameReq req);

    /// <summary>
    /// 编辑 物料目录 排序值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="order">排序值</param>
    /// <returns></returns>
    Task<Resp> UpdateOrder(long id, int order);

}