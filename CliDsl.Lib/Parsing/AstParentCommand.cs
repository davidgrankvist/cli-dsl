namespace CliDsl.Lib.Parsing
{
    public class AstParentCommand : AstCommand
    {
        public AstParentCommand(string name, string summary, List<AstCommand> commands, List<AstArgument> arguments)
            : base(name, ScriptEnvironment.ParentCommand)
        {
            Summary = summary;
            Commands = commands;
            Arguments = arguments;
        }

        public List<AstCommand> Commands { get; }

        public List<AstArgument> Arguments { get; }

        public string? Summary { get; }

        public override bool Equals(object? obj)
        {
            if (obj is not AstParentCommand cmd)
            {
                return false;
            }

            return base.Equals(obj) && Summary == cmd.Summary && Commands.SequenceEqual(cmd.Commands) && Arguments.SequenceEqual(cmd.Arguments);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Summary, Commands, Arguments, base.GetHashCode());
        }
    }
}
