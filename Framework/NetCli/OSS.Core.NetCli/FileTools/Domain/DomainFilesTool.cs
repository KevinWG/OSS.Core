using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal class DomainFilesTool : BaseProjectTool
{
    #region 创建

    public override void Create(SolutionStructure solution)
    {
        base.Create(solution);
        Console.WriteLine($"领域层类库({solution.domain_project.name}) -- done");
    }

    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.domain_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Domain"
        };

        if (ss.solution_mode == SolutionMode.Simple)
        {
            // 简单模式下 仓储和领域实体放在一起
            packageRefs.AddRange(new[] { "OSS.Core.Rep.Dapper.Mysql", "OSS.Core.Extension.Cache", "OSS.Tools.Log" });
        }

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_opened_project.name}\\{ss.domain_opened_project.name}.csproj"
        };

        FileHelper.CreateProjectFile(project.project_file_path, packageRefs, projectRefs);
    }



    public override void Create_CommonFiles(SolutionStructure ss)
    {
        var project = ss.domain_project;
        FileHelper.CreateDirectory(project.common_dir);

        var baeRepFilePath = Path.Combine(project.common_dir, $"{project.const_file_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, "Domain/ModuleConst.txt");
    }

    public override void Create_GlobalFiles(SolutionStructure solution)
    {
        var project = solution.domain_project;
        FileHelper.CreateDirectory(project.global_dir);

        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        FileHelper.CreateFileByTemplate(starterFilePath, solution, "Domain/DomainAppStarter.txt");
    }


    #endregion


    #region 添加实体

    public override void AddEntity(SolutionStructure ss)
    {
        FileHelper.CreateDirectory(ss.domain_project.entity_dir);

        AddEntityIRep(ss);
    }

    private static void AddEntityIRep(SolutionStructure ss)
    {
        if (ss.solution_mode == SolutionMode.Simple)
        {
            return;
        }

        var irepDir = Path.Combine(ss.domain_project.entity_dir, "IRep");
        FileHelper.CreateDirectory(irepDir);

        var irepFilePath = Path.Combine(irepDir, $"I{ss.entity_name}Rep.cs");
        FileHelper.CreateFileByTemplate(irepFilePath, ss, "Domain/IRep/IEntityRep.txt");
    }



    #endregion
}