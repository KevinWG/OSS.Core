namespace OSSCore;

internal class DomainOpenedProjectStructure : BaseProjectStructure
{
    public DomainOpenedProjectStructure(string solutionName, string moduleName, string basePath, string entityName = "") : base(
        string.Concat(solutionName, ".Domain.Opened"), basePath,entityName)
    {
    }
}