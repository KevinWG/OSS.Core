using System.Collections.Generic;
using System.IO;

namespace OSS.Core.NetCli;
internal  class DomainFilesTool:BaseProjectTool
{
    public override void Create_Project(SolutionStructure ss)
    {
        var project = ss.domain_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Domain"
        };
        var projectRefs = new List<string>()
        {
            $"..\\{ss.domain_opened_project.name}\\{ss.domain_opened_project.name}.csproj"
        };

        FileHelper.CreateProjectFile(project.project_file_path, packageRefs, projectRefs);
    }



    public override void Create_CommonFiles(SolutionStructure ss)
    {
        var project = ss.domain_project;
        FileHelper.CreateDirectory(project.common_dir);

        var baeRepFilePath = Path.Combine(project.common_dir, $"{project.const_file_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, "Domain/DomainConst.txt");
    }
    
    
}