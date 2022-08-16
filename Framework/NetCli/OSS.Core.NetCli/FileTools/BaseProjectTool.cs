
using System.Collections.Generic;
using System.Text;

namespace OSSCore
{
    internal abstract class BaseProjectTool
    {
        #region 创建方法
        
        public virtual void Create(SolutionStructure ss)
        {
            Create_Project(ss);
            Create_CommonFiles(ss);
            Create_GlobalFiles(ss);
        }


        public abstract void Create_Project(SolutionStructure ss);
        
        public virtual void Create_CommonFiles(SolutionStructure ss)
        {
        }
        public virtual void Create_GlobalFiles(SolutionStructure ss)
        {
        }

        #endregion


        #region 添加方法

        public virtual void AddEntity(SolutionStructure ss)
        {

        }

        #endregion


        protected static void CreateProjectFile(string filePath, List<string> packageRefs, List<string> projectRefs, bool isWeb = false)
        {
            var content = new StringBuilder();

            content.AppendLine(isWeb ? "<Project Sdk=\"Microsoft.NET.Sdk.Web\">" : "<Project Sdk=\"Microsoft.NET.Sdk\">");

            content.AppendLine(@"
     <PropertyGroup>
         <TargetFramework>net6.0</TargetFramework>
         <ImplicitUsings>enable</ImplicitUsings>
         <Nullable>enable</Nullable>
         <GenerateDocumentationFile>True</GenerateDocumentationFile>
     </PropertyGroup>");

            if (packageRefs != null && packageRefs.Count > 0)
            {
                content.AppendLine("    <ItemGroup>");
                foreach (var packageRef in packageRefs)
                {
                    content.AppendLine($"       <PackageReference Include=\"{packageRef}\" Version=\"*\"/>");
                }
                content.AppendLine("    </ItemGroup>");
            }

            if (projectRefs is { Count: > 0 })
            {
                content.AppendLine("    <ItemGroup>");
                foreach (var item in projectRefs)
                {
                    content.AppendLine($"       <ProjectReference Include=\"{item}\" />");
                }

                content.AppendLine("    </ItemGroup>");
            }

            content.AppendLine("</Project>");

           FileHelper.CreateFile(filePath, content.ToString());
        }
    }
}
