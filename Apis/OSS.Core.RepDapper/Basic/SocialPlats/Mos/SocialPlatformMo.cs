

namespace OSS.Core.RepDapper.Basic.SocialPlats.Mos
{

    /// <summary>
    ///  社交平台实体
    /// </summary>
    public class SocialPlatformMo : BaseOwnerAndStateMo
    {
        /// <summary>
        ///  平台类型
        /// </summary>
        public SocialPlatform social_plat { get; set; }

        /// <summary>
        ///  名称
        /// </summary>
        public string name { get; set; }
    }

}
