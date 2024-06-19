namespace OSSCore;

internal class RepTool : BaseProjectTool
{
    public override void Create(Solution solution)
    {
        base.Create(solution);

        Console.WriteLine($"仓储层类库 ({solution.rep_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        if (ss.mode != SolutionMode.Full)
            return;

        var project = ss.rep_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Rep.Dapper.Mysql", "OSS.Core.Extension.Cache", "OSS.Tools.Config"
        };

        var domainProject = ss.mode == SolutionMode.Normal ? ss.domain_open_project.name : ss.domain_project.name;
        var projectRefs = new List<string>()
        {
            $"..{(ss.mode == SolutionMode.Normal ? "\\Open" : string.Empty)}\\{domainProject}\\{domainProject}.csproj"
        };
        CreateProjectFile(project.project_file_path, packageRefs, projectRefs);
    }

    public override void Create_CommonFiles(Solution ss)
    {
        var project    = ss.rep_project;
        var baseRepDir = project.common_dir;

        FileHelper.CreateDirectory(baseRepDir);

        var dbRepTemplateDir = GetRepDBTypeTemplateDir(ss);
        var baeRepFilePath   = Path.Combine(baseRepDir, $"{project.base_class_name}.cs");

        FileHelper.CreateFileByTemplate(baeRepFilePath, ss, $"Repository/{dbRepTemplateDir}/BaseRep.txt");
    }

    public override void Create_GlobalFiles(Solution ss)
    {
        if (ss.mode <= SolutionMode.Normal)
            return;

        var project = ss.rep_project;
        FileHelper.CreateDirectory(project.global_dir);

        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        FileHelper.CreateFileByTemplate(starterFilePath, ss, "Repository/RepAppStarter.txt");
    }

    #region 添加实体

    public override void AddEntity(Solution ss)
    {
        AddEntity_Rep(ss);
        AddEntity_ChangeStarter(ss);

        Console.WriteLine("仓储层实体 -- done");
    }

    private static void AddEntity_Rep(Solution ss)
    {
        var repDir = Path.Combine(ss.rep_project.project_dir, ss.entity_code);
        if (ss.mode != SolutionMode.Full)
        {
            repDir = Path.Combine(repDir, "Rep");
        }

        FileHelper.CreateDirectory(repDir);

        var dbRepTemplateDir   = GetRepDBTypeTemplateDir(ss);
        var repFilePath = Path.Combine(repDir, $"{ss.entity_code}Rep.cs");

        FileHelper.CreateFileByTemplate(repFilePath, ss, $"Repository/{dbRepTemplateDir}/EntityRep.txt");
    }

    private static string GetRepDBTypeTemplateDir(Solution ss)
    {
        var repTxtDir = ss.db_type == DBType.SqlServer ? "SqlServer" : "MySql";
        return repTxtDir;
    }

    // 修改仓储启动注册文件,注入仓储接口实现
    private static void AddEntity_ChangeStarter(Solution ss)
    {
        if (ss.mode != SolutionMode.Full) //    仅全模式下才会有数据层接口注入
            return;

        var project         = ss.rep_project;
        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");

        var injectRepStr = @$"
        // {ss.entity_display} 仓储接口注入
        InsContainer<I{ss.entity_code}Rep>.Set<{ss.entity_code}Rep>();
";

        FileHelper.InsertFileFuncContent(starterFilePath, injectRepStr, "Start(IServiceCollection");
    }

    #endregion
}