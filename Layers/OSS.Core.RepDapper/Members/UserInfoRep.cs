using OSS.Core.DomainMos.Members.Interfaces;
using OSS.Core.DomainMos.Members.Mos;

namespace OSS.Core.RepDapper.Members
{
    public class UserInfoRep : BaseRep<UserInfoMo,long>, IUserInfoRep
    {
        public UserInfoRep()
        {
            m_TableName = "user_info";
        }
    }
}
