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
            RunTest((testCase.Program, testCase.Args, [testCase.ExpectedCommand], testCase.ExpectedParameters));
        }

        private void RunTest((string Program, string[] Args, IEnumerable<AstScriptCommand> ExpectedCommands, List<string> ExpectedParameters) testCase)
        {
            var (program, args, expectedCommands, expectedParams) = testCase;
            var expectedCommandsArr = expectedCommands.ToArray();

            interpreter.Run(program, args);

            Assert.AreEqual(scriptRunnerSpy.Invocations.Count, expectedCommandsArr.Length);
            for (var i = 0; i < expectedCommandsArr.Length; i++)
            {
                var cmd = expectedCommandsArr[i];
                var invocation = scriptRunnerSpy.Invocations[i];

                AssertExtensions.AreEqual(expectedParams, invocation.Parameters);
                Assert.AreEqual(cmd, invocation.Command);
            }
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

        [TestMethod]
        public void ShouldRunCombinedCommand()
        {
            RunTest(InterpreterTestHelper.CreateCombinedCommand());
        }

        [TestMethod]
        public void ShouldRunNestedCombinedCommand()
        {
            RunTest(InterpreterTestHelper.CreateNestedCombinedCommand());
        }


        [TestMethod]
        public void ShouldRunNestedCombinedSelfCommand()
        {
            RunTest(InterpreterTestHelper.CreateNestedCombinedSelfCommand());
        }
    }
}
