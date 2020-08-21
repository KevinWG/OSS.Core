using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Services.Plugs.Log;
using OSS.Tools.Log;
using Xunit;

namespace OSS.Core.Tests.Plugs.Log
{
    public class LogServiceTests : BaseTests
    {
        private static LogService _service = new LogService();

        [Fact]
        public async Task WriteLogTest()
        {
            await _service.WriteLog(new LogInfo()
            {
                msg_body = "测试日志",
                source_name = "default"
            });
        }

    }
}
