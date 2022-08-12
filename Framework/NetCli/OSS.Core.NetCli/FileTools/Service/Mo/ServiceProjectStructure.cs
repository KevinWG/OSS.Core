namespace OSS.Core.NetCli;
internal class ServiceProjectStructure : BaseProjectStructure
{
    public ServiceProjectStructure(string solutionName, string moduleName, string basePath) : base(
        string.Concat(solutionName, ".Service"), basePath)
    {
    }
}