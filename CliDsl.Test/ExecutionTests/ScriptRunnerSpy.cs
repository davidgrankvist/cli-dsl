
using CliDsl.Lib.Execution;
using CliDsl.Lib.Parsing;

namespace CliDsl.Test.ExecutionTests
{
    internal class ScriptRunnerSpy : IScriptRunner
    {
        public List<(AstScriptCommand Command, List<string> Parameters)> Invocations { get; } = [];

        public void Run(AstScriptCommand command, List<string> parameters)
        {
            Invocations.Add((command, parameters));
        }
    }
}
