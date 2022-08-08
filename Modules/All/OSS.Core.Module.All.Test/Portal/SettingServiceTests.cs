using OSS.Core.Module.Portal;

namespace OSS.Core.Module.All.Test.Portal;

[TestClass]
public class SettingServiceTests : BaseTest
{
    private static readonly SettingService _service = new();

    //[TestMethod]
    //public async Task SettingTest()
    //{
    //    var settingRes = await _service.GetAuthSetting();
    //    Assert.IsTrue(settingRes.IsSuccessOrDataNull());

    //    var setting = settingRes.data ?? new AuthSetting();

    //    var updateRes = await _service.SaveAuthSetting(setting);
    //    Assert.IsTrue(updateRes.IsSuccess());
    //}
}