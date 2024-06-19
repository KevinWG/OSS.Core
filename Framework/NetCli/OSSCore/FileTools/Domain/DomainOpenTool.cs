using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal class DomainOpenTool : BaseProjectTool
{
    public override void Create(Solution solution)
    {
        base.Create(solution);
        Console.WriteLine($"领域层类库（共享）({solution.domain_open_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        var project = ss.domain_open_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Domain.Open"
        };

        CreateProjectFile(project.project_file_path, packageRefs, null,false,project.name);
    }



    #region 添加实体

    public override void AddEntity(Solution ss)
    {
        AddEntity_Mo(ss);
        AddEntity_DTO(ss);

        AddEntity_IOpenService(ss);

        Console.WriteLine("领域层实体（共享） -- done");
    }

    private static void AddEntity_Mo(Solution ss)
    {
        var entityDir = Path.Combine(ss.domain_open_project.entity_dir, "Entity");
        FileHelper.CreateDirectory(entityDir);

        var entityFilePath = Path.Combine(entityDir, $"{ss.entity_code}Mo.cs");
        FileHelper.CreateFileByTemplate(entityFilePath, ss, "Domain/EntityMo.txt");
    }


    private static void AddEntity_DTO(Solution ss)
    {
        var dtoDir = Path.Combine(ss.domain_open_project.entity_dir, "DTO");
        FileHelper.CreateDirectory(dtoDir);

        var addEntFilePath = Path.Combine(dtoDir, string.Concat("Add", ss.entity_code, "Req.cs"));
        FileHelper.CreateFileByTemplate(addEntFilePath, ss, "Domain/DTO/AddEntityReq.txt");

        var searchEntFilePath = Path.Combine(dtoDir, string.Concat("Search", ss.entity_code, "Req.cs"));
        FileHelper.CreateFileByTemplate(searchEntFilePath, ss, "Domain/DTO/SearchEntityReq.txt");
    }

    private static void AddEntity_IOpenService(Solution ss)
    {
        var interfaceDir = Path.Combine(ss.domain_open_project.entity_dir, "IService");
        FileHelper.CreateDirectory(interfaceDir);

        var oServiceFilePath = Path.Combine(interfaceDir, $"I{ss.entity_code}OpenService.cs");
        FileHelper.CreateFileByTemplate(oServiceFilePath, ss, "Domain/Open/IEntityOpenService.txt");
    }

    #endregion
}