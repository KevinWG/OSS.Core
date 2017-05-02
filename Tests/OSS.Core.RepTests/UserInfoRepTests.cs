using Xunit;
using OSS.Core.DomainMos.Members.Mos;
using OSS.Core.RepDapper.Members;

namespace OSS.Core.RepTests
{
    public class UserInfoRepTests
    {
        [Fact]
        public void TestMethod1()
        {
            var rep = new UserInfoRep();
            rep.Insert(new UserInfoMo(){nick_name = "再次测试"});
        }
    }
}
