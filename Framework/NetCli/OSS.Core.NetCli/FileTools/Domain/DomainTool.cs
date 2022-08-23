using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal class DomainTool : BaseProjectTool
{
    #region 创建

    public override void Create(Solution solution)
    {
        base.Create(solution);
        Console.WriteLine($"领域层类库({solution.domain_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        var project = ss.domain_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        { 
            "OSS.Tools.Log",
            "OSS.Core.Domain"
        };

        if (ss.no_rep_injection)
        {
            // 简单模式下 仓储和领域实体放在一起
            packageRefs.AddRange(new[] { "OSS.Core.Rep.Dapper.Mysql", "OSS.Core.Extension.Cache" });
        }

        var projectRefs = new List<string>()
        {
            $"..\\Open\\{ss.domain_open_project.name}\\{ss.domain_open_project.name}.csproj"
        };

        CreateProjectFile(project.project_file_path, packageRefs, projectRefs);
    }



    public override void Create_CommonFiles(Solution ss)
    {
        var project = ss.domain_project;
        FileHelper.CreateDirectory(project.common_dir);

        var baeRepFilePath = Path.Combine(project.common_dir, $"{project.const_file_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, "Domain/ModuleConst.txt");
    }

    public override void Create_GlobalFiles(Solution solution)
    {
        var project = solution.domain_project;
        FileHelper.CreateDirectory(project.global_dir);

        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        FileHelper.CreateFileByTemplate(starterFilePath, solution, "Domain/DomainAppStarter.txt");
    }


    #endregion

    #region 添加实体

    public override void AddEntity(Solution ss)
    {
        FileHelper.CreateDirectory(ss.domain_project.entity_dir);

        AddEntityIRep(ss);

        Console.WriteLine("领域层实体 -- done");
    }

    private static void AddEntityIRep(Solution ss)
    {
        if (ss.no_rep_injection)
            return;

        var iRepDir = Path.Combine(ss.domain_project.entity_dir, "IRep");
        FileHelper.CreateDirectory(iRepDir);

        var iRepFilePath = Path.Combine(iRepDir, $"I{ss.entity_name}Rep.cs");
        FileHelper.CreateFileByTemplate(iRepFilePath, ss, "Domain/IRep/IEntityRep.txt");
    }



    #endregion
}