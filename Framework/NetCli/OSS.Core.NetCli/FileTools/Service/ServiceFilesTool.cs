using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;

internal class ServiceFilesTool : BaseProjectTool
{
    public override void Create(SolutionStructure solution)
    {
        base.Create(solution);
        Console.WriteLine($"服务层类库 ({solution.service_project.name}) -- done");
    }

    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.service_project;
        FileHelper.CreateDirectory(project.project_dir);
        
        var packageRefs = new List<string>()
        {
            "OSS.DataFlow",
            "OSS.Tools.Log",
            "OSS.Core.Extension.Cache",
            "OSS.Core.Extension.PassToken"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_project.name}\\{ss.domain_project.name}.csproj",
            $"..\\{ss.service_opened_project.name}\\{ss.service_opened_project.name}.csproj"
        };

        var projectFilePath = Path.Combine(project.project_dir, project.name + ".csproj");
        CreateProjectFile(projectFilePath, packageRefs, projectRefs);
    }

    public override void Create_GlobalFiles(SolutionStructure ss)
    {
        var project = ss.service_project;
        FileHelper.CreateDirectory(project.global_dir);

        var baeStarterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        FileHelper.CreateFileByTemplate(baeStarterFilePath, ss, "Service/ServiceAppStarter.txt");

        var localClientPath = Path.Combine(project.global_dir, $"{project.local_client_name}.cs");
        FileHelper.CreateFileByTemplate(localClientPath, ss, "Service/LocalModuleClient.txt");
    }
    

    #region 添加实体

    public override void AddEntity(SolutionStructure ss)
    {
        FileHelper.CreateDirectory(ss.service_project.entity_dir);

        var repDefine = ss.solution_mode == SolutionMode.Default 
            ? $"I{ss.entity_name}Rep _{ss.entity_name}Rep = InsContainer<I{ss.entity_name}Rep>.Instance" 
            : $"{ss.entity_name}Rep _{ss.entity_name}Rep = new()";

        var oServiceFilePath = Path.Combine(ss.service_project.entity_dir, $"{ss.entity_name}Service.cs");
        FileHelper.CreateFileByTemplate(oServiceFilePath, ss, "Service/EntityService.txt",
            new Dictionary<string, string>() { { "{rep_define}", repDefine } });
        
        AddEntity_ChangeLocalClient(ss);

        Console.WriteLine("服务层实体 -- done");
    }

    private static void AddEntity_ChangeLocalClient(SolutionStructure ss)
    {
        var project = ss.service_project;

        var localClientPath = Path.Combine(project.global_dir, $"{project.local_client_name}.cs");
        var injectStr = @$"
    /// <inheritdoc />
    public IOpened{ss.entity_name}Service {ss.entity_name} {{ get; }} = SingleInstance<{ss.entity_name}Service>.Instance;";

        FileHelper.InsertFileFuncContent(localClientPath, injectStr, project.local_client_name);
    }

    #endregion
}