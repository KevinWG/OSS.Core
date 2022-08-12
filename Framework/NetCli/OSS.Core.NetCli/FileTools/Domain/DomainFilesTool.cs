using System.Collections.Generic;
using System.IO;

namespace OSS.Core.NetCli;
internal  class DomainFilesTool:BaseTool
{
    public override void Create(SolutionStructure pFiles)
    {
        CreateDomainFiles(pFiles);
    }

    private static void CreateDomainFiles(SolutionStructure ss)
    {
        CreateDomain_ProjectFiles(ss);
    }

    private static void CreateDomain_ProjectFiles(SolutionStructure ss)
    {
        var packageRefs = new List<string>()
        {
            "OSS.Core.Domain"
        };
        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_opened_project.name}\\{ss.domain_opened_project.name}.csproj"
        };

        FileHelper.CreateProjectFile(ss.domain_project.project_file_path, packageRefs, projectRefs);
    }

    private static void CreateDomain_CommonFiles(SolutionStructure ss)
    {
        var baeRepFilePath = Path.Combine(ss.domain_project.common_dir, $"{ss.domain_project.const_file_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, "Templates/Domain/DomainConst.txt");
    }
    
    
}