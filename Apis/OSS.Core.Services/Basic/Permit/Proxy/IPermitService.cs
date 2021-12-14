using OSS.Common.Resp;
using System.Threading.Tasks;
using OSS.Core.Context;

namespace OSS.Core.Services.Basic.Permit.Proxy
{
    public interface IPermitService
    {
        Task<Resp<FuncDataLevel>> CheckIfHaveFunc(string funcCode,string queryCode);
    }
}
