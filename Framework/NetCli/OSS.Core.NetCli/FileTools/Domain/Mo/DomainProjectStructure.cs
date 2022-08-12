
namespace OSSCore;
internal class DomainProjectStructure : BaseProjectStructure
{
    public DomainProjectStructure(string solutionName,string moduleName,string basePath) : base(
        string.Concat(solutionName, ".Domain"), basePath)
    {
        const_file_name = string.Concat(moduleName, "Const");
    }

    public string const_file_name { get; }
}