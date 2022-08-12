using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal class ServiceFilesTool : BaseProjectTool
{
    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.service_project;
        FileHelper.CreateDirectory(project.project_dir);
        
        var packageRefs = new List<string>()
        {
            "OSS.DataFlow",
            "OSS.Tools.Log",
            "OSS.Core.Extension.Cache",
            "OSS.Core.Extension.PassToken"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_project.name}\\{ss.domain_project.name}.csproj",
            $"..\\{ss.service_opened_project.name}\\{ss.service_opened_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(project.project_dir, project.name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, projectRefs);
    }

    public override void Create_GlobalFiles(SolutionStructure ss)
    {
        var project = ss.service_project;
        FileHelper.CreateDirectory(project.global_dir);

        var baeStarterFilePath = Path.Combine(project.global_dir, $"{project.starter_file_name}.cs");
        FileHelper.CreateFileByTemplate(baeStarterFilePath, ss, "Service/ServiceAppStarter.txt");

        var localClientPath = Path.Combine(project.global_dir, $"{project.local_client_name}.cs");
        FileHelper.CreateFileByTemplate(localClientPath, ss, "Service/LocalModuleClient.txt");
    }
}