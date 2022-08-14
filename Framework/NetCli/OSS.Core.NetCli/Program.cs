
using System;
using System.IO;
using OSSCore;

internal class Program
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
        }

        ConsoleTips();
    }


    #region 创建解决方案

    

    #endregion

    private static void CreateSolution(string[] args)
    {
        var paras = GetCreateParas(args);

        if (string.IsNullOrEmpty(paras.module_name))
        {
            ConsoleTips();
            return;
        }

        var basePath = Directory.GetCurrentDirectory();
        var projectFiles = new SolutionStructure(paras, basePath);

        new SolutionFileTool().Create(projectFiles);
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