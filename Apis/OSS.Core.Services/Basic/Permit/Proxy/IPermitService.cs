using OSS.Common.BasicMos.Resp;
using System.Threading.Tasks;

namespace OSS.Core.Services.Basic.Permit.Proxy
{
    public interface IPermitService
    {
        Task<Resp> CheckIfHaveFunc(string funcCode);
    }
}
