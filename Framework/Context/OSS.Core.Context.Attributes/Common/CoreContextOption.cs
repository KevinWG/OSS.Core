namespace OSS.Core.Context.Attributes
{
    /// <summary>
    /// 选项
    /// </summary>
    public class CoreContextOption
    {
        /// <summary>
        ///  服务端Web页面渲染时，跳转的错误页面
        /// </summary>
        public string ErrorPage { get; set; } = string.Empty;

        /// <summary>
        ///  服务端Web页面渲染时，跳转的登录页
        /// </summary>
        public string LoginPage { get; set; } = string.Empty;

        /// <summary>
        /// JS请求接口（Fetch或者AJAX形式）附带的头标识名称
        ///    附带此标识的接口，未登录或异常时返回JSON（不会跳转）
        /// </summary>
        public string JSRequestHeaderName = "x-core-app";
    }
}