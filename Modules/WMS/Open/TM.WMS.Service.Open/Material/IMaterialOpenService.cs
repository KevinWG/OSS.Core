using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  Material 领域对象开放接口
/// </summary>
public interface IMaterialOpenService
{
    /// <summary>
    ///  物料管理搜索列表（附带通过Id生成的PassToken）
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<MaterialView>> MSearch(SearchReq req);

    /// <summary>
    ///  通过id获取物料详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<MaterialView>> Get(long id);

    /// <summary>
    ///  通过id获取有效可用状态的物料详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<MaterialView>> GetUseable(long id);

    /// <summary>
    ///  设置物料可用状态
    /// </summary>
    /// <param name="id">通过Id生成的通行码</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<Resp> SetUseable(long id, ushort flag);

    /// <summary>
    ///  添加物料对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddMaterialReq req);


    /// <summary>
    ///  修改物料对象
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> Update(long id, UpdateMaterialReq req);
}
