using CliDsl.Lib.Lexing;

namespace CliDsl.Lib.Parsing
{
    public class Parser
    {
        public AstParentCommand Parse(IEnumerable<LexerToken> tokens)
        {
            var tokenList = tokens.ToList();
            return ParseParentCommand(tokenList, 0, "root");
        }

        private AstParentCommand ParseParentCommand(List<LexerToken> tokens, int curr, string identifier)
        {
            var openBlockCount = 0;
            var closeBlockCount = 0;

            var summary = "";
            var arguments = new List<AstArgument>();
            var commands = new List<AstCommand>();
            for (var i = curr; i < tokens.Count && openBlockCount >= closeBlockCount; i++)
            {
                var token = tokens[i];
                switch (token.Type)
                {
                    case LexerTokenType.BlockStart:
                        openBlockCount++;
                        break;
                    case LexerTokenType.BlockEnd:
                        closeBlockCount++;
                        break;
                    case LexerTokenType.Summary:
                        summary = ParseSummary(tokens, i);
                        break;
                    case LexerTokenType.Argument:
                        arguments.Add(ParseArgument(tokens, i));
                        break;
                    case LexerTokenType.Command:
                        var isDirectChild = openBlockCount == closeBlockCount;
                        if (isDirectChild)
                        {
                            commands.Add(ParseCommand(tokens, i));
                        }
                        break;
                }
            }

            return new AstParentCommand(identifier, summary, commands, arguments);
        }

        private AstCommand ParseCommand(List<LexerToken> tokens, int curr)
        {
            if (tokens[curr].Type != LexerTokenType.Command)
            {
                throw new InvalidOperationException("Unexpected token type when parsing command");
            }
            var command = tokens[curr];
            var identifier = tokens[curr + 1].RawToken;
            var environmentName = tokens[curr + 2].RawToken;
            var environment = ToEnvironment(environmentName);

            AstCommand parsedCommand;
            if (environment == ScriptEnvironment.ParentCommand)
            {
                parsedCommand = ParseParentCommand(tokens, curr + 4, identifier);
            }
            else
            {
                var script = tokens[curr + 4].RawToken;
                parsedCommand = new AstScriptCommand(identifier, environment, script);
            }

            return parsedCommand;
        }

        private AstArgument ParseArgument(List<LexerToken> tokens, int curr)
        {
            if (tokens[curr].Type != LexerTokenType.Argument)
            {
                throw new InvalidOperationException("Unexpected token type when parsing argument");
            }
            var identifier = tokens[curr + 1].RawToken;
            var description = tokens[curr + 3].RawToken;

            return new AstArgument(identifier, description);
        }

        private string ParseSummary(List<LexerToken> tokens, int curr)
        {
            if (tokens[curr].Type != LexerTokenType.Summary)
            {
                throw new InvalidOperationException("Unexpected token type when parsing summary");
            }
            var summary = tokens[curr + 2].RawToken;

            return summary;
        }

        private static ScriptEnvironment ToEnvironment(string environmentName)
        {
            ScriptEnvironment environment;
            switch (environmentName)
            {
                case "cmds":
                    environment = ScriptEnvironment.ParentCommand;
                    break;
                case "cmdz":
                    environment = ScriptEnvironment.Commands;
                    break;
                case "sh":
                    environment = ScriptEnvironment.Sh;
                    break;
                case "bash":
                    environment = ScriptEnvironment.Bash;
                    break;
                case "ps":
                    environment = ScriptEnvironment.PowerShell;
                    break;
                case "bat":
                    environment = ScriptEnvironment.Batch;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown script environment: {environmentName}");
            }

            return environment;
        }
    }
}
