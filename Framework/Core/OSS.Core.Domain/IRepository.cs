
using OSS.Common.Resp;

namespace OSS.Core.Domain
{
    /// <summary>
    ///  仓储层接口
    /// </summary>
    /// <typeparam name="MType">实体类型</typeparam>
    /// <typeparam name="IdType">id类型</typeparam>
    public interface IRepository<MType, in IdType> //where MType : IDomainId<IdType>
    {
        /// <summary>
        ///   插入数据
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        Task Add(MType mo);

        /// <summary>
        ///   插入数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<Resp> AddList(IList<MType> list);

        /// <summary>
        /// 软删除，仅仅修改  status = CommonStatus.Delete 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Resp> SoftDeleteById(IdType id);

        /// <summary>
        /// 通过id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MType> GetById(IdType id);

        /// <summary>
        /// 通过id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RType> GetById<RType>(IdType id);
    }

}
