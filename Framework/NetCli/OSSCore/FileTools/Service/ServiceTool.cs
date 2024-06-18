using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal class ServiceTool : BaseProjectTool
{
    public override void Create(Solution solution)
    {
        base.Create(solution);
        Console.WriteLine($"服务层类库 ({solution.service_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        if (ss.mode == SolutionMode.Simple_Plus)
            return;

        var project = ss.service_project;
        FileHelper.CreateDirectory(project.project_dir);
        
        var packageRefs = new List<string>()
        {
            "OSS.DataFlow",
            "OSS.Core.Extension.Cache",
            "OSS.Core.Extension.PassToken"
        };

        var projectRefs = new List<string>
        {
            ss.mode == SolutionMode.Simple
                ? $"..\\{ss.rep_project.name}\\{ss.rep_project.name}.csproj"
                : $"..\\{ss.domain_project.name}\\{ss.domain_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(project.project_dir, project.name + ".csproj");
        CreateProjectFile(projectFilePath, packageRefs, projectRefs);
    }

    public override void Create_GlobalFiles(Solution ss)
    {
        if (ss.mode == SolutionMode.Simple_Plus)
            return;

        var project = ss.service_project;
        FileHelper.CreateDirectory(project.global_dir);

        var baeStarterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        FileHelper.CreateFileByTemplate(baeStarterFilePath, ss, "Service/ServiceAppStarter.txt");
    }
    
    #region 添加实体

    public override void AddEntity(Solution ss)
    {
        var path = ss.service_project.entity_dir;
        if (ss.mode == SolutionMode.Simple_Plus)
        {
            path = Path.Combine(path, "Service");
        }

        FileHelper.CreateDirectory(path);

        var oServiceFilePath = Path.Combine(path, $"{ss.entity_name}Service.cs");
        FileHelper.CreateFileByTemplate(oServiceFilePath, ss, "Service/EntityService.txt");
        
        Console.WriteLine("服务层实体 -- done");
    }

    #endregion
}