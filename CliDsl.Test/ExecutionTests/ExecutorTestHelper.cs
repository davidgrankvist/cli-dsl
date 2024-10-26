using CliDsl.Lib.Execution;
using CliDsl.Lib.Parsing;

namespace CliDsl.Test.ExecutionTests
{
    public static class ExecutorTestHelper
    {
        public static (AstParentCommand Ast, ExecutionArguments Args, AstScriptCommand ExpectedCommand) CreateSimpleCommand()
        {
            var target = new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello");

            var ast = new AstParentCommand("root", "", [
                target,
            ], []);
            var args = new ExecutionArguments(["something"], []);

            return (ast, args, target);
        }

        public static (AstParentCommand Ast, ExecutionArguments Args, AstScriptCommand ExpectedCommand) CreateNestedCommand()
        {
            var target = new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello");

            var ast = new AstParentCommand("root", "", [
                new AstParentCommand("parent", "", [
                    target,
                    ], []),
                target,
            ], []);
            var args = new ExecutionArguments(["parent", "something"], []);

            return (ast, args, target);
        }

        public static (AstParentCommand Ast, ExecutionArguments Args, AstScriptCommand ExpectedCommand) CreateSelfCommand()
        {
            var target = new AstScriptCommand("self", ScriptEnvironment.Sh, "echo hello");

            var ast = new AstParentCommand("root", "", [
                target,
            ], []);
            var args = new ExecutionArguments([], []);

            return (ast, args, target);
        }

        public static (AstParentCommand Ast, ExecutionArguments Args, AstScriptCommand ExpectedCommand) CreateNestedSelfCommand()
        {
            var target = new AstScriptCommand("self", ScriptEnvironment.Sh, "echo hello");

            var ast = new AstParentCommand("root", "", [
                new AstParentCommand("parent", "", [
                    target,
                    ], []),
                target,
            ], []);
            var args = new ExecutionArguments(["parent"], []);

            return (ast, args, target);
        }

        public static (AstParentCommand Ast, ExecutionArguments Args, IEnumerable<AstScriptCommand> ExpectedCommands) CreateCombinedCommand()
        {
            var ast = new AstParentCommand("root", "", [
                new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello"),
                new AstScriptCommand("somethingElse", ScriptEnvironment.Sh, "echo helloo"),
                new AstScriptCommand("multi", ScriptEnvironment.Commands, "something\nsomethingElse"),
            ], []);

            List<AstScriptCommand> expectedCommands = [
                new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello"),
                new AstScriptCommand("somethingElse", ScriptEnvironment.Sh, "echo helloo")
             ];
            var args = new ExecutionArguments(["multi"], []);

            return (ast, args, expectedCommands);
        }

        public static (AstParentCommand Ast, ExecutionArguments Args, IEnumerable<AstScriptCommand> ExpectedCommands) CreateNestedCombinedCommand()
        {
            var ast = new AstParentCommand("root", "", [
                new AstParentCommand("nested", "", [
                    new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello"),
                    new AstScriptCommand("somethingElse", ScriptEnvironment.Sh, "echo helloo"),
                    new AstScriptCommand("multi", ScriptEnvironment.Commands, "something\nsomethingElse"),
                ], [])
            ], []);

            List<AstScriptCommand> expectedCommands = [
                new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello"),
                new AstScriptCommand("somethingElse", ScriptEnvironment.Sh, "echo helloo")
             ];
            var args = new ExecutionArguments(["nested", "multi"], []);

            return (ast, args, expectedCommands);
        }

        public static (AstParentCommand Ast, ExecutionArguments Args, IEnumerable<AstScriptCommand> ExpectedCommands) CreateCombinedSelfCommand()
        {
            var ast = new AstParentCommand("root", "", [
                new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello"),
                new AstScriptCommand("somethingElse", ScriptEnvironment.Sh, "echo helloo"),
                new AstScriptCommand("self", ScriptEnvironment.Commands, "something\nsomethingElse"),
            ], []);

            List<AstScriptCommand> expectedCommands = [
                new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello"),
                new AstScriptCommand("somethingElse", ScriptEnvironment.Sh, "echo helloo")
             ];
            var args = new ExecutionArguments([], []);

            return (ast, args, expectedCommands);
        }

        public static (AstParentCommand Ast, ExecutionArguments Args, IEnumerable<AstScriptCommand> ExpectedCommands) CreateNestedCombinedSelfCommand()
        {
            var ast = new AstParentCommand("root", "", [
                new AstParentCommand("nested", "", [
                    new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello"),
                    new AstScriptCommand("somethingElse", ScriptEnvironment.Sh, "echo helloo"),
                    new AstScriptCommand("self", ScriptEnvironment.Commands, "something\nsomethingElse"),
                ], [])
            ], []);

            List<AstScriptCommand> expectedCommands = [
                new AstScriptCommand("something", ScriptEnvironment.Sh, "echo hello"),
                new AstScriptCommand("somethingElse", ScriptEnvironment.Sh, "echo helloo")
             ];
            var args = new ExecutionArguments(["nested"], []);

            return (ast, args, expectedCommands);
        }
    }
}
