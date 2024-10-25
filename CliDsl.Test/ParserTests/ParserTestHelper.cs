
using CliDsl.Lib.Lexing;
using CliDsl.Lib.Parsing;

namespace CliDsl.Test.ParserTests
{
    public static class ParserTestHelper
    {
        public static (IEnumerable<LexerToken> ExpectedTokens, AstParentCommand ExpectedAst) CreateSimpleCommand()
        {
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "something"),
                new LexerToken(LexerTokenType.ScriptType, "sh"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, "echo hello\necho goodbye"),
                new LexerToken(LexerTokenType.BlockEnd),
            };
            var ast = new AstParentCommand("root", "", [new AstScriptCommand("something", "sh", "echo hello\necho goodbye")], []);


            return (tokens, ast);
        }

        public static (IEnumerable<LexerToken> ExpectedTokens, AstParentCommand ExpectedAst) CreateNestedCommands()
        {
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
            var ast = new AstParentCommand("root", "", [
                new AstParentCommand("build", "", [
                    new AstScriptCommand("server", "sh", "echo server"),
                    new AstScriptCommand("client", "sh", "echo client"),
                ], []),
            ], []);

            return (tokens, ast);
        }

        public static (IEnumerable<LexerToken> ExpectedTokens, AstParentCommand ExpectedAst) CreateDocs()
        {
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
            var ast = new AstParentCommand("root", "Some description.", [], [
                new AstArgument("someArg", "Some parameter description."),
             ]);

            return (tokens, ast);
        }

        public static (IEnumerable<LexerToken> ExpectedTokens, AstParentCommand ExpectedAst) CreateSelfCommand()
        {
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Self, "self"),
                new LexerToken(LexerTokenType.ScriptType, "sh"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, "echo hello"),
                new LexerToken(LexerTokenType.BlockEnd),
            };
            var ast = new AstParentCommand("root", "", [
                new AstScriptCommand("self", "sh", "echo hello"),
            ], []);

            return (tokens, ast);
        }

        public static (IEnumerable<LexerToken> ExpectedTokens, AstParentCommand ExpectedAst) CreatedIndentedEmbeddedScript()
        {
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "something"),
                new LexerToken(LexerTokenType.ScriptType, "madeUpLang"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, "for thing in things\n    echo thing.status\nend"),
                new LexerToken(LexerTokenType.BlockEnd),
            };
            var ast = new AstParentCommand("root", "", [
                new AstScriptCommand("something", "madeUpLang", "for thing in things\n    echo thing.status\nend"),    
            ], []);
            return (tokens, ast);
        }
    }
}
