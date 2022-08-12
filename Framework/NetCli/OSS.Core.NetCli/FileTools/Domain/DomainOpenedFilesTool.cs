using System.Collections.Generic;

namespace OSS.Core.NetCli;

internal class DomainOpenedFilesTool : BaseProjectTool
{

    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.domain_opened_project;
        FileHelper.CreateDirectory(project.project_dir);
        
        var packageRefs = new List<string>()
        {
            "OSS.Core.Domain.Opened"
        };

        FileHelper.CreateProjectFile(project.project_file_path, packageRefs, null);
    }
}