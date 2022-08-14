
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

        public virtual void AddEntity(SolutionStructure ss, string entityName)
        {

        }

        #endregion
    }
}
