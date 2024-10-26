using CliDsl.Lib.Engine;
using CliDsl.Lib.Parsing;
using CliDsl.Test.ExecutionTests;
using CliDsl.Test.TestUtils;

namespace CliDsl.Test.EngineTests
{
    [TestClass]
    public class InterpreterTest
    {
        private Interpreter interpreter;
        private ScriptRunnerSpy scriptRunnerSpy;

        [TestInitialize]
        public void TestInitialize()
        {
            scriptRunnerSpy = new ScriptRunnerSpy();
            interpreter = new Interpreter(scriptRunnerSpy);
        }

        private void RunTest((string Program, string[] Args, AstScriptCommand ExpectedCommand, List<string> ExpectedParameters) testCase)
        {
            var (program, args, expectedCommand, expectedParams) = testCase;

            using (var reader = new StringReader(program))
            {
                interpreter.Run(reader, args);
            }

            Assert.AreEqual(scriptRunnerSpy.Invocations.Count, 1);
            var invocation = scriptRunnerSpy.Invocations.Single();
            AssertExtensions.AreEqual(expectedParams, invocation.Parameters);
            Assert.AreEqual(expectedCommand, invocation.Command);
        }

        [TestMethod]
        public void ShouldRunSimpleCommand()
        {
            RunTest(InterpreterTestHelper.CreateSimpleCommand());
        }

        [TestMethod]
        public void ShouldRunNestedCommand()
        {
            RunTest(InterpreterTestHelper.CreateNestedCommand());
        }

        [TestMethod]
        public void ShouldRunSelfCommand()
        {
            RunTest(InterpreterTestHelper.CreateSelfCommand());
        }

        [TestMethod]
        public void ShouldRunNestedSelfCommand()
        {
            RunTest(InterpreterTestHelper.CreateNestedSelfCommand());
        }
    }
}
