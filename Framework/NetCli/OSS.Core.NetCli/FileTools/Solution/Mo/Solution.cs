﻿using System.IO;

namespace OSSCore;


internal class Solution
{
    public Solution(CreateParas paras, string basePath, string entityName = "")
    {
        base_path = basePath;
        module_name = paras.module_name;
        //solution_mode = paras.solution_mode;
        entity_name = entityName;

        no_rep_injection = paras.solution_mode == SolutionMode.Simple;

        solution_name = string.IsNullOrEmpty(paras.solution_pre)
            ? paras.module_name
            : string.Concat(paras.solution_pre, ".", paras.module_name);

        domain_project = new DomainProject(solution_name, module_name, basePath, entityName);
        domain_opened_project = new DomainOpenedProject(solution_name, module_name, basePath, entityName);

        service_project = new ServiceProject(solution_name, module_name, basePath, entityName);
        service_opened_project = new ServiceOpenedProject(solution_name, module_name, basePath, entityName);

        rep_project = new RepProject(solution_name, module_name, basePath, entityName);

        webapi_project = new WebApiProject(solution_name, module_name, basePath, entityName);

        http_client_project = new ClientProject(solution_name, module_name, basePath, entityName);
    }

    public string base_path { get;  }

    public string module_name { get; }

    public string entity_name { get; set; }

    public string solution_name { get;  }

    public bool no_rep_injection { get; set; }
    //public SolutionMode solution_mode { get; }

    public ClientProject http_client_project { get; }

    public DomainProject domain_project { get; }

    public DomainOpenedProject domain_opened_project { get; }

    public ServiceProject service_project { get; }

    public ServiceOpenedProject service_opened_project { get; }

    public RepProject rep_project { get; }

    public WebApiProject webapi_project { get; }

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