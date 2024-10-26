namespace CliDsl.App.SpecialCommands
{
    internal class Bootstrapper
    {
        private const string CommandFileName = "commands.txt";
        private const string CliFileName = "cli";
        private const string ExecutableName = "clidsl";

        public const string BatPreset = "bat";
        public const string ShPreset = "sh";

        public void CreateCliFiles(string preset)
        {
            if (preset == BatPreset)
            {
                File.WriteAllText(CommandFileName, CreateCommands(preset));
                File.WriteAllText($"{CliFileName}.bat", CreateBatCli(CommandFileName));
            }
            else if (preset == ShPreset)
            {
                File.WriteAllText(CommandFileName, CreateCommands(preset));
                File.WriteAllText($"{CliFileName}.sh", CreateShCli(CommandFileName));
            }
            else
            {
                throw new ArgumentException($"Unsupported preset {preset}");
            }
        }

        private string CreateCommands(string env)
        {
            var contents = @"
cmd self {env} {
    echo helloself
}

cmd hello {env} {
    echo hello
}
".Replace("{env}", env);
            return contents;
        }

        private string CreateBatCli(string fileName)
        {
            var contents = $"@echo off\n\n{ExecutableName}.exe {fileName} %*";

            return contents;
        }

        private string CreateShCli(string fileName)
        {
            var contents = $"#!/usr/bin/env sh\n\n{ExecutableName} {fileName} \"$@\"";

            return contents;
        }
    }
}
