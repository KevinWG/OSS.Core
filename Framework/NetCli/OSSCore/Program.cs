// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Fluid;
using OSSCore;

TemplateOptions.Default.MemberAccessStrategy = new UnsafeMemberAccessStrategy();


if (args == null || args.Length < 1)
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


 static ParaItem GetAddEntityParas(string[] args)
{
    var paras = new ParaItem();

    for (var i = 1; i < args.Length; i++)
    {
        var p = args[i];
        if (p.StartsWith("--display="))
        {
            paras.display = p.Replace("--display=", "").TrimEnd();
        }
        else
        {
            paras.name = p;
        }
    }
    return paras;
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

 static ModulePara GetCreateParas(string[] args)
{
    var paras = new ModulePara();

    for (var i = 1; i < args.Length; i++)
    {
        var p = args[i];

        if (p.StartsWith("--pre="))
        {
            paras.solution_pre = p.Replace("--pre=", "").Trim();
        }
        else if (p.StartsWith("--mode="))
        {
            var mode = p.Replace("--mode=", "").Trim().ToLower();
            if (mode == "simple")
            {
                paras.solution_mode = SolutionMode.Simple;
            }
            else if (mode == "simple_plus")
            {
                paras.solution_mode = SolutionMode.Simple_Plus;
            }
            else
            {
                paras.solution_mode = SolutionMode.Default;
            }
        }
        else if (p.StartsWith("--display="))
        {
            paras.display = p.Replace("--display=", "").Trim();
        }
        else
        {
            paras.name = p;
        }
    }

    return paras;
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