namespace OSS.Core.NetCli;
internal class ServiceProjectStructure : BaseProjectStructure
{
    public ServiceProjectStructure(string solutionName, string moduleName, string basePath) : base(
        string.Concat(solutionName, ".Service"), basePath)
    {
        starter_file_name = string.Concat(moduleName, "ServiceStarter");
        local_client_name = string.Concat("Local", moduleName, "Client");
    }
    
    public string starter_file_name { get; }

    public string local_client_name { get; }
}