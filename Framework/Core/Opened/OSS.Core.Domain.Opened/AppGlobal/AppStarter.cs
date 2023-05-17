using Microsoft.Extensions.DependencyInjection;

namespace OSS.Core
{
    /// <summary>
    ///  模块注册器
    /// </summary>
    public abstract class AppStarter
    {
        /// <summary>
        ///   启动配置相关服务信息
        /// </summary>
        /// <param name="serviceCollection"></param>
        public abstract void Start(IServiceCollection serviceCollection);
    }

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
