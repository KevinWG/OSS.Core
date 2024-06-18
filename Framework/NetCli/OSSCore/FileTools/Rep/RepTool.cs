using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal class RepTool : BaseProjectTool
{
    public override void Create(Solution solution)
    {
        base.Create(solution);
        Console.WriteLine($"仓储层类库 ({solution.rep_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        if (ss.mode == SolutionMode.Simple_Plus)
            return;

        var project = ss.rep_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Rep.Dapper.Mysql", "OSS.Core.Extension.Cache", "OSS.Tools.Config"
        };

        var domainProject = ss.mode == SolutionMode.Simple ? ss.domain_open_project.name : ss.domain_project.name;
        var projectRefs = new List<string>()
        {
            $"..{(ss.mode == SolutionMode.Simple ? "\\Open" : string.Empty)}\\{domainProject}\\{domainProject}.csproj"
        };
        CreateProjectFile(project.project_file_path, packageRefs, projectRefs);
    }

    public override void Create_CommonFiles(Solution ss)
    {
        var project    = ss.rep_project;
        var baseRepDir = project.common_dir;

        FileHelper.CreateDirectory(baseRepDir);

        var baeRepFilePath = Path.Combine(baseRepDir, $"{project.base_class_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, "Repository/BaseRep.txt");
    }

    public override void Create_GlobalFiles(Solution ss)
    {
        if (ss.mode == SolutionMode.Simple_Plus)
            return;

        var project = ss.rep_project;
        FileHelper.CreateDirectory(project.global_dir);

        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        FileHelper.CreateFileByTemplate(starterFilePath, ss, "Repository/RepAppStarter.txt");
    }


    #region 添加实体

    public override void AddEntity(Solution ss)
    {
        AddEntity_Rep(ss);
        AddEntity_ChangeStarter(ss);
        Console.WriteLine("仓储层实体 -- done");
    }

    private static void AddEntity_Rep(Solution ss)
    {
        var repDir = Path.Combine(ss.rep_project.project_dir, ss.entity_name);
        if (ss.mode == SolutionMode.Simple_Plus)
        {
            repDir = Path.Combine(repDir, "Rep");
        }

        FileHelper.CreateDirectory(repDir);

        var repFilePath = Path.Combine(repDir, $"{ss.entity_name}Rep.cs");
        FileHelper.CreateFileByTemplate(repFilePath, ss, "Repository/EntityRep.txt");
    }

    // 修改仓储启动注册文件,注入仓储接口实现
    private static void AddEntity_ChangeStarter(Solution ss)
    {
        if (ss.mode != SolutionMode.Default) //    仅全模式下才会有数据层接口注入
            return;

        var project         = ss.rep_project;
        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");

        var injectRepStr = $"\r\n        InsContainer<I{ss.entity_name}Rep>.Set<{ss.entity_name}Rep>();";

        FileHelper.InsertFileFuncContent(starterFilePath, injectRepStr, "Start(IServiceCollection");
    }

    #endregion
}