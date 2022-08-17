using System.IO;

namespace OSSCore;


internal class SolutionStructure
{
    public SolutionStructure(CreateParas paras, string basePath, string entityName = "")
    {
        base_path = basePath;
        module_name = paras.module_name;
        solution_mode = paras.solution_mode;
        entity_name = entityName;

        solution_name = string.IsNullOrEmpty(paras.solution_pre)
            ? paras.module_name
            : string.Concat(paras.solution_pre, ".", paras.module_name);

        domain_project = new DomainProjectStructure(solution_name, module_name, basePath, entityName);
        domain_opened_project = new DomainOpenedProjectStructure(solution_name, module_name, basePath, entityName);

        service_project = new ServiceProjectStructure(solution_name, module_name, basePath, entityName);
        service_opened_project = new ServiceOpenedProjectStructure(solution_name, module_name, basePath, entityName);

        rep_project = new RepProjectStructure(solution_name, module_name, basePath, entityName);

        webapi_project = new WebApiProjectStructure(solution_name, module_name, basePath, entityName);

        http_client_project = new HttpClientProject(solution_name, module_name, basePath, entityName);
    }

    public string base_path { get;  }

    public string module_name { get; }

    public string entity_name { get; set; }

    public string solution_name { get;  }

    public SolutionMode solution_mode { get; }

    public HttpClientProject http_client_project { get; }

    public DomainProjectStructure domain_project { get; }

    public DomainOpenedProjectStructure domain_opened_project { get; }

    public ServiceProjectStructure service_project { get; }

    public ServiceOpenedProjectStructure service_opened_project { get; }

    public RepProjectStructure rep_project { get; }

    public WebApiProjectStructure webapi_project { get; }

}


public class BaseProjectStructure
{
    public BaseProjectStructure(string projectName, string basePath,string entityName )
    {
        name = projectName;

        project_dir = Path.Combine(basePath, name);
        common_dir  = Path.Combine(project_dir, "Common");
        global_dir  = Path.Combine(project_dir, "AppGlobal");

        project_file_path = Path.Combine(project_dir, name + ".csproj");

        if (!string.IsNullOrEmpty(entityName))
        {
            entity_dir = Path.Combine(project_dir, entityName);
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




    public string entity_name { get; set; }
    public string entity_dir { get; set; }
}