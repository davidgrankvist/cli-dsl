namespace CliDsl.Lib.Parsing
{
    public class AstArgument
    {
        public AstArgument(string name, string description)
        {
            Name = name; 
            Description = description;
        }
        public string Name { get; }

        public string Description { get; }

        public override bool Equals(object? obj)
        {
            if (obj is not AstArgument arg)
            {
                return false;
            }

            return Name == arg.Name && Description == arg.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description);
        }
    }
}
