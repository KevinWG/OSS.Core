using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal  class WebApiFilesTool: BaseProjectTool
{
    public override void Create(SolutionStructure solution)
    {
        base.Create(solution);
        Console.WriteLine($"应用协议层（WebApi）类库({solution.webapi_project.name}) -- done");
    }


    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.webapi_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "Swashbuckle.AspNetCore",
            "OSS.Core.Context.Attributes",
            "OSS.Core.Extension.Mvc.Configuration"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{ss.service_project.name}\\{ss.service_project.name}.csproj",
        };

        if (ss.solution_mode==SolutionMode.Default)
        {
            projectRefs.Add($"..\\{ss.rep_project.name}\\{ss.rep_project.name}.csproj");
        }

        var projectFilePath = Path.Combine(project.project_dir, project.name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, projectRefs,true);

        CreateProgramFile(ss);
    }

    public override void Create_GlobalFiles(SolutionStructure ss)
    {
        var project = ss.webapi_project;
        FileHelper.CreateDirectory(project.global_dir);

        var starterFilePath = Path.Combine(project.global_dir, project.starter_class_name + ".cs");
        FileHelper.CreateFileByTemplate(starterFilePath, ss, "WebApi/GlobalStarter.txt");
    }
    
    private static void CreateProgramFile(SolutionStructure ss)
    {
        var project = ss.webapi_project;
        var templateContent = FileHelper.LoadTemplateContent(ss,"WebApi/Program.txt");

        var repRegisterStr = ss.solution_mode == SolutionMode.Simple
            ? string.Empty
            : $"builder.Services.Register<{ss.rep_project.starter_class_name}>();       // 仓储层启动注入";

        var newContent = templateContent.Replace("{RepStarterRegister}", repRegisterStr);

        var programFilePath = Path.Combine(project.project_dir, "Program.cs");
        FileHelper.CreateFile(programFilePath, newContent);
    }

}