using CliDsl.Lib.Parsing;

namespace CliDsl.Lib.Execution
{
    public class Executor
    {
        private readonly IScriptRunner scriptRunner;

        public Executor(IScriptRunner scriptRunner)
        {
            this.scriptRunner = scriptRunner;
        }

        public void Execute(AstParentCommand ast, ExecutionArguments args)
        {
            var command = ResolvePath(ast, args.ExecutionPath);
            scriptRunner.Run(command, args.Parameters);
        }

        private AstScriptCommand ResolvePath(AstParentCommand ast, List<string> path)
        {
            AstCommand? current = ast;
            AstScriptCommand? result = null;
            for (var i = 0; i < path.Count; i++)
            {
                var commandName = path[i];
                var isLast = i == path.Count - 1;

                if (current is AstParentCommand parentCmd)
                {
                    var childCmd = parentCmd.Commands.Find(child => child.Name == commandName);
                    current = childCmd;
                    if (isLast && current is AstScriptCommand script)
                    {
                        result = script;
                    }
                }
                else if (current is AstScriptCommand scriptCmd)
                {
                    if (isLast)
                    {
                        result = scriptCmd;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unable to resolve path. Unknown command: {commandName}");
                }
            }

            if (current is AstParentCommand cmd)
            {
                AstScriptCommand? selfCommand = (AstScriptCommand?)cmd.Commands.Find(child => child is AstScriptCommand scriptChild && scriptChild.Name == "self");
                result = selfCommand;
            }

            if (result == null)
            {
                throw new InvalidOperationException($"Unable to resolve path: {path}");
            }

            return result;
        }
    }
}
