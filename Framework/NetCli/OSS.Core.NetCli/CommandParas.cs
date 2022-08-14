
internal class Command
{
    public string name { get; set; }

    public CreateParas create_paras { get; set; }

}



internal class CreateParas
{
    public string module_name { get; set; }

    public string solution_pre { get; set; }

    public SolutionMode solution_mode { get; set; } = SolutionMode.Default;
}

public enum SolutionMode
{
    Default,
    Simple
}