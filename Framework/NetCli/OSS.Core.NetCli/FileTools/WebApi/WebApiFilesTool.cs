using System.Collections.Generic;
using System.IO;

namespace OSS.Core.NetCli;

internal  class WebApiFilesTool: BaseProjectTool
{
    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.webapi_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "Swashbuckle.AspNetCore"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{ss.service_project.name}\\{ss.service_project.name}.csproj",
            $"..\\{ss.rep_project.name}\\{ss.rep_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(project.project_dir, project.name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, projectRefs,true);
    }
    
    public override void Create_GlobalFiles(SolutionStructure ss)
    {
        var project = ss.webapi_project;
        FileHelper.CreateDirectory(project.global_dir);

        var programFilePath = Path.Combine(project.project_dir, "Program.cs");
        FileHelper.CreateFileByTemplate(programFilePath,ss, "WebApi/Program.txt");

        var usedClientFilePath = Path.Combine(project.global_dir, project.used_client_file+ ".cs");
        FileHelper.CreateFileByTemplate(usedClientFilePath, ss, "WebApi/UsedClientStarter.txt");
    }
}