namespace OSS.Core.RepDapper.Basic.Portal.Mos
{
    /// <summary>
    ///     成员状态
    /// </summary>
    public enum UserStatus
    { 
        // 用户一旦注册不可能存在删除的情况
        // Delete=-1000,

        /// <summary>
        ///     拉黑
        /// </summary>
        Locked = -100,
        
        /// <summary>
        ///     第三方授权后等待选择是否绑定(否则跳过直接创建用户)
        /// </summary>
        WaitOauthChooseBind = -20,

        /// <summary>
        ///     等待授权后绑定
        /// </summary>
        WaitOauthBind = -15,

        /// <summary>
        ///  等待激活
        /// </summary>
        WaitActive=-10,

        /// <summary>
        ///     正常用户状态
        /// </summary>
        Normal = 0
    }

    public enum SexType
    {
        UnKnow = 0,
        Male = 1,
        Female = 2
    }
    
    /// <summary>
    ///     成员信息类型枚举
    /// </summary>
    public enum MemberInfoType
    {
        OnlyId,
        Info
    }


    /// <summary>
    ///  第三方账号登录后和系统账号绑定类型
    ///  当 OauthRegisterType 为 Bind 类型时
    ///     前端第三方授权后进入绑定页面，使用当前枚举
    /// </summary>
    public enum TempOauthUserBindType
    {
        /// <summary>
        ///  非第三方授权绑定
        /// </summary>
        None,

        /// <summary>
        /// 跳过绑定，直接进入系统
        /// （系统创建默认账号
        /// </summary>
        Skip,

        /// <summary>
        ///  绑定系统账号
        ///    选择绑定已有或创建新用户时
        /// </summary>
        Bind
    }



    /// <summary>
    ///     授权注册时绑定类型
    ///     可以根据平台自定义
    /// </summary>
    public enum OauthRegisterType
    {
        /// <summary>
        ///     直接注册
        /// </summary>
        JustRegister,

        /// <summary>
        ///  第三方授权后用户选择
        ///    1. 是否绑定已有或创建新用户
        ///    2. 跳过，系统创建默认用户
        /// </summary>
        ChooseBind,

        /// <summary>
        ///     绑定
        /// </summary>
        Bind


    }
}