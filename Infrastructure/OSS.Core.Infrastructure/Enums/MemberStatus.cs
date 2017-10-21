namespace OSS.Core.Infrastructure.Enums
{
    /// <summary>
    ///  成员状态
    /// </summary>
    public enum MemberStatus
    {
        /// <summary>
        ///  拉黑
        /// </summary>
        Black = -1000,

        /// <summary>
        ///  第三方授权后等待选择是否绑定
        /// </summary>
        WaitOauthChooseBind=-20,

        /// <summary>
        ///  等待授权后绑定
        /// </summary>
        WaitOauthBind=-15,

        /// <summary>
        ///  正常用户状态
        /// </summary>
        Normal = 0
    }

    public enum Sex
    {
        Unknow=0,
        male = 1,
        Female =2
    }

}
