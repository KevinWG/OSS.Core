using System.IO;

namespace OSSCore;

internal class ServiceOpenedProject : BaseProjectStructure
{
    public ServiceOpenedProject(string solutionName, string moduleName,
                                string basePath, string entityName = "")
        : base(
            string.Concat(solutionName, ".Domain.Open")
            , Path.Combine(basePath, "Open"), entityName)
    {
    }

}