using System.Threading.Tasks;
using OSS.Common.Helpers;
using OSS.Core.Services.Plugs.Log.IProxies;
using OSS.Tools.Log;

namespace OSS.Core.Services.Sys_Global
{
    public class EmailLogProvider : IToolLog
    {
        public Task WriteLogAsync(LogInfo info)
        {
            return InsContainer<ILogServiceProxy>.Instance.WriteLog(info);
        }
    }
}
