using CliDsl.Lib.Execution;
using CliDsl.Lib.Lexing;
using CliDsl.Lib.Parsing;

namespace CliDsl.Lib.Engine
{
    public class Interpreter
    {
        private readonly Lexer lexer;
        private readonly Parser parser;
        private readonly ArgParser argParser;
        private readonly Executor executor;

        public Interpreter(IScriptRunner scriptRunner)
        {
            lexer = new Lexer();
            parser = new Parser();
            argParser = new ArgParser();
            executor = new Executor(scriptRunner);
        }

        public void Run(TextReader reader, string[] args)
        {
            var executionArgs = argParser.Parse(args);
            var tokens = lexer.Tokenize(reader);
            var ast = parser.Parse(tokens);
            executor.Execute(ast, executionArgs);
        }
    }
}
