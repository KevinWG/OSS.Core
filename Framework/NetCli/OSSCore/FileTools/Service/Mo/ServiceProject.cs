namespace OSSCore;
internal class ServiceProject : BaseProjectStructure
{
    public ServiceProject(string solutionName, string moduleName, string basePath, string entityName = "") : base(
        string.Concat(solutionName, ".Service"), basePath,entityName)
    {
        starter_class_name = string.Concat(moduleName, "ServiceStarter");
        local_client_name = string.Concat("Local", moduleName, "Client");
    }
    
    public string starter_class_name { get; }

    public string local_client_name { get; }
}