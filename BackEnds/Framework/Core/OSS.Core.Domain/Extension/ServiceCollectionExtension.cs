using OSS.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   全局容器扩展方法
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        ///  注册 模块 服务启动配置
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <param name="serviceCollection"></param>
        public static void Register<TR>(this IServiceCollection serviceCollection) where TR : AppStarter, new()
        {
            new TR().Start(serviceCollection);
        }
        
    }
}
