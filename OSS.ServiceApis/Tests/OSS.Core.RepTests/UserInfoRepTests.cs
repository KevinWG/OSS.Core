using OSS.Common.Authrization;
using OSS.Core.Domains.Members;
using Xunit;
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
            info.AppClient = AppClientType.Window;
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

            var res = rep.Add(mo);
            mo.id = res.Id;
            mo.email = "222222@qq.com";
            mo.mobile = "222222222222";

        }
    }
}
