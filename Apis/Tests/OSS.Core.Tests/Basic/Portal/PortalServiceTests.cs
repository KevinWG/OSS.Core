//using OSS.Common.ComModels;
//using Vantel.RepDapper.Basic.Users;
//using Vantel.Services.Basic.Portal;
//using Vantel.Services.Basic.Portal.Mos;
//using OSS.Core.Infrastructure.Enums.User;
//using Xunit;

//namespace OSS.ProCore.RepTests.UserTests
//{
//    public class PortalServiceTests : BaseTests
//    {
//        private static readonly PortalService _portalService = new PortalService();

//        [Fact]
//        public void EmailRegTest()
//        {
//            var strEmail = "1986088337@qq.com";
//            var req = new PortalPasswordReq()
//            {
//                name = strEmail,
//                password = "111111",
//                type = RegLoginType.Email
//            };

//            var userRes = _portalService.PwdLogin(req).Result;
//            if (userRes.IsSuccess())
//            {
//                var re = UserInfoRep.Instance.SoftDeleteById(userRes.Detail.Id).Result;
//            }

//            var checkRes = _portalService.CheckIfCanReg(RegLoginType.Email, strEmail).Result;
//            Assert.True(checkRes.IsSuccess(), "单元测试的邮箱已被占用");

//            var regUserRes = _portalService.PwdReg(req).Result;
//            Assert.True(regUserRes.IsSuccess(), "邮箱注册失败");

//            var loginRes = _portalService.PwdLogin(req).Result;
//            Assert.True(loginRes.IsSuccess(), "登录测试失败！");
//        }

//        [Fact]
//        public void MobileRegTest()
//        {
//            var strMobile = "11111111111";
//            var req = new PortalPasswordReq()
//            {
//                name = strMobile,
//                password = "111111",
//                type = RegLoginType.Mobile
//            };

//            var userRes = _portalService.PwdLogin(req).Result;
//            if (userRes.IsSuccess())
//            {
//                var re = UserInfoRep.Instance.SoftDeleteById(userRes.Detail.Id).Result;
//            }

//            var regUserRes = _portalService.PwdReg(req).Result;
//            Assert.True(regUserRes.IsSuccess(), "手机注册失败");

//            var loginRes = _portalService.PwdLogin(req).Result;
//            Assert.True(loginRes.IsSuccess(), "手机登录测试失败！");
//        }
//    }
//}
