using Microsoft.Extensions.DependencyInjection;

namespace OSS.Core
{
    /// <summary>
    ///  模块注册器
    /// </summary>
    public abstract class ModuleStarter
    {
        /// <summary>
        ///   启动配置相关服务信息
        /// </summary>
        /// <param name="serviceCollection"></param>
        public abstract void Start(IServiceCollection serviceCollection);
    }
    
}
