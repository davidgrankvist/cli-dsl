
namespace CliDsl.Lib.Lexing;

public class Lexer
{
    private readonly List<LexerToken> tokens = [];

    private LexerToken preBlockToken;

    public IEnumerable<LexerToken> Tokenize(TextReader reader)
    {
        tokens.Clear();

        var program = reader.ReadToEnd();
        // TODO(improvement): it would be better to maintain the whole lines for embedded scripts to keep context
        var rawTokens = program.Split().Where(s => !string.IsNullOrEmpty(s));

        foreach (var rawToken in rawTokens)
        {
            var token = ToToken(rawToken);
            tokens.Add(token);
        }

        return tokens;
    }

    private LexerToken ToToken(string rawToken)
    {
        LexerToken token;
        switch (rawToken)
        {
            case "cmd":
                token = new LexerToken(LexerTokenType.Command);
                break;
            case "self":
                token = new LexerToken(LexerTokenType.Self);
                break;
            case "summary":
                token = new LexerToken(LexerTokenType.Summary);
                break;
            case "arg":
                token = new LexerToken(LexerTokenType.Argument);
                break;
            case "{":
                token = new LexerToken(LexerTokenType.BlockStart);
                if (tokens.Count > 0)
                {
                    preBlockToken = tokens.Last();
                }
                break;
            case "}":
                token = new LexerToken(LexerTokenType.BlockEnd);
                break;
            default:
                token = ToContextDependentToken(rawToken);
                break;
        }

        return token;
    }

    private LexerToken ToContextDependentToken(string rawToken)
    {
        LexerToken token;

        var prevToken = tokens.Last();
        switch (prevToken.Type)
        {
            case LexerTokenType.Command:
            case LexerTokenType.Argument:
                token = new LexerToken(LexerTokenType.Identifier, rawToken);
                break;
            case LexerTokenType.Self:
            case LexerTokenType.Identifier:
                token = new LexerToken(LexerTokenType.ScriptType, rawToken);
                break;
            default:
                token = ToBlockContent(rawToken);
                break;
        }

        return token;
    }

    private LexerToken ToBlockContent(string rawToken)
    {
        LexerToken token;
        switch (preBlockToken.Type)
        {
            case LexerTokenType.ScriptType:
                token = new LexerToken(LexerTokenType.Script, rawToken);
                break;
            default:
                token = new LexerToken(LexerTokenType.Docs, rawToken);
                break;
        }

        return token;
    }
}
