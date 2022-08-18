using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Portal;

public class PortalNameReq
{
    /// <summary>
    /// 登录注册类型
    /// </summary>
    public PortalNameType type { get; set; }

    /// <summary>
    /// 手机号 或者 邮箱
    /// </summary>
    [Required(ErrorMessage = "请填写账号信息!")]
    public string name { get; set; } = string.Empty;
}