namespace OSSCore;
internal class RepProjectStructure : BaseProjectStructure
{
    public RepProjectStructure(string solutionName, string moduleName, string basePath) : base(
            string.Concat(solutionName, ".Repository"), basePath)
    {
        base_class_name    = string.Concat("Base", moduleName, "Rep");
        starter_class_name = string.Concat(moduleName, "RepStarter");
    }

    public string base_class_name { get; }

    public string starter_class_name { get; }
}