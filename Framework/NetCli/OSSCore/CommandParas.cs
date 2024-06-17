
namespace OSSCore;

internal class ModulePara: ParaItem
{
    public string solution_pre { get; set; }

    public SolutionMode solution_mode { get; set; } = SolutionMode.Default;
}

public enum SolutionMode
{
    Default=0,
    Simple =1,

    Simple_Plus =2,
}


public class ParaItem
{
    public string name { get; set; }

    public string display { get; set; }
}