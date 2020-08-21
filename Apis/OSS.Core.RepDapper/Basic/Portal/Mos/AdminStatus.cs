namespace OSS.Core.RepDapper.Basic.Portal.Mos
{
    public enum AdminStatus
    {
        /// <summary>
        ///  删除
        /// </summary>
        Delete =-1000,

        /// <summary>
        ///  锁定
        /// </summary>
        Locked = -100,

        /// <summary>
        ///  正常
        /// </summary>
        Normal=0
    }

    public enum AdminType
    {
        /// <summary>
        ///  正常
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 超级管理员
        /// </summary>
        Supper=100,
    }
}
