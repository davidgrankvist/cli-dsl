using CliDsl.Lib.Parsing;

namespace CliDsl.Lib.Execution
{
    public interface IScriptRunner
    {
        public void Run(AstScriptCommand command, List<string> parameters);
    }
}
