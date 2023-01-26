//using OSS.Common;
//using OSS.Common.Resp;

//namespace TM.WMS;

///// <summary>
/////  MSku 领域对象开放接口
///// </summary>
//public interface IMCodeOpenService
//{
//    /// <summary>
//    ///  物料库存单位列表（附带通过Id生成的PassToken）
//    /// </summary>
//    /// <param name="m_id">物料Id</param>
//    /// <returns></returns>
//    Task<TokenListResp<MSkuMo>> MList(long m_id);

//    /// <summary>
//    ///  通过id获取物料库存单位详情
//    /// </summary>
//    /// <param name="id"></param>
//    /// <returns></returns>
//    Task<Resp<MSkuMo>> Get(long id);

//    /// <summary>
//    ///  设置物料库存单位可用状态
//    /// </summary>
//    /// <param name="pass_token">通过Id生成的通行码</param>
//    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
//    /// <returns></returns>
//    Task<Resp> SetUseable(string pass_token, ushort flag);

//    /// <summary>
//    ///  添加物料库存单位对象
//    /// </summary>
//    /// <param name="req"></param>
//    /// <returns></returns>
//    Task<LongResp> Add(AddMSkuReq req);


//    /// <summary>
//    ///  编辑物料库存单位对象
//    /// </summary>
//    /// <param name="pass_token"></param>
//    /// <param name="req"></param>
//    /// <returns></returns>
//    Task<Resp> Edit(string pass_token, AddMSkuReq req);
//}
