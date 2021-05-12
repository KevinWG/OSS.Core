using System.Threading.Tasks;
using OSS.Tools.Log;

namespace OSS.Core.Services.Plugs.Log.IProxies
{
    public interface ILogServiceProxy 
    {
        Task WriteLog(LogInfo info);
    }
}
