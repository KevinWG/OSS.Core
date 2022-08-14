namespace OSSCore;
internal class WebApiProjectStructure: BaseProjectStructure
{
    public WebApiProjectStructure(string solutionName, string moduleName, string basePath) : base(
        string.Concat(solutionName, ".WebApi"), basePath)
    {
        starter_file_name = string.Concat(moduleName, "GlobalStarter");
    }

    public string starter_file_name { get; }
}