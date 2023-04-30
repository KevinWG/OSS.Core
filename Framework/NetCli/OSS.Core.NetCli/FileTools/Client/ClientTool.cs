using System;
using System.Collections.Generic;
using System.IO;

namespace OSSCore;
internal  class ClientTool : BaseProjectTool
{
    #region 创建

    public override void Create(Solution ss)
    {
        base.Create(ss);
        Console.WriteLine($"Http客户端类库({ss.http_client_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        var project = ss.http_client_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Client.Http"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_open_project.name}\\{ss.domain_open_project.name}.csproj"
        };

        CreateProjectFile(project.project_file_path, packageRefs, projectRefs);

        var moduleClientPath = Path.Combine(project.project_dir, $"{project.module_client_name}.cs");
        FileHelper.CreateFileByTemplate(moduleClientPath, ss, "Client/Http/ModuleClient.txt");
    }

    public override void Create_CommonFiles(Solution ss)
    {
        var project = ss.http_client_project;
        FileHelper.CreateDirectory(project.common_dir);

        var moduleReqPath = Path.Combine(project.common_dir, $"{project.module_req_name}.cs");
        FileHelper.CreateFileByTemplate(moduleReqPath, ss, "Client/Http/ModuleRequest.txt"
            , new Dictionary<string, string>() { { "{module_req}", project.module_req_name }});
    }


    #endregion


    #region 新增实体

    public override void AddEntity(Solution ss)
    {
        var project = ss.http_client_project;
        FileHelper.CreateDirectory(project.entity_dir);

        var entityClientPath = Path.Combine(project.entity_dir, $"{ss.entity_name}HttpClient.cs");

        FileHelper.CreateFileByTemplate(entityClientPath,ss, "Client/Http/EntityClient.txt",
            new Dictionary<string, string>(){{ "{module_req}",project.module_req_name } });

        AddEntity_ChangeModuleClient(ss);
    }

    private static void AddEntity_ChangeModuleClient(Solution ss)
    {
        var project         = ss.http_client_project;
        
        var moduleClientPath = Path.Combine(project.project_dir, $"{project.module_client_name}.cs");
        var injectStr     = @$"
    /// <summary>
    ///  {ss.entity_name} 接口
    /// </summary>
    public static I{ss.entity_name}OpenService {ss.entity_name} {{get; }} = SingleInstance<{ss.entity_name}HttpClient>.Instance;";

        FileHelper.InsertFileFuncContent(moduleClientPath, injectStr, project.module_client_name);
    }
    #endregion

}