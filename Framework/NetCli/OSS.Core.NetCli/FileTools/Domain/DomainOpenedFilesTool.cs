using System;
using System.Collections.Generic;

namespace OSSCore;

internal class DomainOpenedFilesTool : BaseProjectTool
{
    public override void Create(SolutionStructure solution)
    {
        base.Create(solution);
        Console.WriteLine($"领域层类库（共享）({solution.domain_opened_project.name}) -- done");
    }

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