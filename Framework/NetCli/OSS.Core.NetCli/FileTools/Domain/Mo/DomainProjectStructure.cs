﻿
namespace OSSCore;
internal class DomainProjectStructure : BaseProjectStructure
{
   

    public DomainProjectStructure(string solutionName,string moduleName,string basePath) : base(
        string.Concat(solutionName, ".Domain"), basePath)
    {
        const_file_name = string.Concat(moduleName, "Const");
        starter_class_name = string.Concat(moduleName, "DomainStarter");
    }


    public string starter_class_name { get; }

    public string const_file_name { get; }
}