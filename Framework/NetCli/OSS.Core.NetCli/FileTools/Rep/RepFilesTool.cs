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
        
        FileHelper.CreateProjectFile(project.project_file_path, packageRefs, projectRefs);
    }

    public override void Create_CommonFiles(SolutionStructure ss)
    {
        var project = ss.rep_project;
        FileHelper.CreateDirectory(project.common_dir);

        var baeRepFilePath = Path.Combine(project.common_dir, $"{project.base_class_name}.cs");
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
        var repDir = ss.solution_mode == SolutionMode.Simple
            ? Path.Combine(ss.domain_project.entity_dir, "Rep")
            : Path.Combine(ss.rep_project.project_dir, ss.entity_name);

        FileHelper.CreateDirectory(repDir);

        var repFilePath = Path.Combine(repDir, $"{ss.entity_name}Rep.cs");
        FileHelper.CreateFileByTemplate(repFilePath,ss, "Repository/EntityRep.txt");
    }


    #endregion
}