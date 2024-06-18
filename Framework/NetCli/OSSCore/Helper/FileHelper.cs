using System.Text;
using Fluid;

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
    
    public static void CreateFile(string filePath, string fileContent)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        using var sw = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write),Encoding.UTF8);
        sw.WriteLine(fileContent);
    }

    public static string LoadFile(string filePath)
    {
        string    content;
        using var file = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read));
        {
            content = file.ReadToEnd();
        }
        return content;
    }

    public static void InsertFileFuncContent(string filePath, string insertContent, string flag)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        var content = FileHelper.LoadFile(filePath);

        var index = content.IndexOf('{', content.IndexOf(flag))+1;

        var newContent = content.Contains(insertContent) ? content : content.Insert(index, insertContent);

        CreateFile(filePath, newContent);
    }



    private static readonly FluidParser _fluidParser = new();

    /// <summary>
    ///  通过模版生成文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="ss"></param>
    /// <param name="templateRelativePath"></param>
    public static void CreateFileByTemplate(string filePath, Solution ss, string templateRelativePath)
    {
        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", templateRelativePath);
        var content      = LoadFile(templatePath);

        if (!_fluidParser.TryParse(content, out var template, out var error))
        {
            Console.WriteLine($"({filePath})文件处理异常： {error}");
        }

        var context = new TemplateContext(ss);
        content = template.Render(context);

        CreateFile(filePath, content);
    }







    #region 通过替换处理模版

    [Obsolete]
    public static void CreateFileByReplaceTemplate(string filePath, Solution ss, string templateRelativePath, Dictionary<string, string> extParas = null)
    {
        var content = LoadTemplateContent(ss,templateRelativePath, extParas);
        CreateFile(filePath, content);
    }
    [Obsolete]
    private static string LoadTemplateContent(Solution ss, string templateRelativePath, Dictionary<string, string> extParas = null)
    {
        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", templateRelativePath);
        var content = LoadFile(templatePath);

        var newContent = content
            .Replace("{solution_name}", ss.solution_name)

            .Replace("{module_name}", ss.module_name)
            .Replace("{module_display}", ss.module_display)

            .Replace("{entity_name}", ss.entity_name)
            .Replace("{entity_display}", ss.entity_display)

            .Replace("{domain_project_name}", ss.domain_project.name)
            .Replace("{domain_opened_project_name}", ss.domain_open_project.name)

            .Replace("{service_project_name}", ss.service_project.name)
            .Replace("{repository_project_name}", ss.rep_project.name)

            .Replace("{webapi_project_name}", ss.webapi_project.name);

        if (extParas ==null)
        {
            return newContent;
        }

      
        return  extParas.Aggregate(newContent,
                (eNew, kPair) => eNew.Replace( kPair.Key, kPair.Value));
    }

    #endregion
}