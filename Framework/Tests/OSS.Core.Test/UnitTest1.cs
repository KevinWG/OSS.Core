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

            Assert.IsTrue(newIdentity.CheckSign(secret,10, "token").IsSuccess(),"Ӧ��ǩ������ʧ��!");
        }
    }
}