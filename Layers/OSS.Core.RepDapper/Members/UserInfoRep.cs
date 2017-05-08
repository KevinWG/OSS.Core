using OSS.Core.DomainMos.Members.Interfaces;

namespace OSS.Core.RepDapper.Members
{
    public class UserInfoRep : BaseRep, IUserInfoRep
    {
        public UserInfoRep()
        {
            m_TableName = "user_info";
        }


    }
}
