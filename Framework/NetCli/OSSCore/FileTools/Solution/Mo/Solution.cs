namespace OSSCore;

internal class Solution
{
    #region 初始化
    
    public Solution(ModulePara paras, string basePath, string entityName = "", string entityDisplay = "")
    {
        base_path = basePath;

        db_type        = paras.db_type;
        module_code    = paras.code;
        module_display = string.IsNullOrEmpty(paras.display) ? module_code : paras.display;

        entity_code    = entityName;
        entity_display = string.IsNullOrEmpty(entityDisplay) ? entityName : entityDisplay;

        solution_name = string.IsNullOrEmpty(paras.solution_pre)
            ? paras.code
            : string.Concat(paras.solution_pre, ".", paras.code);

        domain_open_project = new DomainOpenProject(solution_name, module_code, basePath, entityName);

        domain_project  = new DomainProject(solution_name, module_code, basePath, entityName);
        service_project = new ServiceProject(solution_name, module_code, basePath, entityName);
        rep_project     = new RepProject(solution_name, module_code, basePath, entityName);

        webapi_project      = new WebApiProject(solution_name, module_code, basePath, entityName);
        http_client_project = new ClientProject(solution_name, module_code, basePath, entityName);

        mode = paras.solution_mode;

        switch (mode)
        {
            case SolutionMode.Normal:
                // 一般模式，仓储文件直接放在 Domain 目录
                rep_project.ResetFrom(domain_project);
                break;
            case SolutionMode.Simple:
                // 极简模式下，仓储，逻辑，统一放置在Domain下
                rep_project.ResetFrom(domain_project);
                service_project.ResetFrom(domain_project);
                break;
        }
    }

    #endregion

    /// <summary>
    ///  基础路径
    /// </summary>
    public string base_path { get;  }

    /// <summary>
    ///  模块编码
    /// </summary>
    public string module_code { get; } 

    /// <summary>
    ///  模块显示名称
    /// </summary>
    public string module_display { get; }

    /// <summary>
    ///  实体领域编码
    /// </summary>
    public string entity_code    { get; set; } 

    /// <summary>
    ///  实体领域展示名称
    /// </summary>
    public string entity_display { get; set; } 

    /// <summary>
    ///  解决方案名称
    /// </summary>
    public string solution_name { get;  } 

    /// <summary>
    ///  数据库类型
    /// </summary>
    public DBType db_type { get; set; } 


    /// <summary>
    ///  解决方案模式
    /// </summary>
    public SolutionMode mode { get; set; }

    #region 子项目

    public ClientProject http_client_project { get; }

    public DomainProject domain_project { get; }

    public DomainOpenProject domain_open_project { get; }

    public ServiceProject service_project { get; }

    public RepProject rep_project { get; }

    public WebApiProject webapi_project { get; }

    #endregion
}


public class BaseProjectStructure
{
    /// <summary>
    ///  初始化
    /// </summary>
    /// <param name="projectName"></param>
    /// <param name="basePath"></param>
    /// <param name="entityCode"></param>
    public BaseProjectStructure(string projectName, string basePath,string entityCode )
    {
        name = projectName;

        project_dir       = Path.Combine(basePath, name);
        common_dir        = Path.Combine(basePath, name, "Common");
        global_dir        = Path.Combine(basePath, name, "AppGlobal");
        project_file_path = Path.Combine(basePath, name, name + ".csproj");

        entity_code=entityCode;

        if (!string.IsNullOrEmpty(entityCode))
        {
            entity_dir = Path.Combine(basePath, name, entityCode);
        }
    }

    public string name { get; set; }
    
    /// <summary>
    ///  项目文件夹
    /// </summary>
    public string project_dir { get; set; }
    
    /// <summary>
    ///  项目文件地址
    /// </summary>
    public string project_file_path { get; set; }


    /// <summary>
    ///  内部公用文件夹
    /// </summary>
    public string common_dir { get; set; }

    /// <summary>
    ///  全局文件夹
    /// </summary>
    public string global_dir { get; set; }

    /// <summary>
    ///  领域对象名称
    /// </summary>
    public string entity_code { get; set; }

    /// <summary>
    ///  领域对象文件目录
    /// </summary>
    public string entity_dir { get; set; } = string.Empty;

    internal void ResetFrom(BaseProjectStructure source)
    {
        name = source.name;
        project_dir =   source.project_dir;
        common_dir = source.common_dir;

        global_dir = source.global_dir;
        entity_code = source.entity_code;
        entity_dir = source.entity_dir;
        project_file_path = source.project_file_path;
    }
}