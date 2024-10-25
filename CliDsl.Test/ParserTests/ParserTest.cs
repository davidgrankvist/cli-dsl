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
        public void ShouldTokenizeSimpleCommand()
        {
            RunTest(ParserTestHelper.CreateSimpleCommand());
        }

        [TestMethod]
        public void ShouldTokenizeNestedCommands()
        {
            RunTest(ParserTestHelper.CreateNestedCommands());
        }

        [TestMethod]
        public void ShouldTokenizeDocs()
        {
            RunTest(ParserTestHelper.CreateDocs());

        }

        [TestMethod]
        public void ShouldTokenizeSelfCommand()
        {
            RunTest(ParserTestHelper.CreateSelfCommand());
        }

        [TestMethod]
        public void ShouldTokenizeIndentedEmbeddedScript()
        {
            RunTest(ParserTestHelper.CreatedIndentedEmbeddedScript());
        }
    }
}