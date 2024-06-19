namespace OSSCore;
internal class WebApiProject: BaseProjectStructure
{
    public WebApiProject(string solutionName, string moduleName, string basePath, string entityName = "") : base(
        string.Concat(solutionName, ".WebApi"), basePath,entityName)
    {
        starter_class_name = string.Concat(moduleName, "GlobalStarter");
        nonce_secret = Guid.NewGuid().ToString().Replace("-", "");
    }


    public string starter_class_name { get; }


    public string nonce_secret { get;  }
}