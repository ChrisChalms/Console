namespace CC.Console
{
    public class ConsoleCommand
    {
        public string Name { get; }
        public CommandHandler Handler { get; }
        public string HelpText { get; }

        // Construct
        public ConsoleCommand(string name, CommandHandler handler, string helpText)
        {
            Name = name;
            Handler = handler;
            HelpText = helpText;
        }
    }
}