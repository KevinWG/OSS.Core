using OSS.Common.Authrization;
using Xunit;
using OSS.Core.DomainMos.Members.Mos;
using OSS.Core.RepDapper.Members;

namespace OSS.Core.RepTests
{

    public class UserInfoRepTests
    {

        [Fact]
        public void TikectTest()
        {
            var info = new SysAuthorizeInfo();

            info.AppSource = "FrontWeb";
            info.AppClient = "PC";
            info.AppVersion = "1.0";
            info.DeviceId = "Test Device";

            info.WebBrowser = "Chrome";

            var ticket = info.ToSignData("5c567449b8714a038c464059788d4fa6");
            //  appclient=PC;appsource=FrontWeb;appversion=1.0;deviceid=Test%20Device;timespan=1495277505;webbrowser=Chrome;sign=tBL1yvayljCTiBvO7u3As%2F3RLoc%3D
        }

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
