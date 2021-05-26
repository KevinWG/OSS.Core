using System;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Enums;
using OSS.Common.Extension;
using OSS.Common.Helpers;
using OSS.Core.Context;

namespace OSS.Core.RepDapper
{
    /// <inheritdoc />
    public class BaseOwnerAndStateMo : BaseOwnerMo
    {
        /// <summary>
        /// 状态信息
        /// </summary>
        public CommonStatus status { get; set; }
    }

    public class BaseOwnerMo : BaseMo<long>
    {
        /// <summary>
        ///  【创建/归属】用户Id
        /// </summary>
        public long owner_uid { get; set; }

        ///// <summary>
        /////  【创建/归属】租户Id
        ///// </summary>
        //public long owner_tid { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public long add_time { get; set; }

        ///// <summary>
        /////  数据应用来源
        ///// </summary>
        //public string from_app_id { get; set; }
    }


    /// <summary>
    ///   基础实体类扩展赋值
    /// </summary>
    public static class BaseTenantMoExtension
    {
        /// <summary>
        ///  从上下文中初始化基础信息
        /// </summary>
        /// <param name="t"></param>
        public static void InitialBaseFromContext(this BaseOwnerMo t)
        {
            if (t.id<=0)
                t.id = NumHelper.SmallSnowNum();

            var appIdentity = CoreAppContext.Identity;
            t.owner_uid = CoreUserContext.Identity?.id.ToInt64()??0;

            //t.owner_tid   = appIdentity.tenant_id.ToInt64();
            t.add_time    = DateTime.Now.ToUtcSeconds();
            //t.from_app_id = appIdentity.app_id;
            
        }
    }
}
