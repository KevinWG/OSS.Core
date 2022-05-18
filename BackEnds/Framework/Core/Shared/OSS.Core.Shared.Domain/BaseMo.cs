namespace OSS.Core.Domain
{
    /// <summary>
    ///  基础实体
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public class BaseMo<IdType>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public IdType id { get; set; }
    }

}