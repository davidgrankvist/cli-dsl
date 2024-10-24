using CliDsl.Lib.Lexing;

namespace CliDsl.Test.LexerTests
{
    internal static class LexerTestExtensions
    {
        public static IEnumerable<LexerToken> Tokenize(this Lexer lexer, string program)
        {
            IEnumerable<LexerToken> tokens;
            using (var reader = new StringReader(program))
            {
                tokens = lexer.Tokenize(reader);
            }

            return tokens;
        }
    }
}
