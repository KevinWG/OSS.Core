namespace OSS.Core.Domain
{
    /// <summary>
    ///  基础所有者实体
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public class BaseOwnerMo<IdType> : BaseMo<IdType>
    {
        /// <summary>
        ///  【创建/归属】用户Id
        /// </summary>
        public long owner_uid { get; set; }

        ///// <summary>
        /////  【创建/归属】租户Id
        ///// </summary>
        //public long owner_tid { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public long add_time { get; set; }

        ///// <summary>
        /////  数据应用来源
        ///// </summary>
        //public string from_app_id { get; set; }
    }



}