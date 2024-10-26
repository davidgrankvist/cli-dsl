
using CliDsl.Lib.Lexing;
using CliDsl.Lib.Parsing;

namespace CliDsl.Test.ParserTests
{
    public static class ParserTestHelper
    {
        public static (IEnumerable<LexerToken> ExpectedTokens, AstParentCommand ExpectedAst) CreateSimpleCommand()
        {
            var script = @"
echo hello
echo goodbye
";
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "something"),
                new LexerToken(LexerTokenType.ScriptType, "sh"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, script),
                new LexerToken(LexerTokenType.BlockEnd),
            };
            var ast = new AstParentCommand("root", "", [new AstScriptCommand("something", ScriptEnvironment.Sh, script)], []);


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
                    new AstScriptCommand("server", ScriptEnvironment.Sh, "echo server"),
                    new AstScriptCommand("client", ScriptEnvironment.Sh, "echo client"),
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
                new AstScriptCommand("self", ScriptEnvironment.Sh, "echo hello"),
            ], []);

            return (tokens, ast);
        }

        public static (IEnumerable<LexerToken> ExpectedTokens, AstParentCommand ExpectedAst) CreatedIndentedEmbeddedScript()
        {
            var script = @"
for (($i = 0); $i -lt 10; $i++)
{
    Write-Host $i
}
";
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "something"),
                new LexerToken(LexerTokenType.ScriptType, "ps"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, script),
                new LexerToken(LexerTokenType.BlockEnd),
            };
            var ast = new AstParentCommand("root", "", [
                new AstScriptCommand("something", ScriptEnvironment.PowerShell, script),    
            ], []);
            return (tokens, ast);
        }

        public static (IEnumerable<LexerToken> ExpectedTokens, AstParentCommand ExpectedAst) CreateCombinedCommand()
        {
            var tokens = new List<LexerToken>()
            {
                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "something"),
                new LexerToken(LexerTokenType.ScriptType, "sh"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, "echo hello"),
                new LexerToken(LexerTokenType.BlockEnd),

                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "somethingElse"),
                new LexerToken(LexerTokenType.ScriptType, "sh"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, "echo helloo"),
                new LexerToken(LexerTokenType.BlockEnd),

                new LexerToken(LexerTokenType.Command),
                new LexerToken(LexerTokenType.Identifier, "multi"),
                new LexerToken(LexerTokenType.ScriptType, "cmdz"),
                new LexerToken(LexerTokenType.BlockStart),
                    new LexerToken(LexerTokenType.Script, "something\nsomethingElse"),
                new LexerToken(LexerTokenType.BlockEnd),
            };
            var ast = new AstParentCommand("root", "", [
                new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello"),
                new AstScriptCommand("somethingElse", ScriptEnvironment.Sh, "echo helloo"),
                new AstScriptCommand("multi", ScriptEnvironment.Commands, "something\nsomethingElse"),
            ], []);
            return (tokens, ast);
        }
    }
}
