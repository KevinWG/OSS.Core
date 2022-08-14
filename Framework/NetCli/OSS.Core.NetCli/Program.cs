
using OSSCore;
using System;
using System.IO;
using System.Text.Json;

public class Program
{
    static void Main(string[] args)
    {
        if (args==null || args.Length <1)
        {
            ConsoleTips();
            return;
        }

        DispatchCommand(args);
        
    }
    

    private static void DispatchCommand(string[] args)
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

    private static void AddEntity(string[] args)
    {
        var entityName = args[1];
        if (string.IsNullOrEmpty(entityName))
        {
            ConsoleTips();
            return;
        }


        var basePath = Directory.GetCurrentDirectory();

        var paras = GetParasFromFile(basePath);
        var ss = new SolutionStructure(paras, basePath,entityName);
        
        new SolutionFileTool().AddEntity(ss);
    }

    private static CreateParas GetParasFromFile(string basePath)
    {
        var jsonFilePath = Path.Combine(basePath, module_json_file_name);
        if (!File.Exists(jsonFilePath))
        {
            throw new Exception("未能在当期目录找到模块创建记录，无法添加对象文件!");
        }

        var mJsonStr = FileHelper.LoadFile(jsonFilePath);
        
        return JsonSerializer.Deserialize<CreateParas>(mJsonStr);
    }



    #endregion

    private const string module_json_file_name = "module.core.json";


    #region 创建解决方案


    private static void CreateSolution(string[] args)
    {
        var paras = GetCreateParas(args);

        if (string.IsNullOrEmpty(paras.module_name))
        {
            ConsoleTips();
            return;
        }

        var basePath = Directory.GetCurrentDirectory();
        var ss = new SolutionStructure(paras, basePath);

        new SolutionFileTool().Create(ss);

        var moduleJsonPath = Path.Combine(basePath, module_json_file_name);
        FileHelper.CreateFile(moduleJsonPath, JsonSerializer.Serialize(paras));
    }
    
    
    private static CreateParas GetCreateParas(string[] args)
    {
        var paras = new CreateParas();

        for (var i=1;i<args.Length;i++)
        {
            var p= args[i];

            if (p.StartsWith("--pre="))
            {
                paras.solution_pre = p.Replace("--pre=", "").TrimEnd();
            }
            else if (p.StartsWith("--mode="))
            {
                var mode= p.Replace("--mode=", "").TrimEnd();
                paras.solution_mode = mode == "simple" ? SolutionMode.Simple : SolutionMode.Default;
            }
            else
            {
                paras.module_name = p;
            }
        }
        return paras;
    }


    #endregion

    private static void ConsoleTips()
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
}