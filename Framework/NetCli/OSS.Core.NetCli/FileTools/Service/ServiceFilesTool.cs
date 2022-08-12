using System.Collections.Generic;
using System.IO;

namespace OSS.Core.NetCli;

internal  class ServiceFilesTool:BaseTool
{
    public override void Create(SolutionStructure pFiles)
    {
        CreateServiceFiles(pFiles);
    }
    

    private static void CreateServiceFiles(SolutionStructure pFiles)
    {
        var projectPath = Path.Combine(pFiles.base_path, pFiles.service_project.name);
        FileHelper.CreateDirectory(projectPath);
        
        var packageRefs = new List<string>()
        {
            "OSS.DataFlow","OSS.Core.Extension.Cache","OSS.Tools.Log"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{pFiles.domain_project.name}\\{pFiles.domain_project.name}.csproj",
            $"..\\{pFiles.service_opened_project.name}\\{pFiles.service_opened_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(projectPath, pFiles.service_project.name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, projectRefs);
    }
}