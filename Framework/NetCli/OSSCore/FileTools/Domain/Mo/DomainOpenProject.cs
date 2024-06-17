using System.IO;

namespace OSSCore;

internal class DomainOpenProject : BaseProjectStructure
{
    public DomainOpenProject(string solutionName, string moduleName,
                             string basePath, string entityName = ""
        ) 
        : base(
        string.Concat(solutionName, ".Domain.Open")
        , Path.Combine(basePath, "Open")
        , entityName)
    {
    }
}