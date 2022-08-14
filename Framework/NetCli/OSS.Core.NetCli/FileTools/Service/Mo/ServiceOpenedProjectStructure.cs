namespace OSSCore;
internal class ServiceOpenedProjectStructure : BaseProjectStructure
{
    public ServiceOpenedProjectStructure(string solutionName, string moduleName, string basePath, string entityName = "") : base(
        string.Concat(solutionName, ".Service.Opened"), basePath,entityName)
    {
        client_interface_name = string.Concat("I", moduleName, "Client");
    }

    public string client_interface_name { get; }
}