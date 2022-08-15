using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;
internal  class RepFilesTool : BaseProjectTool
{
    public override void Create(SolutionStructure solution)
    {
        if (solution.solution_mode == SolutionMode.Simple)
        {
            Create_CommonFiles(solution);
            return;
        }

        base.Create(solution);
        Console.WriteLine($"仓储层类库 ({solution.rep_project.name}) -- done");
    }

    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.rep_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Rep.Dapper.Mysql", "OSS.Core.Extension.Cache","OSS.Tools.Log"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_project.name}\\{ss.domain_project.name}.csproj"
        };
        
        CreateProjectFile(project.project_file_path, packageRefs, projectRefs);
    }

    public override void Create_CommonFiles(SolutionStructure ss)
    {
        var project = ss.rep_project;
        var baseRepDir = ss.solution_mode == SolutionMode.Default
            ? project.common_dir
            : ss.domain_project.common_dir;

        FileHelper.CreateDirectory(baseRepDir);

        var baeRepFilePath = Path.Combine(baseRepDir, $"{project.base_class_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, "Repository/BaseRep.txt");
    }

    public override void Create_GlobalFiles(SolutionStructure ss)
    {
        var project = ss.rep_project;
        FileHelper.CreateDirectory(project.global_dir);

        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        FileHelper.CreateFileByTemplate(starterFilePath, ss, "Repository/RepAppStarter.txt");
    }


    #region 添加实体

    public override void AddEntity(SolutionStructure ss)
    {
        AddEntity_Rep(ss);
        AddEntity_ChangeStarter(ss);
        Console.WriteLine("仓储层实体 -- done");
    }

    private static void AddEntity_Rep(SolutionStructure ss)
    {
        var repDir = ss.solution_mode == SolutionMode.Default
            ? Path.Combine(ss.rep_project.project_dir, ss.entity_name)
            : Path.Combine(ss.domain_project.entity_dir, "Rep"); // 简化模式，放置在Domain文件夹

        FileHelper.CreateDirectory(repDir);

        var repFilePath = Path.Combine(repDir, $"{ss.entity_name}Rep.cs");
        FileHelper.CreateFileByTemplate(repFilePath, ss, "Repository/EntityRep.txt", new Dictionary<string, string>()
        {
            {
                "{rep_interface}", ss.solution_mode == SolutionMode.Default ? $",I{ss.entity_name}Rep" : string.Empty
            }
        });
    }

    private static void AddEntity_ChangeStarter(SolutionStructure ss)
    {
        if (ss.solution_mode != SolutionMode.Default) 
            return;

        var project         = ss.rep_project;
        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        
        var injectRepStr = $"\r\n        InsContainer<I{ss.entity_name}Rep>.Set<{ss.entity_name}Rep>();";
        
        FileHelper.InsertFileFuncContent(starterFilePath, injectRepStr, "Start(IServiceCollection");
    }

    #endregion
}