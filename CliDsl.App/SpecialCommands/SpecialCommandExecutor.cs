namespace CliDsl.App.SpecialCommands
{
    internal class SpecialCommandExecutor
    {
        public bool Execute(string[] args)
        {
            var didExecute = false;
            if (args.Length >= 1 && args[0] == "bootstrap")
            {
                var preset = args.Length >= 2 ? args[1] : Bootstrapper.ShPreset;
                var bs = new Bootstrapper();
                bs.CreateCliFiles(preset);

                didExecute = true;
            }

            return didExecute;
        }
    }
}
