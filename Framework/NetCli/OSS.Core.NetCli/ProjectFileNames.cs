internal class ProjectFileNames
{
    public ProjectFileNames(ModuleParas paras,string basePath)
    {
        base_path     = basePath;
        solution_name = string.IsNullOrEmpty(paras.solution_pre)
            ? paras.module_name:string.Concat(paras.solution_pre, ".", paras.module_name);

        domain_project_name        = string.Concat(solution_name, ".Domain");
        domain_opened_project_name = string.Concat(domain_project_name, ".Opened");

        service_project_name        = string.Concat(solution_name, ".Service");
        service_opened_project_name = string.Concat(service_project_name, ".Opened");

        repository_project_name = string.Concat(solution_name, ".Repository");
        webapi_project_name     = string.Concat(solution_name, ".WebApi");
    }

    public string base_path { get;  }

    public string solution_name { get;  }

    public string domain_project_name { get; }

    public string domain_opened_project_name { get; }

    public string service_project_name { get; }

    public string service_opened_project_name { get; }

    public string repository_project_name { get; }

    public string webapi_project_name { get; }

}