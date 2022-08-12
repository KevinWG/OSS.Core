using System.Collections.Generic;
using System.IO;

namespace OSS.Core.NetCli;
internal  class RepFilesTool : BaseTool
{
    public override void Create(SolutionStructure ss)
    {
        CreateProjectFile(ss);
        CreateCommonFiles(ss);
        CreateGlobalFiles(ss);
    }
    
    private static void CreateProjectFile(SolutionStructure ss)
    {
        var packageRefs = new List<string>()
        {
            "OSS.Core.Rep.Dapper.Mysql", "OSS.Core.Extension.Cache","OSS.Tools.Log"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_project.name}\\{ss.domain_project.name}.csproj"
        };
        
        FileHelper.CreateProjectFile(ss.rep_project.project_file_path, packageRefs, projectRefs);
    }

    private static void CreateCommonFiles(SolutionStructure ss)
    {
        var baeRepFilePath = Path.Combine(ss.rep_project.common_dir, $"{ss.rep_project.base_file_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, "Templates/Repository/BaseRep.txt");
    }

    private static void CreateGlobalFiles(SolutionStructure ss)
    {
        var baeRepFilePath = Path.Combine(ss.rep_project.global_dir, $"{ss.rep_project.starter_file_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, "Templates/Repository/RepAppStarter.txt");
    }
}