namespace OSS.Core.Shared.Domain
{
    /// <summary>
    ///     通用状态码 （如果需要更多状态需要自定义枚举，此值不再新增）
    ///     不同的领域对象可能会一到多个
    /// </summary>
    public enum CommonStatus
    {
        /// <summary>
        ///    删除
        /// </summary>
        Deleted = -10000,

        /// <summary>
        /// 待激活
        /// </summary>
        UnActived = -100,

        /// <summary>
        ///   正常
        /// </summary>
        Original = 0,

        /// <summary>
        ///  结束
        /// </summary>
        Finished = 10000
    }
}