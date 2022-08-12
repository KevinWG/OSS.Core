
namespace OSSCore
{
    internal abstract class BaseProjectTool
    {
        public void Create(SolutionStructure solution)
        {
            Create_Project(solution);
            Create_CommonFiles(solution);
            Create_GlobalFiles(solution);
        }

        public abstract void Create_Project(SolutionStructure solution);
        
        public virtual void Create_CommonFiles(SolutionStructure solution)
        {

        }
    
        public virtual void Create_GlobalFiles(SolutionStructure solution)
        {

        }
    }
}
