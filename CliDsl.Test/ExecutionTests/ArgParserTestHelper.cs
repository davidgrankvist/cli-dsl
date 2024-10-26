using CliDsl.Lib.Execution;

namespace CliDsl.Test.ExecutionTests
{
    public static class ArgParserTestHelper
    {
        public static (string[] Args, ExecutionArguments Expected) CreateSimplePath()
        {
            string[] args = ["a", "b", "c", "d"];
            var expected = new ExecutionArguments(args.ToList(), []);

            return (args, expected);
        }

        public static (string[] Args, ExecutionArguments Expected) CreatePathWithParameters()
        {
            string[] args = ["a", "b", "--verbose", "--someValue=5"];
            var expected = new ExecutionArguments([args[0], args[1]], [args[2], args[3]]);

            return (args, expected);
        }


        public static (string[] Args, ExecutionArguments Expected) CreatePathWithPositionalParameters()
        {
            string[] args = ["a", "--verbose", "someParam"];
            var expected = new ExecutionArguments([args[0]], [args[1], args[2]]);

            return (args, expected);
        }

        public static (string[] Args, ExecutionArguments Expected) CreatePathWithParameterEscape(int count)
        {
            var escapeStr = "";
            for (var i = 0; i < count; i++)
            {
                escapeStr += ArgParser.ParameterEscape;
            }
            string[] args = ["a", escapeStr, "someParam"];
            var expected = new ExecutionArguments([args[0]], [args[2]]);

            return (args, expected);
        }
    }
}
