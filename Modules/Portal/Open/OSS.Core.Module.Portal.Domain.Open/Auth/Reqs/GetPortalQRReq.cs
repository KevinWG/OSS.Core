using OSS.Common.Resp;

namespace OSS.Core.Module.Portal
{
    public class GetPortalQRResp : Resp
    {
        /// <summary>
        ///  当前登录注册的编码，用来获取扫码登录注册结果
        /// </summary>
        public string portal_ticket { get; set; }

        /// <summary>
        ///  二维码的内容编码（url格式），自行生成二维码即可
        /// </summary>
        public string qr_code { get; set; }
    }
}
