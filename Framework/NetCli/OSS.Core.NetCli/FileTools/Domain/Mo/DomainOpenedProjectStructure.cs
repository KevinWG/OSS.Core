


namespace OSS.Core.NetCli;
internal class DomainOpenedProjectStructure : BaseProjectStructure
{
    public DomainOpenedProjectStructure(string solutionName, string moduleName, string basePath) : base(
        string.Concat(solutionName, ".Domain.Opened"), basePath)
    {
    }
}