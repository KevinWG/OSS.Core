using Xunit;
using OSS.Core.DomainMos.Members.Mos;
using OSS.Core.RepDapper.Members;

namespace OSS.Core.RepTests
{
    public class UserInfoRepTests
    {
        [Fact]
        public void UserTest()
        {
            var mo = new UserInfoMo
            {
                email = "11111@qq.com",
                mobile = "1111111111"
            };

            var rep = new UserInfoRep();

            var res = rep.Insert(mo);
            mo.Id = res.Id;
            mo.email = "222222@qq.com";
            mo.mobile = "222222222222";

            rep.UpdateAll(mo); //  全量更新测试

            rep.Update(mo, m => new {m.email}, m => m.Id == 1); //  部分更新
            rep.DeleteSoft(m => m.Id == 16, mo); //  软删除
            rep.Get(m => m.Id == 16, mo); //  查询
        }
    }
}
