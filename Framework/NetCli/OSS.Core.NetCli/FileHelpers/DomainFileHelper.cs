using System.Collections.Generic;
using System.IO;

internal static class DomainFileHelper
{
    public static void CreateDomainFiles(ProjectFileNames mParas)
    {
        var projectPath = Path.Combine(mParas.base_path, mParas.domain_project_name);
        FileHelper.CreateDirectory(projectPath);
        
        var packageRefs =new List<string>()
        {
            "OSS.Core.Domain"
        };
        var projectRefs = new List<string>()
        {
            $"..\\{mParas.domain_opened_project_name}\\{mParas.domain_opened_project_name}.csproj"
        };

        var projectFilePath = Path.Combine(projectPath, mParas.domain_project_name+".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, projectRefs);
    }

    public static void CreateDomainOpenedFiles(ProjectFileNames mParas)
    {
        var projectPath = Path.Combine(mParas.base_path, mParas.domain_opened_project_name);
        FileHelper.CreateDirectory(projectPath);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Domain.Opened"
        };

        var projectFilePath = Path.Combine(projectPath, mParas.domain_opened_project_name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, null);
    }
}