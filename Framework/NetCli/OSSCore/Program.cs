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
        --display=xxx, 指定模块名称
        --dbtype=SqlServer|MySql, 指定数据库类型，默认MySql

        --mode=normal|simple|full, 指定解决方案结构模型
              full： 接口层  服务层 领域层 仓储层 完全独立
            normal： 接口层  服务层 领域层（包含 仓储、领域）
            simple： 接口层  领域层      （包含 仓储、服务、领域）

osscore add entityName (创建领域对象名为entityName的各模块文件)

    可选参数：
        --display=xxx, 指定领域对象名称
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
            case "name":
            case "":
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
                    "full"   => SolutionMode.Full,
                    "simple" => SolutionMode.Simple,
                    _        => SolutionMode.Normal
                };
                break;
            case "dbtype":
                paras.db_type = paraDic.Value.ToLower() switch
                {
                    "sqlserver" => DBType.SqlServer,
                    _           => DBType.SqlServer
                };
                break;
            case "name":
            case "":
                paras.name = paraDic.Value;
                break;
        }
    }
    return paras;
}

static Dictionary<string, string> GetArgParaDictionary(string[] args)
{
    var paras = new Dictionary<string, string>();

    var curKey = string.Empty;

    for (var i = 1; i < args.Length; i++)
    {
        var arg = args[i].Trim();

        if (i==1 && !arg.StartsWith('-'))
        {
            curKey        = "name";
            paras[curKey] = arg;
            continue;
        }

        if (arg.StartsWith('-'))
        {
            var argStr      = args[i].Trim('-');
            var argStrSplit = argStr.Split('=', StringSplitOptions.RemoveEmptyEntries);

            curKey        = argStrSplit[0];

            if (argStrSplit.Length>1)
            {
                paras[curKey] = argStrSplit[1];
            }
            for (var j = 2; j < argStrSplit.Length; j++)
            {
                paras[curKey] += argStrSplit[1];
            }
        }
        else
        {
            paras[curKey] += arg;
        }
    }
    return paras;
}



#endregion