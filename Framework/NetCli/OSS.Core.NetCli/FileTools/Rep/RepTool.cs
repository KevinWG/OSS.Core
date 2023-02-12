using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;
internal  class RepTool : BaseProjectTool
{
    public override void Create(Solution solution)
    {
        Create_CommonFiles(solution);
        
        base.Create(solution);
        Console.WriteLine($"仓储层类库 ({solution.rep_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        var project = ss.rep_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Rep.Dapper.Mysql", "OSS.Core.Extension.Cache"
        };

        var domainProject = ss.no_rep_injection ? ss.domain_open_project.name : ss.domain_project.name;
        var projectRefs = new List<string>()
        {
            $"..{(ss.no_rep_injection?"\\Open":string.Empty)}\\{domainProject}\\{domainProject}.csproj"
        };
        
        CreateProjectFile(project.project_file_path, packageRefs, projectRefs);
    }

    public override void Create_CommonFiles(Solution ss)
    {
        var project = ss.rep_project;
        var baseRepDir = ss.no_rep_injection
            ? ss.domain_project.common_dir : project.common_dir;

        FileHelper.CreateDirectory(baseRepDir);

        var baeRepFilePath = Path.Combine(baseRepDir, $"{project.base_class_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, "Repository/BaseRep.txt");
    }

    public override void Create_GlobalFiles(Solution ss)
    {
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
        var repDir =  Path.Combine(ss.rep_project.project_dir, ss.entity_name); 

        FileHelper.CreateDirectory(repDir);

        var repFilePath = Path.Combine(repDir, $"{ss.entity_name}Rep.cs");
        FileHelper.CreateFileByTemplate(repFilePath, ss, "Repository/EntityRep.txt", new Dictionary<string, string>()
        {
            {
                "{rep_interface}", ss.no_rep_injection ?  string.Empty :$",I{ss.entity_name}Rep"
            }
        });
    }

    private static void AddEntity_ChangeStarter(Solution ss)
    {
        if (ss.no_rep_injection) 
            return;

        var project         = ss.rep_project;
        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        
        var injectRepStr = $"\r\n        InsContainer<I{ss.entity_name}Rep>.Set<{ss.entity_name}Rep>();";
        
        FileHelper.InsertFileFuncContent(starterFilePath, injectRepStr, "Start(IServiceCollection");
    }

    #endregion
}