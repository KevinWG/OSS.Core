namespace OSSCore;
internal class RepProject : BaseProjectStructure
{
    public RepProject(string solutionName, string moduleName, string basePath, string entityName = "") : base(
            string.Concat(solutionName, ".Repository"), basePath,entityName)
    {
        base_class_name    = string.Concat("Base", moduleName, "Rep");
        starter_class_name = string.Concat(moduleName, "RepStarter");
    }

    public string base_class_name { get; }

    public string starter_class_name { get; }
}