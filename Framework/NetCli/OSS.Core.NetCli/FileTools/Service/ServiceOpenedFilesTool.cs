using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal  class ServiceOpenedFilesTool : BaseProjectTool
{
    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.service_opened_project;
        FileHelper.CreateDirectory(project.project_dir);

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_opened_project.name}\\{ss.domain_opened_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(project.project_dir, project.name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, null, projectRefs);
    }


    public override void Create_CommonFiles(SolutionStructure ss)
    {
        var project = ss.service_opened_project;
        FileHelper.CreateDirectory(project.common_dir);

        var baeClientFilePath = Path.Combine(project.common_dir, $"{project.client_interface_name}.cs");
        FileHelper.CreateFileByTemplate(baeClientFilePath, ss, "Service/IModuleClient.txt");
    }
}