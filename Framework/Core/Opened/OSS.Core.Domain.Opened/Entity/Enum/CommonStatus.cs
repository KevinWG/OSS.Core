namespace OSS.Core.Domain
{
    /// <summary>
    /// 通用状态码 （领域对象可定义自有状态类型，此枚举适用简单状态管理）
    /// <para>【-10000】：已删除 </para>
    /// <para>  【-100】：未激活 </para>
    /// <para>     【0】：正常   </para>
    /// <para> 【10000】：已结束 </para>
    /// </summary>
    public enum CommonStatus
    {
        /// <summary>
        ///    已删除
        /// </summary>
        Deleted = -10000,

        /// <summary>
        /// 未激活
        /// </summary>
        UnActive = -100,

        /// <summary>
        ///   正常
        /// </summary>
        Original = 0,

        /// <summary>
        ///  已结束
        /// </summary>
        Finished = 10000
    }
}