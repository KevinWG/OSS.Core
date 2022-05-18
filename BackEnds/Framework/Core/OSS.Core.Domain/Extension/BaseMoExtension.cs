
using OSS.Common.Extension;
using OSS.Common.Helpers;
using OSS.Core.Context;

namespace OSS.Core.Domain.Extension
{
    public static class BaseMoExtension
    {
        /// <summary>
        ///  从上下文中初始化基础信息
        /// </summary>
        /// <param name="t"></param>
        public static void InitialBaseFromContext(this BaseOwnerMo<long> t)
        {
            if (t.id <= 0)
                t.id = NumHelper.SmallSnowNum();

            if (t.owner_uid <= 0)
            {
                var userIdentity = CoreUserContext.Identity;
                if (userIdentity != null)
                {
                    t.owner_uid = userIdentity.id.ToInt64();
                }
            }
            //t.owner_tid   = appIdentity.tenant_id.ToInt64();
            t.add_time = DateTime.Now.ToUtcSeconds();
            //t.from_app_id = appIdentity.app_id;

        }
    }
}
