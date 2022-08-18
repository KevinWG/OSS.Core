using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal  class ServiceOpenedTool : BaseProjectTool
{
    public override void Create(Solution solution)
    {
        base.Create(solution);
        Console.WriteLine($"服务层类库（共享） ({solution.service_open_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        var project = ss.service_open_project;
        FileHelper.CreateDirectory(project.project_dir);

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_open_project.name}\\{ss.domain_open_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(project.project_dir, project.name + ".csproj");
        CreateProjectFile(projectFilePath, null, projectRefs);
    }
    
    #region 添加实体

    public override void AddEntity(Solution ss)
    {
        FileHelper.CreateDirectory(ss.service_open_project.entity_dir);

        var oServiceFilePath = Path.Combine(ss.service_open_project.entity_dir, $"I{ss.entity_name}OpenService.cs");
        FileHelper.CreateFileByTemplate(oServiceFilePath,ss, "Service/IEntityOpenService.txt");

        Console.WriteLine("服务层实体（共享） -- done");
    }

 
    #endregion
}