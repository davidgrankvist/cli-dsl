namespace CliDsl.Lib.Lexing;

public class Lexer
{
    private readonly List<LexerToken> tokens = [];

    private LexerToken preBlockToken;

    public IEnumerable<LexerToken> Tokenize(TextReader reader)
    {
        tokens.Clear();

        string line;
        var elex = new EmbeddedStringLexer();
        while ((line = reader.ReadLine()) != null)
        {
            var rawTokens = line.Split().Where(s => !string.IsNullOrEmpty(s));
            if (!rawTokens.Any())
            {
                continue;
            }

            var tokenPeek = ToToken(rawTokens.First());
            var charPeek = reader.Peek();
            if (elex.Process(line, tokenPeek, charPeek, tokens))
            {
                continue;
            }

            tokens.AddRange(rawTokens.Select(ToToken));
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

    private static string Dedent(string s, int n)
    {
        int count = 0;

        foreach (var c in s.ToCharArray())
        {
            if (count == n || !char.IsWhiteSpace(c))
            {
                break;
            }
            count++;
        }
        return s.Substring(count);
    }

    /// <summary>
    /// Parse embedded scripts or docs strings as a single token.
    /// Dedents the string based on the indentation of the first line.
    /// </summary>
    private class EmbeddedStringLexer
    {
        private LexerTokenType embeddedType = LexerTokenType.ScriptType;
        private List<string> embeddedLines = [];
        private int dedent = 0;

        public bool Process(string line, LexerToken tokenPeek, int charPeek, List<LexerToken> tokens)
        {
            var shouldContinue = false;
            if (tokenPeek.Type == LexerTokenType.Script || tokenPeek.Type == LexerTokenType.Docs)
            {
                var isFirst = embeddedLines.Count == 0;
                string t;
                if (isFirst)
                {
                    t = line.TrimStart();
                    dedent = line.Length - t.Length;
                }
                else
                {
                    t = Dedent(line, dedent);
                }
                embeddedType = tokenPeek.Type;
                embeddedLines.Add(t);
                shouldContinue = true;
            }
            else if (embeddedLines.Count != 0)
            {
                var p = charPeek;
                var newLine = p == '\r' ? "\r\n" : "\n";
                var embedded = string.Join(newLine, embeddedLines);
                var embeddedToken = new LexerToken(embeddedType, embedded);
                tokens.Add(embeddedToken);

                embeddedLines.Clear();
            }

            return shouldContinue;
        }
    }
}
