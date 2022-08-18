namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///     成员状态
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        ///     拉黑
        /// </summary>
        Locked = -100,

        /// <summary>
        ///  等待激活
        /// </summary>
        WaitActive = -10,

        /// <summary>
        ///     正常用户状态
        /// </summary>
        Normal = 0
    }


    /// <summary>
    ///  性别
    /// </summary>
    public enum SexType
    {
        /// <summary>
        ///  未知
        /// </summary>
        UnKnow = 0,
        /// <summary>
        /// 男
        /// </summary>
        Male = 1,
        /// <summary>
        ///  女
        /// </summary>
        Female = 2
    }

    /// <summary>
    ///     授权注册时绑定类型
    ///     可以根据平台自定义
    /// </summary>
    public enum SocialRegisterType
    {
        /// <summary>
        ///     第三方授权后默认直接注册
        /// </summary>
        JustRegister = 0,

        /// <summary>
        ///  第三方授权后用户主动绑定系统账号
        ///     需前端配合全局错误码决定：
        ///        ret =  是否绑定已有或创建新用户
        ///         2. 跳过，系统创建默认用户
        /// </summary>
        UserBind = 10,
    }
}