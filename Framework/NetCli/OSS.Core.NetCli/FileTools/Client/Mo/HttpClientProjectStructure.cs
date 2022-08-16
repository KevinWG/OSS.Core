
using System;

namespace OSSCore;
internal class HttpClientProject : BaseProjectStructure
{
    public HttpClientProject(string solutionName,string moduleName,string basePath, string entityName = "") : base(
        string.Concat(solutionName, ".Client.Http"), basePath, entityName)
    {
        module_req_name    = string.Concat(moduleName, "Req");
        module_client_name = string.Concat("Http", moduleName, "Client");
    }

    public string module_req_name { get;  }


    public string module_client_name { get; }

}