using CliDsl.App.SpecialCommands;
using CliDsl.Lib.Engine;

namespace CliDsl.App;

class Program
{
    static void Main(string[] args)
    {
        var specialCommandExecutor = new SpecialCommandExecutor();
        if(specialCommandExecutor.Execute(args))
        {
            return;
        }

        if (args.Length == 0)
        {
            throw new ArgumentException("Please provider a script file name");
        }

        var scriptRunner = new SubProcessScriptRunner();
        var interpreter = new Interpreter(scriptRunner);

        var fileName = args[0];
        if (!File.Exists(fileName))
        {
            throw new InvalidOperationException("File not found");
        }

        var scriptArgs = args.Where((_, i) => i != 0).ToArray();
        using (var reader = new StreamReader(fileName))
        {
            interpreter.Run(reader, scriptArgs);
        }
    }
}
