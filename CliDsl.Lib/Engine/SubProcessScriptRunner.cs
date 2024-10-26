using System.Diagnostics;

using CliDsl.Lib.Execution;
using CliDsl.Lib.Parsing;

namespace CliDsl.Lib.Engine
{
    public class SubProcessScriptRunner : IScriptRunner
    {
        public void Run(AstScriptCommand command, List<string> parameters)
        {
            using (var process = CreateProcess(command, parameters))
            {
                process.Start();
                process.WaitForExit();
            }
        }

        private Process CreateProcess(AstScriptCommand command, List<string> parameters)
        {
            Process process;
            switch(command.Environment)
            {
                case ScriptEnvironment.PowerShell:
                    process = CreatePowershellProcess(command, parameters);
                    break;
                case ScriptEnvironment.Batch:
                    process = CreateCmdProcess(command, parameters);
                    break;
                case ScriptEnvironment.Sh:
                    process = CreateShProcess(command, parameters);
                    break;
                case ScriptEnvironment.Bash:
                    process = CreateBashProcess(command, parameters);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported script environment {command.Environment}");
            }

            return process;
        }

        private static Process CreatePowershellProcess(AstScriptCommand command, List<string> parameters)
        {
            return CreateProcess("powershell", $"-Command \"{command.Script}\"");
        }

        private static Process CreateCmdProcess(AstScriptCommand command, List<string> parameters)
        {
            return CreateProcess("cmd", $"/C \"{command.Script}\"");
        }

        private static Process CreateShProcess(AstScriptCommand command, List<string> parameters)
        {
            return CreateProcess("sh", $"-c \"{command.Script}\"");
        }

        private static Process CreateBashProcess(AstScriptCommand command, List<string> parameters)
        {
            return CreateProcess("bash", $"-c \"{command.Script}\"");
        }

        private static Process CreateProcess(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false,
            };
            var process = new Process()
            {
                StartInfo = startInfo,
            };
            return process;
        }

    }
}
