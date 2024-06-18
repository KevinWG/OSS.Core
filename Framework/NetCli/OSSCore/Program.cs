// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Fluid;
using OSSCore;

TemplateOptions.Default.MemberAccessStrategy = new UnsafeMemberAccessStrategy();

if (args.Length < 1)
{
    ConsoleTips();
    return;
}
DispatchCommand(args);

 static void DispatchCommand(string[] args)
{
    var commandName = args[0].ToLower();
    switch (commandName)
    {
        case "new":
            CreateSolution(args);
            break;
        case "add":
            AddEntity(args);
            break;
        default:
            ConsoleTips();
            break;
    }
}

#region 添加领域对象

 static void AddEntity(string[] args)
{
    var entityPara = GetAddEntityParas(args);

    if (string.IsNullOrEmpty(entityPara.name))
    {
        ConsoleTips();
        return;
    }

    var basePath = Directory.GetCurrentDirectory();

    var paras = GetParasFromFile(basePath);
    var ss = new Solution(paras, basePath, entityPara.name, entityPara.display);

    new SolutionTool().AddEntity(ss);
}






static ModulePara GetParasFromFile(string basePath)
{
    var jsonFilePath = Path.Combine(basePath, "module.core.json");
    if (!File.Exists(jsonFilePath))
    {
        throw new Exception("未能在当期目录找到模块创建记录，无法添加对象文件!");
    }

    var mJsonStr = FileHelper.LoadFile(jsonFilePath);
    return JsonSerializer.Deserialize<ModulePara>(mJsonStr);
}

#endregion

#region 创建解决方案

 static void CreateSolution(string[] args)
{
    var paras = GetCreateParas(args);

    if (string.IsNullOrEmpty(paras.name))
    {
        ConsoleTips();
        return;
    }

    var basePath = Directory.GetCurrentDirectory();
    var ss = new Solution(paras, basePath);

    new SolutionTool().Create(ss);

    var moduleJsonPath = Path.Combine(basePath, "module.core.json");
    FileHelper.CreateFile(moduleJsonPath, JsonSerializer.Serialize(paras));
}

#endregion

 static void ConsoleTips()
{
    var commandStr =
        @"
可执行指令：
osscore new moduleA （创建名称为 moduleA 的模块解决方案）

    可选参数提示：
        --pre=xxx, 指定解决方案前缀
        --mode=simple|default, 指定解决方案结构
            simple： 没有独立的仓储类库（和领域对象类库合并）
            default：独立仓储层

osscore add entityName (创建领域对象名为entityName的各模块文件)
";

    Console.WriteLine(commandStr);
}

#region 参数处理

static ParaItem GetAddEntityParas(string[] args)
{
    var paras = new ParaItem();
    var paraDics = GetArgParaDictionary(args);

    foreach (var paraDic in paraDics)
    {
        switch (paraDic.Key)
        {
            case "display":
                paras.display = paraDic.Value;
                break;
            default:
                paras.name = paraDic.Value;
                break;
        }
    }
    return paras;
}

static ModulePara GetCreateParas(string[] args)
{
    var paras    = new ModulePara();
    var paraDics = GetArgParaDictionary(args);

    foreach (var paraDic in paraDics)
    {
        switch (paraDic.Key)
        {
            case "pre":
                paras.solution_pre = paraDic.Value;
                break;
            case "display":
                paras.display = paraDic.Value;
                break;
            case "mode":
                paras.solution_mode = paraDic.Value.ToLower() switch
                {
                    "simple"      => SolutionMode.Normal,
                    "simple_plus" => SolutionMode.Simple,
                    _             => SolutionMode.Full
                };
                break;
            case "dbtype":
                paras.db_type = paraDic.Value.ToLower() switch
                {
                    "sqlserver" => DBType.SqlServer,
                    _           => DBType.SqlServer
                };
                break;
            default:
                paras.name = paraDic.Value;
                break;
        }
    }
    return paras;
}

static Dictionary<string, string> GetArgParaDictionary(string[] args)
{
    //var name  = string.Empty;
    var paras = new Dictionary<string, string>();

    for (var i = 1; i < args.Length; i++)
    {
        var pStr = args[i].Trim('-').Trim(' ');

        var pStrSplit = pStr.Split('=', StringSplitOptions.RemoveEmptyEntries);
        if (pStrSplit.Length == 1)
        {
            paras[""] = pStrSplit[0];
            continue;
        }

        paras.Add(pStrSplit[0], pStrSplit[1]);
    }

    //paras.Add("", name);
    return paras;
}



#endregion