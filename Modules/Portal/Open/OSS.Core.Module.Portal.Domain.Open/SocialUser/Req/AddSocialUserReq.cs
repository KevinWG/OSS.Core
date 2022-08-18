using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;


public class AddOrUpdateSocialReq
{
    /// <summary>
    ///  应用平台
    /// </summary>
    public AppPlatform social_plat { get; set; }

    /// <summary>
    ///   社交平台Id
    /// </summary>
    public string social_app_key { get; set; }

    /// <summary>
    ///  应用的用户Id
    /// </summary>
    public string app_user_id { get; set; }

    /// <summary>
    ///  平台下统一用户Id
    /// </summary>
    public string app_union_id { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public SexType sex { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string nick_name { get; set; }

    /// <summary>
    ///  头像
    /// </summary>
    public string head_img { get; set; }


    /// <summary>
    ///   第三方账号的其他扩展信息
    /// </summary>
    public string ext { get; set; }

}

