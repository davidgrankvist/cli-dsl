namespace CliDsl.Lib.Parsing
{
    public class AstScriptCommand : AstCommand
    {
        public AstScriptCommand(string name, string environment, string script)
            : base (name, environment)
        {
            Script = script;
        }

        public string Script { get; }

        public override bool Equals(object? obj)
        {
            if (obj is not AstScriptCommand cmd)
            {
                return false;
            }

            return base.Equals(obj) && Script == cmd.Script;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Script, base.GetHashCode());
        }
    }
}
