namespace OSSCore;

internal class DomainTool : BaseProjectTool
{
    #region 创建

    public override void Create(Solution solution)
    {
        base.Create(solution);

        Create_ReadMeFile(solution);

        Console.WriteLine($"领域层类库({solution.domain_project.name}) -- done");
    }

    public override void Create_Project(Solution ss)
    {
        var project = ss.domain_project;
        FileHelper.CreateDirectory(project.project_dir);

        var packageRefs = new List<string>()
        {
            "OSS.Core.Domain"
        };

        if (ss.mode <= SolutionMode.Normal)
        {
            if (ss.mode <= SolutionMode.Simple)
            {
                // 极简模式下 服务放置
                packageRefs.AddRange(new[]
                {
                    "OSS.DataFlow",
                    "OSS.Core.Extension.Cache",
                    "OSS.Core.Extension.PassToken"
                });
            }

            packageRefs.Add("OSS.Tools.Log");
            packageRefs.Add("OSS.Tools.Config");
            switch (ss.db_type)
            {
                case DBType.SqlServer:
                    packageRefs.Add("OSS.Core.Rep.Dapper.SqlServer");
                    break;
                default:
                    packageRefs.Add("OSS.Core.Rep.Dapper.Mysql");
                    break;
            }
        }

        var projectRefs = new List<string>()
        {
            $"..\\Open\\{ss.domain_open_project.name}\\{ss.domain_open_project.name}.csproj"
        };

        CreateProjectFile(project.project_file_path, packageRefs, projectRefs);
    }


    public  void Create_ReadMeFile(Solution ss)
    {
        var projectDir = ss.domain_project.project_dir;
        FileHelper.CreateDirectory(projectDir);

        var readMeFilePath = Path.Combine(projectDir, $"ReadMe.txt");
        FileHelper.CreateFileByTemplate(readMeFilePath, ss, "Domain/ReadMe.txt");
    }

    /// <inheritdoc />
    public override void Create_CommonFiles(Solution ss)
    {
    }

    /// <inheritdoc />
    public override void Create_GlobalFiles(Solution solution)
    {
        if (solution.mode == SolutionMode.Normal)
            return;

        var project = solution.domain_project;
        FileHelper.CreateDirectory(project.global_dir);

        var starterFilePath = Path.Combine(project.global_dir, $"{project.starter_class_name}.cs");
        FileHelper.CreateFileByTemplate(starterFilePath, solution, "Domain/DomainAppStarter.txt");

        var baeRepFilePath = Path.Combine(project.global_dir, $"{project.const_file_name}.cs");
        FileHelper.CreateFileByTemplate(baeRepFilePath, solution, "Domain/ModuleConst.txt");
    }

    #endregion

    #region 添加实体

    public override void AddEntity(Solution ss)
    {
        AddEntityIRep(ss);

        Console.WriteLine("领域层 -- done");
    }

    private static void AddEntityIRep(Solution ss)
    {
        if (ss.mode != SolutionMode.Full)
            return;

        FileHelper.CreateDirectory(Path.Combine(ss.domain_project.entity_dir,"IRep"));
        
        var iRepDir = Path.Combine(ss.domain_project.entity_dir, "IRep");
        FileHelper.CreateDirectory(iRepDir);

        var iRepFilePath = Path.Combine(iRepDir, $"I{ss.entity_code}Rep.cs");
        FileHelper.CreateFileByTemplate(iRepFilePath, ss, "Domain/IRep/IEntityRep.txt");
    }


    #endregion
}