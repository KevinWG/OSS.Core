#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 配置信息辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace OSS.Core.Infrastructure.Utils
{
    public static class ConfigUtil
    {
        private static IConfiguration config;
        public static IConfiguration Configuration {
            get
            {
                if (config != null) return config;
                var configBuilder =
                    new ConfigurationBuilder().Add(new JsonConfigurationSource()
                    {
                        Path = "appsettings.json",
                        ReloadOnChange = true
                    });
                config = configBuilder.Build();
                return config;

            }
            set { config = value; }
        }
        
        /// <summary>
        /// Gets a configuration sub-section with the specified key.
        /// </summary>
        /// <param name="key">The key of the configuration section.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" />.</returns>
        /// <remarks>
        ///     This method will never return <c>null</c>. If no matching sub-section is found with the specified key,
        ///     an empty <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" /> will be returned.
        /// </remarks>
        public static IConfigurationSection GetSection(string key)
        {
            return Configuration?.GetSection(key);
        }



        public static string GetConnectionString(string name)
        {
            return Configuration.GetConnectionString(name);
        }

        /// <summary>
        /// Gets the immediate descendant configuration sub-sections.
        /// </summary>
        /// <returns>The configuration sub-sections.</returns>
        public static IEnumerable<IConfigurationSection> GetChildren()
        {
            return Configuration?.GetChildren();
        }
    }
}
