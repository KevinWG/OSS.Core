namespace OSSCore;

internal class ModulePara: ParaItem
{
    /// <summary>
    ///  解决方案前缀
    /// </summary>
    public string solution_pre { get; set; } = string.Empty;

    public SolutionMode solution_mode { get; set; } = SolutionMode.Normal;

    public DBType db_type { get; set; } = DBType.MySql;
}



public enum SolutionMode
{
    Full   = 0,

    Normal = 1,

    Simple = 2,
}

public enum DBType
{
    MySql = 0,

    SqlServer = 1
}


public class ParaItem
{
    /// <summary>
    ///  名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///  显示
    /// </summary>
    public string display { get; set; } = string.Empty;
}