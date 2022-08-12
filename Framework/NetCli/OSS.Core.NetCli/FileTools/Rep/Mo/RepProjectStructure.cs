namespace OSSCore;
internal class RepProjectStructure : BaseProjectStructure
{
    public RepProjectStructure(string solutionName, string moduleName, string basePath) : base(
            string.Concat(solutionName, ".Repository"), basePath)
    {
        base_file_name    = string.Concat("Base", moduleName, "Rep");
        starter_file_name = string.Concat(moduleName, "RepStarter");
    }

    public string base_file_name { get; }

    public string starter_file_name { get; }
}