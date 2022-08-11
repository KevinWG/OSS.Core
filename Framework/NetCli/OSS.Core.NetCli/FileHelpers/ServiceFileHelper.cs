using System.Collections.Generic;
using System.IO;

internal static class ServiceFileHelper
{
    public static void CreateServiceOpenedFiles(ProjectFileNames mParas)
    {
        var projectPath = Path.Combine(mParas.base_path, mParas.service_opened_project_name);
        FileHelper.CreateDirectory(projectPath);
        
        var projectRefs = new List<string>()
        {
            $"..\\{mParas.domain_opened_project_name}\\{mParas.domain_opened_project_name}.csproj"
        };

        var projectFilePath = Path.Combine(projectPath, mParas.service_opened_project_name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, null, projectRefs);
    }

    public static void CreateServiceFiles(ProjectFileNames mParas)
    {
        var projectPath = Path.Combine(mParas.base_path, mParas.service_project_name);
        FileHelper.CreateDirectory(projectPath);
        
        var packageRefs = new List<string>()
        {
            "OSS.DataFlow","OSS.Core.Extension.Cache","OSS.Tools.Log"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{mParas.domain_project_name}\\{mParas.domain_project_name}.csproj",
            $"..\\{mParas.service_opened_project_name}\\{mParas.service_opened_project_name}.csproj"
        };

        var projectFilePath = Path.Combine(projectPath, mParas.service_project_name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, projectRefs);
    }
}