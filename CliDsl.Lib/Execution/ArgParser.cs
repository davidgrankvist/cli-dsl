namespace CliDsl.Lib.Execution
{
    public class ArgParser
    {
        public const char ParameterEscape = '-';

        public ExecutionArguments Parse(string[] args)
        {
            List<string> path = [];
            List<string> parameters = [];
            var isPath = true;

            foreach (var arg in args)
            {
                if (isPath && arg.StartsWith("-"))
                {
                    isPath = false;
                }

                if (isPath)
                {
                    path.Add(arg);
                }
                else if (!IsParameterEscape(arg))
                {
                    parameters.Add(arg);
                }
            }

            return new ExecutionArguments(path, parameters);
        }

        private static bool IsParameterEscape(string arg)
        {
            var isEscape = true;
            foreach (var c in arg.ToCharArray())
            {
                if (c != ParameterEscape)
                {
                    isEscape = false;
                }
            }

            return isEscape;
        }
    }
}
