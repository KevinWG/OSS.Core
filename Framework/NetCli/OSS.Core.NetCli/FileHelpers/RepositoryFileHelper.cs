using System.Collections.Generic;
using System.IO;

internal static class RepositoryFileHelper
{
    public static void CreateRepositoryFiles(ProjectFileNames mParas)
    {
        var projectPath = Path.Combine(mParas.base_path, mParas.repository_project_name);
        FileHelper.CreateDirectory(projectPath);

        
        var packageRefs = new List<string>()
        {
            "OSS.Core.Rep.Dapper.Mysql", "OSS.Core.Extension.Cache","OSS.Tools.Log"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{mParas.domain_project_name}\\{mParas.domain_project_name}.csproj"
        };

        var projectFilePath = Path.Combine(projectPath, mParas.repository_project_name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, projectRefs);
    }
}