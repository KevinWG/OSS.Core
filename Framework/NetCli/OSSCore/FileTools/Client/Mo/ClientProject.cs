namespace OSSCore;
public class ClientProject : BaseProjectStructure
{
    public ClientProject(string solutionName,string moduleName,string basePath, string entityName = "") 
        : base(string.Concat(solutionName, ".Client.Http")
        , Path.Combine(basePath, "Open"), entityName)
    {
        if (!string.IsNullOrEmpty(entityName))
        {
            entity_dir = Path.Combine(project_dir, "Clients");
        }

        module_req_name    = string.Concat(moduleName, "RemoteReq");
        module_client_name = string.Concat(moduleName, "RemoteClient");
    }

    /// <summary>
    ///  模块请求名称
    /// </summary>
    public string module_req_name { get;  }
    
    /// <summary>
    /// 模块客户端请求全局静态类
    /// </summary>
    public string module_client_name { get; } 

}