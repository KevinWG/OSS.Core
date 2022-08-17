namespace OSSCore;

internal class DomainOpenedProject : BaseProjectStructure
{
    public DomainOpenedProject(string solutionName, string moduleName, string basePath, string entityName = "") : base(
        string.Concat(solutionName, ".Domain.Opened"), basePath,entityName)
    {
    }
}