namespace OSS.Core.NetCli;
internal class ServiceOpenedProjectStructure : BaseProjectStructure
{
    public ServiceOpenedProjectStructure(string solutionName, string moduleName, string basePath) : base(
        string.Concat(solutionName, ".Service.Opened"), basePath)
    {
        client_interface_name = string.Concat("I", moduleName, "Client");
    }

    public string client_interface_name { get; }
}