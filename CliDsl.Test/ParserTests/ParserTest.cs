using CliDsl.Lib.Lexing;
using CliDsl.Lib.Parsing;

namespace CliDsl.Test.ParserTests
{
    [TestClass]
    public class ParserTest
    {
        private Parser parser;

        [TestInitialize]
        public void TestInitialize()
        {
            parser = new Parser();
        }

        private void RunTest((IEnumerable<LexerToken> Tokens, AstParentCommand ExpectedAst) testCase)
        {
            var (tokens, expectedAst) = testCase;

            var ast = parser.Parse(tokens);

            Assert.AreEqual(expectedAst, ast);
        }

        [TestMethod]
        public void ShouldParseSimpleCommand()
        {
            RunTest(ParserTestHelper.CreateSimpleCommand());
        }

        [TestMethod]
        public void ShouldParseNestedCommands()
        {
            RunTest(ParserTestHelper.CreateNestedCommands());
        }

        [TestMethod]
        public void ShouldParseDocs()
        {
            RunTest(ParserTestHelper.CreateDocs());

        }

        [TestMethod]
        public void ShouldParseSelfCommand()
        {
            RunTest(ParserTestHelper.CreateSelfCommand());
        }

        [TestMethod]
        public void ShouldParseIndentedEmbeddedScript()
        {
            RunTest(ParserTestHelper.CreatedIndentedEmbeddedScript());
        }

        [TestMethod]
        public void ShouldParseCombinedCommand()
        {
            RunTest(ParserTestHelper.CreateCombinedCommand());
        }
    }
}