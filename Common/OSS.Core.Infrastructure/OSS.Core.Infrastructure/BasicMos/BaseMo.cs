using System;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Enums;
using OSS.Common.Extention;
using OSS.Common.Helpers;
using OSS.Core.Context;

namespace OSS.Core.Infrastructure.BasicMos
{
    /// <inheritdoc />
    public class BaseOwnerAndStateMo : BaseOwnerMo
    {
        /// <summary>
        /// 状态信息
        /// </summary>
        public CommonStatus status { get; set; }
    }

    public class BaseOwnerMo : BaseMo<string>
    {
        /// <summary>
        ///  【创建/归属】用户Id
        /// </summary>
        public string owner_uid { get; set; }

        /// <summary>
        ///  【创建/归属】租户Id
        /// </summary>
        public string owner_tid { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public long add_time { get; set; }

        /// <summary>
        ///  数据应用来源
        /// </summary>
        public string from_app_id { get; set; }
    }


    /// <summary>
    ///   基础实体类扩展赋值
    /// </summary>
    public static class BaseTenantMoExtention
    {
        /// <summary>
        ///  从上下文中初始化基础信息
        /// </summary>
        /// <param name="t"></param>
        public static void InitialBaseFromContext(this BaseOwnerMo t)
        {
            if (string.IsNullOrEmpty(t.id))
                t.id = NumHelper.SnowNum().ToString();

            var appIdentity = AppReqContext.Identity;
            t.owner_uid = UserContext.Identity?.id;

            t.owner_tid   = appIdentity.tenant_id;
            t.add_time    = DateTime.Now.ToUtcSeconds();
            t.from_app_id = appIdentity.app_id;
            
        }
    }
}
