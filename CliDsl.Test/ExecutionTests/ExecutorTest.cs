using CliDsl.Lib.Execution;
using CliDsl.Lib.Parsing;
using CliDsl.Test.TestUtils;

namespace CliDsl.Test.ExecutionTests
{
    [TestClass]
    public class ExecutorTest
    {
        private Executor executor;
        private ScriptRunnerSpy scriptRunnerSpy;

        [TestInitialize]
        public void TestInitialize()
        {
            scriptRunnerSpy = new ScriptRunnerSpy();
            executor = new Executor(scriptRunnerSpy);
        }

        private void RunTest((AstParentCommand Ast, ExecutionArguments Args, AstScriptCommand ExpectedCommand) testCase)
        {
            RunTest((testCase.Ast, testCase.Args, [testCase.ExpectedCommand]));
        }

        private void RunTest((AstParentCommand Ast, ExecutionArguments Args, IEnumerable<AstScriptCommand> ExpectedCommands) testCase)
        {
            var (ast, args, expectedCommands) = testCase;
            var expectedCommandsArr = expectedCommands.ToArray();

            executor.Execute(ast, args);

            Assert.AreEqual(scriptRunnerSpy.Invocations.Count, expectedCommandsArr.Length);
            for (var i = 0;  i < expectedCommandsArr.Length; i++)
            {
                var command = expectedCommandsArr[i];

                var invocation = scriptRunnerSpy.Invocations[i];
                AssertExtensions.AreEqual(args.Parameters, invocation.Parameters);
                Assert.AreEqual(command, invocation.Command);
            }
        }

        [TestMethod]
        public void ShouldExecuteSimpleCommand()
        {
            RunTest(ExecutorTestHelper.CreateSimpleCommand());
        }

        [TestMethod]
        public void ShouldExecuteNestedCommand()
        {
            RunTest(ExecutorTestHelper.CreateNestedCommand());
        }

        [TestMethod]
        public void ShouldExecuteSelfCommand()
        {
            RunTest(ExecutorTestHelper.CreateSelfCommand());
        }

        [TestMethod]
        public void ShouldExecuteNestedSelfCommand()
        {
            RunTest(ExecutorTestHelper.CreateNestedSelfCommand());
        }

        [TestMethod]
        public void ShouldExecuteCombinedCommand()
        {
            RunTest(ExecutorTestHelper.CreateCombinedCommand());
        }

        [TestMethod]
        public void ShouldExecuteNestedCombinedCommand()
        {
            RunTest(ExecutorTestHelper.CreateNestedCombinedCommand());
        }

        [TestMethod]
        public void ShouldExecuteCombinedSelfCommand()
        {
            RunTest(ExecutorTestHelper.CreateCombinedSelfCommand());
        }

        [TestMethod]
        public void ShouldExecuteNestedCombinedSelfCommand()
        {
            RunTest(ExecutorTestHelper.CreateNestedCombinedSelfCommand());
        }
    }
}
