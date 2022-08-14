
using System;
using System.IO;
using System.Text;

namespace OSSCore;

internal class SolutionFileTool : BaseProjectTool
{
    private static readonly WebApiFilesTool  _webapiTool  = new();
    private static readonly ServiceFilesTool _serviceTool = new();
    private static readonly RepFilesTool     _repTool = new();
    private static readonly DomainFilesTool  _domainTool  = new();

    private static readonly ServiceOpenedFilesTool _serviceOpenedTool = new();
    private static readonly DomainOpenedFilesTool  _domainOpenedTool  = new();

    private static readonly ClientFilesTool _clientTool = new();

    public override void Create_Project(SolutionStructure ss)
    {
        _domainOpenedTool.Create(ss);
        _serviceOpenedTool.Create(ss);
        _clientTool.Create(ss);

        Console.WriteLine(" ");

        _domainTool.Create(ss);
        _repTool.Create(ss);
        _serviceTool.Create(ss);
        _webapiTool.Create(ss);
        
        CreateSolutionFile(ss);

        Console.WriteLine("全部完成!");
    }

    private static void CreateSolutionFile(SolutionStructure ss)
    {
        var slnContent = new StringBuilder();

        slnContent.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");

        slnContent.AppendLine(
            "Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"Opened\", \"Opened\", \"{77C70B84-7F0C-4B68-A201-AF182B4807C3}\"");
        slnContent.AppendLine("EndProject");

        slnContent.AppendLine(
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.domain_opened_project.name}\", \"{ss.domain_opened_project.name}\\{ss.domain_opened_project.name}.csproj\", \"{{31C4D646-CFB9-4D8A-A4F2-8786E45B5723}}\"");
        slnContent.AppendLine("EndProject");

        slnContent.AppendLine(
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.service_opened_project.name}\", \"{ss.service_opened_project.name}\\{ss.service_opened_project.name}.csproj\", \"{{9C27F876-B43F-4233-A693-0C1F9DC1DF2A}}\"");
        slnContent.AppendLine("EndProject");

        slnContent.AppendLine(
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.domain_project.name}\", \"{ss.domain_project.name}\\{ss.domain_project.name}.csproj\", \"{{865B73D7-123E-41C9-86D5-6386753F924C}}\"");
        slnContent.AppendLine("EndProject");

        slnContent.AppendLine(
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.service_project.name}\", \"{ss.service_project.name}\\{ss.service_project.name}.csproj\", \"{{91D0F8AB-E5D0-41E8-8C40-2D270B12878F}}\"");
        slnContent.AppendLine("EndProject");

        if (ss.solution_mode == SolutionMode.Default)
        {
            slnContent.AppendLine(
                $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.rep_project.name}\", \"{ss.rep_project.name}\\{ss.rep_project.name}.csproj\", \"{{27898B9F-7BCD-4221-BA1C-822C9D4574F9}}\"");
            slnContent.AppendLine("EndProject");

        }

        slnContent.AppendLine(
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{ss.webapi_project.name}\", \"{ss.webapi_project.name}\\{ss.webapi_project.name}.csproj\", \"{{A0D11BA9-C5C1-44A5-8EB1-038A0DAA6423}}\"");
        slnContent.AppendLine("EndProject");

        slnContent.AppendLine("Global");
        slnContent.AppendLine("	GlobalSection(NestedProjects) = preSolution");
        slnContent.AppendLine("		{31C4D646-CFB9-4D8A-A4F2-8786E45B5723} = {77C70B84-7F0C-4B68-A201-AF182B4807C3}");
        slnContent.AppendLine("		{9C27F876-B43F-4233-A693-0C1F9DC1DF2A} = {77C70B84-7F0C-4B68-A201-AF182B4807C3}");
        slnContent.AppendLine("	EndGlobalSection");
        slnContent.AppendLine("EndGlobal");

        var slnFilePath = Path.Combine(ss.base_path, ss.solution_name + ".sln");
        FileHelper.CreateFile(slnFilePath, slnContent.ToString());
    }
}