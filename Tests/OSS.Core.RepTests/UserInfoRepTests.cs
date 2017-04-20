using Xunit;
using OSS.Core.DapperRep.Members;
using OSS.Core.DomainEnts.Members.Ents;

namespace OSS.Core.RepTests
{
    public class UserInfoRepTests
    {
        [Fact]
        public void TestMethod1()
        {
            var rep = new UserInfoRep();
            rep.Test(new UserInfoEnt());
        }
    }
}
