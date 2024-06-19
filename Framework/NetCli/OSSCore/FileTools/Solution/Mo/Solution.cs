using System.Data;
using System.IO;

namespace OSSCore;


internal class Solution
{
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

    public string base_path { get;  } = string.Empty;

    public string module_code { get; } = string.Empty;

    public string module_display { get; } = string.Empty;

    public string entity_code    { get; set; } = string.Empty;
    public string entity_display { get; set; } = string.Empty;

    public string solution_name { get;  } = string.Empty;

    public DBType db_type { get; set; } 



    public SolutionMode mode { get; set; }



    public ClientProject http_client_project { get; }

    public DomainProject domain_project { get; }

    public DomainOpenProject domain_open_project { get; }

    public ServiceProject service_project { get; }

    public RepProject rep_project { get; }

    public WebApiProject webapi_project { get; }
}


public class BaseProjectStructure
{
    public BaseProjectStructure(string projectName, string basePath,string entityName )
    {
        name = projectName;

        project_dir       = Path.Combine(basePath, name);
        common_dir        = Path.Combine(basePath, name, "Common");
        global_dir        = Path.Combine(basePath, name, "AppGlobal");
        project_file_path = Path.Combine(basePath, name, name + ".csproj");

        if (!string.IsNullOrEmpty(entityName))
        {
            entity_dir = Path.Combine(basePath, name, entityName);
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
    public string entity_name { get; set; }

    /// <summary>
    ///  领域对象文件目录
    /// </summary>
    public string entity_dir { get; set; }

    internal void ResetFrom(BaseProjectStructure source)
    {
        name = source.name;
        project_dir =   source.project_dir;
        common_dir = source.common_dir;

        global_dir = source.global_dir;
        entity_name = source.entity_name;
        entity_dir = source.entity_dir;
        project_file_path = source.project_file_path;
    }
}