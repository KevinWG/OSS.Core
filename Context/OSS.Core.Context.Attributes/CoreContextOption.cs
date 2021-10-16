namespace OSS.Core.Context.Attributes
{
    public class CoreContextOption
    {
        /// <summary>
        ///  服务端Web页面渲染时，跳转的错误页面
        /// </summary>
        public string ErrorPage { get; set; }

        /// <summary>
        ///  服务端Web页面渲染时，跳转的404页面
        /// </summary>
        public string NotFoundPage { get; set; }

        /// <summary>
        ///  服务端Web页面渲染时，跳转的登录页
        /// </summary>
        public string LoginPage { get; set; }


        /// <summary>
        /// JS请求接口（Fetch或者AJAX形式）附带的头标识名称
        ///    附带此标识的接口，返回JSON（即使服务端渲染也不会跳转）
        /// </summary>
        public string JSRequestHeaderName = "x-core-app";
    }
}