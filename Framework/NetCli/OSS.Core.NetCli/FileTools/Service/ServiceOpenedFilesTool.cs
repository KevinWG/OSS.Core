using System.Collections.Generic;
using System.IO;

namespace OSS.Core.NetCli;

internal  class ServiceOpenedFilesTool : BaseTool
{
    public override void Create(SolutionStructure pFiles)
    {
        CreateServiceOpenedFiles(pFiles);
    }

    private static void CreateServiceOpenedFiles(SolutionStructure pFiles)
    {
        var projectPath = Path.Combine(pFiles.base_path, pFiles.service_opened_project.name);
        FileHelper.CreateDirectory(projectPath);
        
        var projectRefs = new List<string>()
        {
            $"..\\{pFiles.domain_opened_project.name}\\{pFiles.domain_opened_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(projectPath, pFiles.service_opened_project.name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, null, projectRefs);
    }
}