using System.Collections.Generic;
using System.IO;

internal static class WebApiFileHelper
{
    public static void CreateWebApiFiles(ProjectFileNames mParas)
    {
        var projectPath = Path.Combine(mParas.base_path, mParas.webapi_project_name);
        FileHelper.CreateDirectory(projectPath);

        var packageRefs = new List<string>()
        {
            "Swashbuckle.AspNetCore"
        };

        var projectRefs = new List<string>()
        {
            $"..\\{mParas.service_project_name}\\{mParas.service_project_name}.csproj",
            $"..\\{mParas.repository_project_name}\\{mParas.repository_project_name}.csproj"
        };

        var projectFilePath = Path.Combine(projectPath, mParas.webapi_project_name + ".csproj");
        FileHelper.CreateProjectFile(projectFilePath, packageRefs, projectRefs);
    }
}