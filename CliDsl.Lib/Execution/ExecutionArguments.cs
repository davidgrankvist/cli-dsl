namespace CliDsl.Lib.Execution
{
    public class ExecutionArguments
    {
        public ExecutionArguments(List<string> exeuctionPath, List<string> parameters)
        {
            ExecutionPath = exeuctionPath;
            Parameters = parameters;
        }

        public List<string> ExecutionPath { get; }

        public List<string > Parameters { get; }

        public override bool Equals(object? obj)
        {
            if (obj is not ExecutionArguments args)
            {
                return false;
            }
            
            return ExecutionPath.SequenceEqual(args.ExecutionPath) && Parameters.SequenceEqual(args.Parameters);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ExecutionPath.GetHashCode(), Parameters.GetHashCode());
        }
    }
}
