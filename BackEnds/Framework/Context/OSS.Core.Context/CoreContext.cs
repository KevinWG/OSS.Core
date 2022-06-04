using System;

namespace OSS.Core.Context
{
    /// <summary>
    /// OSSCore 核心上下文信息
    /// </summary>
    public static partial class CoreContext
    {
        /// <summary>
        ///  全局ServiceProvider
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }
    }
    
}
