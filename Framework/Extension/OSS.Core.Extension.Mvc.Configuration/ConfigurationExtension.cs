using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OSS.Tools.Config;

namespace OSS.Core.Extension.Mvc.Configuration
{
    public static class ConfigurationExtension
    {
        /// <summary>
        ///  添加 OSSCore 配置文件工具
        /// </summary>
        public static void AddOssCoreConfiguration(this IServiceCollection service, IConfiguration configuration)
        {
            ConfigHelper.Configuration = configuration;
        }
    }
}