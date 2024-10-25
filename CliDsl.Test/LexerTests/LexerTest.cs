using CliDsl.Lib.Lexing;
using CliDsl.Test.TestUtils;

namespace CliDsl.Test.LexerTests;

[TestClass]
public class LexerTest
{
    private Lexer lexer;

    [TestInitialize]
    public void TestInitialize()
    {
        lexer = new Lexer();
    }

    private void RunTest((string Program, IEnumerable<LexerToken> ExpectedTokens) testCase)
    {
        var (program, expectedTokens) = testCase;

        var tokens = lexer.Tokenize(program);

        AssertExtensions.AreEqual(expectedTokens, tokens);
    }

    [TestMethod]
    public void ShouldTokenizeSimpleCommand()
    {
        RunTest(LexerTestHelper.CreateSimpleCommand());
    }

    [TestMethod]
    public void ShouldTokenizeNestedCommands()
    {
        RunTest(LexerTestHelper.CreateNestedCommands());
    }

    [TestMethod]
    public void ShouldTokenizeDocs()
    {
        RunTest(LexerTestHelper.CreateDocs());

    }

    [TestMethod]
    public void ShouldTokenizeSelfCommand()
    {
        RunTest(LexerTestHelper.CreateSelfCommand());
    }

    [TestMethod]
    public void ShouldTokenizeIndentedEmbeddedScript()
    {
        RunTest(LexerTestHelper.CreatedIndentedEmbeddedScript());
    }
}