using System;
using OSS.Common.Authrization;
using OSS.Common.ComUtils;
using OSS.Common.Extention;
using OSS.Core.Infrastructure.Mos;

namespace OSS.Core.Services
{
    public class BaseService
    {
        protected static void SetBaseUserInfo<T>(T t)
            where T : BaseUIdMo
        {
            t.u_id = MemberShiper.Identity.Id;
            SetBaseInfo(t);
        }


        protected static void SetBaseInfo<T>(T t)
            where T : BaseMo
        {
            if (t.id == 0)
                t.id = NumUtil.SubTimeNum(MemberShiper.Identity.Id);
            t.create_time = DateTime.Now.ToUtcSeconds();
        }

    }
}