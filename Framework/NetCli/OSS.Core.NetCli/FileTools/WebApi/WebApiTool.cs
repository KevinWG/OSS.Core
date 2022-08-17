using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal  class WebApiTool: BaseProjectTool
{
    #region 创建

    public override void Create(Solution solution)
    {
        base.Create(solution);
        Console.WriteLine($"应用协议层（WebApi）类库({solution.webapi_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
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

        if (ss.solution_mode == SolutionMode.Default)
        {
            projectRefs.Add($"..\\{ss.rep_project.name}\\{ss.rep_project.name}.csproj");
        }

        var projectFilePath = Path.Combine(project.project_dir, project.name + ".csproj");
        CreateProjectFile(projectFilePath, packageRefs, projectRefs, true);
        
        CreateProgramFile(ss);
        Create_AppSettingFile(ss);
    }

    public override void Create_CommonFiles(Solution ss)
    {
        FileHelper.CreateDirectory(ss.webapi_project.common_dir);

        var baseFilePath = Path.Combine(ss.webapi_project.common_dir, $"Base{ss.module_name}Controller.cs");
        FileHelper.CreateFileByTemplate(baseFilePath, ss, "WebApi/BaseModuleController.txt");
    }

    public override void Create_GlobalFiles(Solution ss)
    {
        var project = ss.webapi_project;
        FileHelper.CreateDirectory(project.global_dir);

        var starterFilePath = Path.Combine(project.global_dir, project.starter_class_name + ".cs");
        FileHelper.CreateFileByTemplate(starterFilePath, ss, "WebApi/GlobalStarter.txt");

        var authProPath = Path.Combine(project.global_dir, "AuthProvider.cs");
        FileHelper.CreateFileByTemplate(authProPath, ss, "WebApi/AuthProvider.txt");
    }

    private static void CreateProgramFile(Solution ss)
    {
        var project = ss.webapi_project;

        var repRegisterStr = ss.solution_mode == SolutionMode.Simple
            ? string.Empty
            : $"builder.Services.Register<{ss.rep_project.starter_class_name}>();       // 仓储层启动注入";

        var extDic = new Dictionary<string,string> { { "{RepStarterRegister}", repRegisterStr } };
        
        var programFilePath = Path.Combine(project.project_dir, "Program.cs");
        FileHelper.CreateFileByTemplate(programFilePath, ss, "WebApi/Program.txt", extDic);
    }


    private static void Create_AppSettingFile(Solution ss)
    {
        var project = ss.webapi_project;
        
        var appSettingPath = Path.Combine(project.project_dir, "appsettings.json");
        FileHelper.CreateFileByTemplate(appSettingPath, ss, "WebApi/AppSetting.txt");
    }

    #endregion


    #region 添加实体

    public override void AddEntity(Solution ss)
    {
       var controllerDir =  Path.Combine(ss.webapi_project.project_dir, "Controller");
        FileHelper.CreateDirectory(controllerDir);

        var entityControllerFilePath = Path.Combine(controllerDir, $"{ss.entity_name}Controller.cs");
        FileHelper.CreateFileByTemplate(entityControllerFilePath, ss,"WebApi/EntityController.txt");

        Console.WriteLine("应用接口层实体 -- done");
    }

    #endregion
}