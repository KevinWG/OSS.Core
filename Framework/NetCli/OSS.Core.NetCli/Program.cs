
using System;
using System.IO;
using OSS.Core.NetCli;

internal class Program
{
    static void Main(string[] args)
    {
        var paras = GetParas(args);
        if (string.IsNullOrEmpty(paras.module_name))
        {
            ConsoleTips();
            return;
        }

        CreateFiles(paras);
        Console.WriteLine("创建完成!");
    }



    public static void CreateFiles(CommandParas pFiles)
    {
        var basePath    = Directory.GetCurrentDirectory();
        var projectFiles = new SolutionStructure(pFiles, basePath);

        new SolutionFileTool().Create(projectFiles);
    }

    private static CommandParas GetParas(string[] args)
    {
        var paras = new CommandParas();
        if (args == null || args.Length == 0)
            return paras;

        foreach (var p in args)
        {
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
    
    private static void ConsoleTips()
    {
        Console.WriteLine("执行指令：osscore moduleA （创建名称为 moduleA 的模块解决方案）");
        Console.WriteLine("可选参数提示：");
        Console.WriteLine("  --pre=xxx, 指定解决方案前缀");
        Console.WriteLine("  --mode=simple|default, 指定解决方案结构");
        Console.WriteLine("     simple： 没有独立的仓储类库");
        Console.WriteLine("     default：独立仓储层");
    }
}