namespace CliDsl.Lib.Lexing
{
    public struct LexerToken
    {
        public LexerToken(LexerTokenType type)
        {
            Type = type;
        }

        public LexerToken(LexerTokenType type, string rawToken)
        {
            Type = type;
            RawToken = rawToken;
        }

        public LexerTokenType Type { get; }

        public string RawToken { get; }
    }
}