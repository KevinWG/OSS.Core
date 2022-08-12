using System.Collections.Generic;
using System.IO;

namespace OSS.Core.NetCli;
internal  class WebApiFilesTool: BaseTool
{
    public override void Create(SolutionStructure pFiles)
    {
        var projectPath = Path.Combine(pFiles.base_path, pFiles.webapi_project.name);
        FileHelper.CreateDirectory(projectPath);

        var packageRefs = new List<string>()
        {
            "Swashbuckle.AspNetCore"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{pFiles.service_project.name}\\{pFiles.service_project.name}.csproj",
            $"..\\{pFiles.rep_project.name}\\{pFiles.rep_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(projectPath, pFiles.webapi_project.name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, projectRefs,true);
    }
}