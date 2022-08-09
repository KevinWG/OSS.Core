using OSS.Core.Domain;

namespace OSS.Core.Comp.DirConfig.Mysql
{
    internal class DirConfigMo:BaseOwnerMo<string>
    {
        /// <summary>
        /// 模块
        /// </summary>
        public string module { get; set; } = string.Empty;

        /// <summary>
        /// 配置值
        /// </summary>
        public string config_val { get; set; } = string.Empty;

    }
}
