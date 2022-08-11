internal class ModuleParas
{
    public string module_name { get; set; }

    public string solution_pre { get; set; }

    public SolutionMode solution_mode { get; set; }
}

public enum SolutionMode
{
    Default,
    Simple
}