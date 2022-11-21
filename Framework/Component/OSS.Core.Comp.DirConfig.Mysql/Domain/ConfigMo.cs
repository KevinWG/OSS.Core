using OSS.Core.Domain;

namespace OSS.Core.Comp.DirConfig.Mysql
{
    internal class ConfigMo :BaseTenantOwnerAndStateMo<long>// BaseOwnerMo<string>
    {
        ///// <summary>
        ///// 模块
        ///// </summary>
        //public string module { get; set; } = string.Empty;

        /// <summary>
        ///  项 key
        /// </summary>
        public string item_key { get; set; } = string.Empty;

        /// <summary>
        ///   总key
        /// </summary>
        public string list_key { get; set; } = string.Empty;


        /// <summary>
        /// 项Value
        /// </summary>
        public string item_val { get; set; } = string.Empty;

    }
}
