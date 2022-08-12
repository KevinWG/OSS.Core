using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OSSCore;

internal static class FileHelper
{
    public static void CreateDirectory(string projectPath)
    {
        if (!Directory.Exists(projectPath))
        {
            Directory.CreateDirectory(projectPath);
        }
    }

    public static void CreateProjectFile(string filePath, List<string> packageRefs, List<string> projectRefs,
                                         bool isWeb = false)
    {
        var content = new StringBuilder();

        content.AppendLine(isWeb ? "<Project Sdk=\"Microsoft.NET.Sdk.Web\">" : "<Project Sdk=\"Microsoft.NET.Sdk\">");

        content.AppendLine(@"
     <PropertyGroup>
         <TargetFramework>net6.0</TargetFramework>
         <ImplicitUsings>enable</ImplicitUsings>
         <Nullable>enable</Nullable>
     </PropertyGroup>");

        if (packageRefs != null && packageRefs.Count > 0)
        {
            content.AppendLine("    <ItemGroup>");
            foreach (var packageRef in packageRefs)
            {
                content.AppendLine($"       <PackageReference Include=\"{packageRef}\" Version=\"*\"/>");
            }
            content.AppendLine("    </ItemGroup>");
        }

        if (projectRefs is {Count: > 0})
        {
            content.AppendLine("    <ItemGroup>");
            foreach (var item in projectRefs)
            {
                content.AppendLine($"       <ProjectReference Include=\"{item}\" />");
            }

            content.AppendLine("    </ItemGroup>");
        }

        content.AppendLine("</Project>");

        CreateFile(filePath, content.ToString());
    }

    public static void CreateFile(string filePath, string fileContent)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        using var sw = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write),
            Encoding.UTF8);
        sw.WriteLine(fileContent);
    }

    public static void CreateFileByTemplate(string filePath, SolutionStructure pInfo, string templateRelativePath)
    {
        var    templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Templates", templateRelativePath);
        string content;

        using (var file = new StreamReader(new FileStream(templatePath, FileMode.Open, FileAccess.Read)))
        {
            content = file.ReadToEnd();
        }

        var newContent = content.Replace("{module_name}", pInfo.module_name)
            .Replace("{solution_name}", pInfo.solution_name)
            .Replace("{domain_project_name}", pInfo.domain_project.name)
            .Replace("{domain_opened_project_name}", pInfo.domain_opened_project.name)
            .Replace("{service_project_name}", pInfo.service_project.name)
            .Replace("{service_opened_project_name}", pInfo.service_opened_project.name)
            .Replace("{repository_project_name}", pInfo.rep_project.name)
            .Replace("{webapi_project_name}", pInfo.webapi_project.name);

        CreateFile(filePath, newContent);
    }

}