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
            var (ast, args, expectedCommand) = testCase;

            executor.Execute(ast, args);

            Assert.AreEqual(scriptRunnerSpy.Invocations.Count, 1);

            var invocation = scriptRunnerSpy.Invocations.Single();
            AssertExtensions.AreEqual(args.Parameters, invocation.Parameters);
            Assert.AreEqual(expectedCommand, invocation.Command);
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
    }
}
