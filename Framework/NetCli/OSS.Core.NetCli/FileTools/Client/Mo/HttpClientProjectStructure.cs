
namespace OSSCore;
internal class HttpClientProject : BaseProjectStructure
{
    public HttpClientProject(string solutionName,string moduleName,string basePath, string entityName = "") : base(
        string.Concat(solutionName, ".Client.Http"), basePath, entityName)
    {
    }

}