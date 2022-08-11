using System.Collections.Generic;
using System.IO;
using System.Text;

internal static class FileHelper
{
    public static void CreateDirectory(string projectPath)
    {
        if (!Directory.Exists(projectPath))
        {
            Directory.CreateDirectory(projectPath);
        }
    }

    public static void CreateProjectFile(string filePath, List<string> packageRefs,List<string> projectRefs)
    {
        var content = new StringBuilder();
        content.AppendLine(@"<Project Sdk=""Microsoft.NET.Sdk"">
     <PropertyGroup>
         <TargetFramework>net6.0</TargetFramework>
         <ImplicitUsings>enable</ImplicitUsings>
         <Nullable>enable</Nullable>
     </PropertyGroup>");

        if (packageRefs!=null&&packageRefs.Count>0)
        {
            content.AppendLine("    <ItemGroup>");
            foreach (var packageRef in packageRefs)
            {
                content.AppendLine($"       <PackageReference Include=\"{packageRef}\" />");
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

    public static void CreateFile(string filePath,string fileContent)
    {
        if(File.Exists(filePath))
            File.Delete(filePath);

        using var sw = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write),
            Encoding.UTF8);
        sw.WriteLine(fileContent);
    }
}