namespace OSSCore;
internal class ServiceOpenedProject : BaseProjectStructure
{
    public ServiceOpenedProject(string solutionName, string moduleName, string basePath, string entityName = "") : base(
        string.Concat(solutionName, ".Service.Opened"), basePath,entityName)
    {
        //client_interface_name = string.Concat("I", moduleName, "Client");
    }
    
}