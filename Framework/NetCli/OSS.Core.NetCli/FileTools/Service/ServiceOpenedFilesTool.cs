using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal  class ServiceOpenedFilesTool : BaseProjectTool
{
    public override void Create(SolutionStructure solution)
    {
        base.Create(solution);
        Console.WriteLine($"服务层类库（共享） ({solution.service_opened_project.name}) -- done");
    }

    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.service_opened_project;
        FileHelper.CreateDirectory(project.project_dir);

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_opened_project.name}\\{ss.domain_opened_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(project.project_dir, project.name + ".csproj");
        CreateProjectFile(projectFilePath, null, projectRefs);
    }

    public override void Create_GlobalFiles(SolutionStructure ss)
    {
        var project = ss.service_opened_project;
        FileHelper.CreateDirectory(project.global_dir);

        var baeClientFilePath = Path.Combine(project.global_dir, $"{project.client_interface_name}.cs");
        FileHelper.CreateFileByTemplate(baeClientFilePath, ss, "Service/IModuleClient.txt");
    }


    #region 添加实体

    public override void AddEntity(SolutionStructure ss)
    {
        FileHelper.CreateDirectory(ss.service_opened_project.entity_dir);

        var oServiceFilePath = Path.Combine(ss.service_opened_project.entity_dir, $"IOpened{ss.entity_name}Service.cs");
        FileHelper.CreateFileByTemplate(oServiceFilePath,ss, "Service/IOpenedEntityService.txt");

        AddEntity_ChangeModuleClient(ss);

        Console.WriteLine("服务层实体（共享） -- done");
    }

    private static void AddEntity_ChangeModuleClient(SolutionStructure ss)
    {
        var project = ss.service_opened_project;
        
        var moduleClientPath = Path.Combine(project.global_dir, $"{project.client_interface_name}.cs");
        var injectStr = @$"
    /// <summary>
    ///  {ss.entity_name} 开放服务接口
    /// </summary>
    public IOpened{ss.entity_name}Service {ss.entity_name} {{get; }}";

        FileHelper.InsertFileFuncContent(moduleClientPath, injectStr, project.client_interface_name);
    }
    #endregion
}