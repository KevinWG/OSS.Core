namespace OSS.Core.Context
{
    using Provider =  OSS.Common.ServiceProvider;

    /// <summary>
    /// OSSCore 核心上下文信息
    /// </summary>
    public static partial class CoreContext
    {
        /// <summary>
        ///  全局ServiceProvider
        /// </summary>
        public static IServiceProvider? ServiceProvider
        {
            get => Provider.Provider; 
            set => Provider.Provider = value;
        }
    }
    
}
