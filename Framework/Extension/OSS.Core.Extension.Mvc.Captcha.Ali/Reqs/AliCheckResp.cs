namespace OSS.Core.Extension.Mvc.Captcha;

public class AliCheckResp
{
    public string code { get; set; }   

    public string msg { get; set; }

    /// <summary>
    ///  错误消息
    /// </summary>
    public string Message { get; set; }
}