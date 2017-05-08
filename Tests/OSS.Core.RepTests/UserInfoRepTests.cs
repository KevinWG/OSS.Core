using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
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

            var configBuilder =
               new ConfigurationBuilder().Add(new JsonConfigurationSource()
               {
                   Path = "appsettings.json",
                   ReloadOnChange = true
               });
            var m_Config = configBuilder.Build();

            var str = m_Config.GetConnectionString("WriteConnection");
            //var rep = new UserInfoRep();
            //rep.Insert(new UserInfoMo(){nick_name = "再次测试"});
        }
    }
}
