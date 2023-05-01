
using System;
using System.IO;
using System.Text;

namespace OSSCore;

internal class SolutionTool : BaseProjectTool
{
    private static readonly WebApiTool  _webapiTool  = new();
    private static readonly ServiceTool _serviceTool = new();
    private static readonly RepTool     _repTool = new();

    private static readonly DomainTool     _domainTool       = new();
    private static readonly DomainOpenTool _domainOpenedTool = new();

    private static readonly ClientTool _clientTool = new();
    
    #region 创建

    public override void Create_Project(Solution ss)
    {
        _domainOpenedTool.Create(ss);
        _clientTool.Create(ss);

        _domainTool.Create(ss);

        _repTool.Create(ss);
        _serviceTool.Create(ss);
        _webapiTool.Create(ss);

        CreateSolutionFile(ss);

        Console.WriteLine("全部完成!");
    }

    private static void CreateSolutionFile(Solution ss)
    {
        var domainOpenId = Guid.NewGuid().ToString();
        var clientId     = Guid.NewGuid().ToString();

        var domainId  = Guid.NewGuid().ToString();
        var repId = Guid.NewGuid().ToString();
        var serviceId = Guid.NewGuid().ToString();
        var webApiId  = Guid.NewGuid().ToString();

        var slnContent = new StringBuilder();
        slnContent.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");

        slnContent.AppendLine(
            "Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"Open\", \"Open\", \"{77C70B84-7F0C-4B68-A201-AF182B4807C3}\"");
        slnContent.AppendLine("EndProject");

        slnContent.AppendLine(
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.domain_open_project.name}\", \"Open\\{ss.domain_open_project.name}\\{ss.domain_open_project.name}.csproj\", \"{domainOpenId}\"");
        slnContent.AppendLine("EndProject");

        slnContent.AppendLine(
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.http_client_project.name}\", \"Open\\{ss.http_client_project.name}\\{ss.http_client_project.name}.csproj\", \"{clientId}\"");
        slnContent.AppendLine("EndProject");

        if (ss.mode != SolutionMode.Simple)
        {
            slnContent.AppendLine(
                $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.domain_project.name}\", \"{ss.domain_project.name}\\{ss.domain_project.name}.csproj\", \"{domainId}\"");
            slnContent.AppendLine("EndProject");
        }

        slnContent.AppendLine(
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.service_project.name}\", \"{ss.service_project.name}\\{ss.service_project.name}.csproj\", \"{serviceId}\"");
        slnContent.AppendLine("EndProject");

            slnContent.AppendLine(
                $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.rep_project.name}\", \"{ss.rep_project.name}\\{ss.rep_project.name}.csproj\", \"{repId}\"");
            slnContent.AppendLine("EndProject");
     

        slnContent.AppendLine(
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.webapi_project.name}\", \"{ss.webapi_project.name}\\{ss.webapi_project.name}.csproj\", \"{webApiId}\"");
        slnContent.AppendLine("EndProject");

        slnContent.AppendLine("Global");
        slnContent.AppendLine("	GlobalSection(NestedProjects) = preSolution");
        slnContent.AppendLine($"		{domainOpenId} = {{77C70B84-7F0C-4B68-A201-AF182B4807C3}}");
        slnContent.AppendLine($"		{clientId} = {{77C70B84-7F0C-4B68-A201-AF182B4807C3}}");
        slnContent.AppendLine("	EndGlobalSection");
        slnContent.AppendLine("EndGlobal");

        var slnFilePath = Path.Combine(ss.base_path, ss.solution_name + ".sln");
        FileHelper.CreateFile(slnFilePath, slnContent.ToString());
    }


    #endregion

    #region 添加实体

    public override void AddEntity(Solution ss)
    {
        _domainOpenedTool.AddEntity(ss);
        _clientTool.AddEntity(ss);

        _domainTool.AddEntity(ss);

        _repTool.AddEntity(ss);
        _serviceTool.AddEntity(ss);
        _webapiTool.AddEntity(ss);

        Console.WriteLine("全部完成!");
    }

    #endregion
}