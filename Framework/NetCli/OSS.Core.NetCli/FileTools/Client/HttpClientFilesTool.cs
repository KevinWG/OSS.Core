using System;
using System.Collections.Generic;

namespace OSSCore;
internal  class HttpClientFilesTool : BaseProjectTool
{
    public override void Create(SolutionStructure ss)
    {
        base.Create(ss);
        Console.WriteLine($"Http客户端类库({ss.http_client_project.name}) -- done");
    }

    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.http_client_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Client.Http"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_opened_project.name}\\{ss.domain_opened_project.name}.csproj",
            $"..\\{ss.service_opened_project.name}\\{ss.service_opened_project.name}.csproj"
        };

        CreateProjectFile(project.project_file_path, packageRefs, projectRefs);


    }
}