using System.Collections.Generic;

namespace OSS.Core.NetCli;

internal class DomainOpenedFilesTool : BaseTool
{
    public override void Create(SolutionStructure pFiles)
    {
        CreateDomainOpenedFiles(pFiles);
    }
    
    private static void CreateDomainOpenedFiles(SolutionStructure ss)
    {
        var packageRefs = new List<string>()
        {
            "OSS.Core.Domain.Opened"
        };

        FileHelper.CreateProjectFile(ss.domain_opened_project.project_file_path, packageRefs, null);
    }
}