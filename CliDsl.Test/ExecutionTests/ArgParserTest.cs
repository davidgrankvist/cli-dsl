using CliDsl.Lib.Execution;

namespace CliDsl.Test.ExecutionTests
{
    [TestClass]
    public class ArgParserTest
    {
        private ArgParser parser;

        [TestInitialize]
        public void Initialize()
        {
            parser = new ArgParser();
        }

        private void RunTest((string[] Args, ExecutionArguments Expected) testCase)
        {
            var (args, expected) = testCase;

            var result = parser.Parse(args);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShouldParseSimplePath()
        {
            RunTest(ArgParserTestHelper.CreateSimplePath());
        }


        [TestMethod]
        public void ShouldParsePathWithParameters()
        {
            RunTest(ArgParserTestHelper.CreatePathWithParameters());
        }


        [TestMethod]
        public void ShouldParsePathWithPositionalParameters()
        {
            RunTest(ArgParserTestHelper.CreatePathWithPositionalParameters());
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void ShouldParsePathWithParameterEscape(int count)
        {
            RunTest(ArgParserTestHelper.CreatePathWithParameterEscape(count));
        }
    }
}
