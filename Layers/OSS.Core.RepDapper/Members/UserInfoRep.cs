using Dapper;
using MySql.Data.MySqlClient;
using OSS.Core.DomainMos.Members.Mos;
using OSS.Core.DomainMos.Members.RepInterfaces;

namespace OSS.Core.RepDapper.Members
{
    public class UserInfoRep:IUserInfoRep
    {

        public void Test(UserInfoMo ent)
        {
            MySqlConnection con =
                new MySqlConnection("server=127.0.0.1;database=oss_core;uid=root;pwd=123456;charset=utf8mb4");
           // int row = con.Execute("insert into user_info(id,nick_name,email) values(1,'sdc','mmmm@qq.com')");
            var result = con.QuerySingle<UserInfoMo>("select * from user_info where id=@id",new {id=1});
        }
    }
}
