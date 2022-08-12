namespace OSS.Core.NetCli;
internal class WebApiProjectStructure: BaseProjectStructure
{
    public WebApiProjectStructure(string solutionName, string moduleName, string basePath) : base(
        string.Concat(solutionName, ".WebApi"), basePath)
    {
        used_client_file = string.Concat(moduleName, "UsedClientStarter");
    }

    public string used_client_file { get; }
}