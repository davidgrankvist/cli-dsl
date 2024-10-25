using CliDsl.Lib.Lexing;

namespace CliDsl.Test.LexerTests
{
    public static class LexerTestHelper
    {
        public static (string Program, IEnumerable<LexerToken> ExpectedTokens) CreateSimpleCommand()
        {
            var program = @"
cmd something sh {
    echo hello
    echo goodbye
}
";
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "something"),
                new LexerToken(LexerTokenType.ScriptType, "sh"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, "echo hello\necho goodbye"),
                new LexerToken(LexerTokenType.BlockEnd),
            };

            return (program, tokens);
        }

        public static (string Program, IEnumerable<LexerToken> ExpectedTokens) CreateNestedCommands()
        {
            var program = @"
cmd build cmds {
   cmd server sh {
       echo server
   }

   cmd client sh {
       echo client
   }
}
";
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "build"),
                new LexerToken(LexerTokenType.ScriptType, "cmds"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Command),
                    new LexerToken(LexerTokenType.Identifier, "server"),
                    new LexerToken(LexerTokenType.ScriptType, "sh"),
                    new LexerToken(LexerTokenType.BlockStart),
                        new LexerToken(LexerTokenType.Script, "echo server"),
                    new LexerToken(LexerTokenType.BlockEnd),

                    new LexerToken(LexerTokenType.Command),
                    new LexerToken(LexerTokenType.Identifier, "client"),
                    new LexerToken(LexerTokenType.ScriptType, "sh"),
                    new LexerToken(LexerTokenType.BlockStart),
                        new LexerToken(LexerTokenType.Script, "echo client"),
                    new LexerToken(LexerTokenType.BlockEnd),
                new LexerToken(LexerTokenType.BlockEnd),
            };

            return (program, tokens);
        }

        public static (string Program, List<LexerToken> ExpectedTokens) CreateDocs()
        {
            var program = @"
summary {
    Some description.
}

arg someArg {
    Some parameter description.
}
";
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Summary),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Docs, "Some description."),
                new LexerToken(LexerTokenType.BlockEnd),

                new LexerToken(LexerTokenType.Argument),
                new LexerToken(LexerTokenType.Identifier, "someArg"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Docs, "Some parameter description."),
                new LexerToken(LexerTokenType.BlockEnd),
            };

            return (program, tokens);
        }

        public static (string Program, List<LexerToken> ExpectedTokens) CreateSelfCommand()
        {
            var program = @"
cmd self sh {
    echo hello
}
";
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Self),
                new LexerToken(LexerTokenType.ScriptType, "sh"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, "echo hello"),
                new LexerToken(LexerTokenType.BlockEnd),
            };

            return (program, tokens);
        }

        public static (string Program, IEnumerable<LexerToken> ExpectedTokens) CreatedIndentedEmbeddedScript()
        {
            var program = @"
cmd something madeUpLang {
    for thing in things
        echo thing.status
    end
}
";
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "something"),
                new LexerToken(LexerTokenType.ScriptType, "madeUpLang"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, "for thing in things\n    echo thing.status\nend"),
                new LexerToken(LexerTokenType.BlockEnd),
            };
            return (program, tokens);
        }
    }
}
