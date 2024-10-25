﻿using System.Numerics;

namespace CliDsl.Lib.Parsing
{
    public class AstCommand
    {
        public AstCommand(string name, string environment)
        {
            Name = name;
            Environment = environment;
        }
        public string Name { get; }

        public string Environment { get; }

        public override bool Equals(object? obj)
        {
            if (obj is not AstCommand cmd)
            {
                return false;
            }

            return Name == cmd.Name && Environment == cmd.Environment;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Environment);
        }
    }
}
