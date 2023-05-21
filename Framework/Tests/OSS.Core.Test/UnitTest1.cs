using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var secret = "0123456789012345";

            var oldIdentity = new AppIdentity() { };
            var ticket = oldIdentity.ToTicket("AccessKey", secret, "1.0", "token");

            var newIdentity= new AppIdentity();
            newIdentity.FormatFromTicket(ticket);

            Assert.IsTrue(newIdentity.CheckSign(secret,10, "token").IsSuccess(),"应用签名加密失败!");

            var userIdentity = new UserIdentity
            {
                id_type = IdentityType.Admin
            };
            Assert.IsTrue(userIdentity.auth_type == AuthorizeType.Admin, "扩展 IdentityType 属性失败!");
        }
    }
}