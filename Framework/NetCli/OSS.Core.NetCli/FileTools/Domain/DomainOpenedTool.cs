using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal class DomainOpenedTool : BaseProjectTool
{
    public override void Create(Solution solution)
    {
        base.Create(solution);
        Console.WriteLine($"领域层类库（共享）({solution.domain_opened_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        var project = ss.domain_opened_project;
        FileHelper.CreateDirectory(project.project_dir);
        
        var packageRefs = new List<string>()
        {
            "OSS.Core.Domain.Opened"
        };

        CreateProjectFile(project.project_file_path, packageRefs, null);
    }



    #region 添加实体

    public override void AddEntity(Solution ss)
    {
        FileHelper.CreateDirectory(ss.domain_opened_project.entity_dir);

        var entityFilePath = Path.Combine(ss.domain_opened_project.entity_dir, $"{ss.entity_name}Mo.cs");
        FileHelper.CreateFileByTemplate(entityFilePath, ss, "Domain/EntityMo.txt");

        AddEntity_DTO(ss);

        Console.WriteLine("领域层实体（共享） -- done");
    }

    private static void AddEntity_DTO(Solution ss)
    {
        var dtoDir = Path.Combine(ss.domain_opened_project.entity_dir, "DTO");
        FileHelper.CreateDirectory(dtoDir);

        var addEntFilePath = Path.Combine(dtoDir, string.Concat("Add", ss.entity_name, "Req.cs"));
        FileHelper.CreateFileByTemplate(addEntFilePath, ss, "Domain/DTO/AddEntityReq.txt");
    }

    #endregion
}