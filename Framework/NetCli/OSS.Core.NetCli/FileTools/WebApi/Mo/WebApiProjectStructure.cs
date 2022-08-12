namespace OSS.Core.NetCli;
internal class WebApiProjectStructure: BaseProjectStructure
{
    public WebApiProjectStructure(string solutionName, string moduleName, string basePath) : base(
        string.Concat(solutionName, ".WebApi"), basePath)
    {
    }
}