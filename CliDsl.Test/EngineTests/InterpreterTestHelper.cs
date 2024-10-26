
using CliDsl.Lib.Parsing;

namespace CliDsl.Test.EngineTests
{
    public static class InterpreterTestHelper
    {
        public static (string Program, string[] Args, AstScriptCommand ExpectedCommand, List<string> ExpectedParameters) CreateSimpleCommand()
        {
            var program = @"
cmd something bash {
    echo hello
}
";
            string[] args = ["something"];

            var expectedCommand = new AstScriptCommand("something", ScriptEnvironment.Bash, "echo hello");
            var expectedParameters = new List<string>();

            return (program, args, expectedCommand, expectedParameters);
        }

        public static (string Program, string[] Args, AstScriptCommand ExpectedCommand, List<string> ExpectedParameters) CreateNestedCommand()
        {
            var program = @"
cmd parent cmds {
    cmd something bash {
        echo hello
    }
}
";
            string[] args = ["parent", "something"];

            var expectedCommand = new AstScriptCommand("something", ScriptEnvironment.Bash, "echo hello");
            var expectedParameters = new List<string>();

            return (program, args, expectedCommand, expectedParameters);
        }

        public static (string Program, string[] Args, AstScriptCommand ExpectedCommand, List<string> ExpectedParameters) CreateSelfCommand()
        {
            var program = @"
cmd self bash {
    echo hello
}
";
            string[] args = [];

            var expectedCommand = new AstScriptCommand("self", ScriptEnvironment.Bash, "echo hello");
            var expectedParameters = new List<string>();

            return (program, args, expectedCommand, expectedParameters);
        }
        public static (string Program, string[] Args, AstScriptCommand ExpectedCommand, List<string> ExpectedParameters) CreateNestedSelfCommand()
        {
            var program = @"
cmd parent cmds {
    cmd self bash {
        echo hello
    }
}
";
            string[] args = ["parent"];

            var expectedCommand = new AstScriptCommand("self", ScriptEnvironment.Bash, "echo hello");
            var expectedParameters = new List<string>();

            return (program, args, expectedCommand, expectedParameters);
        }

        public static (string Program, string[] Args, AstScriptCommand ExpectedCommand, List<string> ExpectedParameters) CreateCombinedCommand()
        {
            var program = @"
cmd something sh {
    echo hello
}

cmd somethingElse sh {
    echo helloo
}

cmd multi cmdz {
    something
    somethingElse
}
";
            string[] args = ["multi"];

            var expectedCommand = new AstScriptCommand("multi", ScriptEnvironment.Commands, "something\nsomethingElse");
            var expectedParameters = new List<string>();

            return (program, args, expectedCommand, expectedParameters);
        }
    }
}
