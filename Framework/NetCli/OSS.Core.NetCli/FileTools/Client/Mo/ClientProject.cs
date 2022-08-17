

using System;

namespace OSSCore;
internal class ClientProject : BaseProjectStructure
{
    public ClientProject(string solutionName,string moduleName,string basePath, string entityName = "") : base(
        string.Concat(solutionName, ".Client.Http"), basePath, entityName)
    {
        module_req_name    = string.Concat(moduleName, "RemoteReq");
        module_client_name = string.Concat(moduleName, "RemoteClient");
    }

    public string module_req_name { get;  }

    public string module_client_name { get; }

}