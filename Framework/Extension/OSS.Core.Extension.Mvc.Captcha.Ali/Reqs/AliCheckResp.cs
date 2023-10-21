namespace OSS.Core.Extension.Mvc.Captcha;



/// <summary>
///  ali 验证响应结果
/// </summary>
public class AliCheckResp
{
    /// <summary>
    ///  结果编码
    /// </summary>
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///  结果消息
    /// </summary>
    public string msg { get; set; } = string.Empty;

    /// <summary>
    ///  错误消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
}