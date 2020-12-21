using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CC.Console
{
    public static class ConsoleCommands
    {
        private static Dictionary<string, ConsoleCommand> _commands = new Dictionary<string, ConsoleCommand>();

        #region Register/Deregister Commands

        // Construct the command and add it to the dictionary
        public static void AddCommand(string name, CommandHandler handler, string helpText)
        {
            var command = new ConsoleCommand(name, handler, helpText);

            if (!_commands.ContainsKey(name.ToLower()))
                _commands.Add(name.ToLower(), command);
            // Overwrite the existing command, but log a warning to let the user know
            else
            {
                _commands[name] = command;
                Debug.LogWarning($"The command \"{ name }\" was registered but the command already existed, the older command has been overwritten");
            }
        }

        // Remove a registered command
        public static void RemoveCommand(string name)
        {
            if (_commands.ContainsKey(name.ToLower()))
                _commands.Remove(name.ToLower());
            else
                Debug.LogWarning($"Trying to remove command \"{ name }\" but there is not registered command with that name");
        }

        // Adds the default commands to te list
        public static void AddDefaultCommands()
        {
            // Console Commands
            AddCommand("Console.Log", consoleLog, "Logs a message to the console");
            AddCommand("Console.LogWarning", consoleLogWarning, "Logs a warning to the console");
            AddCommand("Console.LogError", consoleLogError, "Logs an error to the console");
            AddCommand("Clear", clearConsole, "Clear the view of all logs");
            AddCommand("Console.Close", closeConsole, "Closes the console window");
            AddCommand("Help", printHelp, "Shows the help text of all available commands");
            
            // Unity Commands
            AddCommand("Debug.Log", debugLog, "Log a message to the Unity console");
            AddCommand("Debug.LogWarning", debugLogWarning, "Log a warning message to the Unity console");
            AddCommand("Debug.LogError", debugLogError, "Log an error message to the Unity console");
            AddCommand("Debug.LogException", debugLogException, "Log an exception to the Unit console");
            AddCommand("Debug.Break", debugBreak, "Pause the editor");
            AddCommand("Quit", applicationQuit, "Stops the editor playing or quits a standalone build");
        }

        #endregion

        // try to execute the command
        public static void ExecuteCommand(string name, string[] args)
        {
            try
            {
                _commands[name.ToLower()].Handler(args);
            }
            catch
            {
                Console.LogError($"Command \"{ name }\" not found");
            }
        }

        #region Default Commands

        // Log to our console
        private static void consoleLog(string[] args) => Console.Log(combineArgs(args));
        private static void consoleLogWarning(string[] args) => Console.LogWarning(combineArgs(args));
        private static void consoleLogError(string[] args) => Console.LogError(combineArgs(args));

        // Prints out the help text of the registered commands
        private static void printHelp(string[] args)
        {
            foreach (var command in _commands.Values.OrderBy(c => c.Name))
                Console.Log($"{command.Name}: {command.HelpText}");
        }

        // Clear/RePool all the logs
        private static void clearConsole(string[] clear) => Console.ClearLog();

        // Close the console window
        private static void closeConsole(string[] args) => Console.CloseConsole();

        // Unity logs
        private static void debugLog(string[] args) => Debug.Log(combineArgs(args));
        private static void debugLogWarning(string[] args) => Debug.LogWarning(combineArgs(args));
        private static void debugLogError(string[] args) => Debug.LogError(combineArgs(args));
        private static void debugLogException(string[] args) => Debug.LogException(new System.Exception(combineArgs(args)));

        // Pause the editor
        private static void debugBreak(string[] args) => Debug.Break();

        // Quit the editor
        private static void applicationQuit(string[] args)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

    #endregion

    #region Helpers

        // Combines the arguments into a space separated string
        private static string combineArgs(string[] args)
        {
            var output = string.Empty;

            for (var i = 0; i < args.Length; i++)
                output += $"{args[i]} ";

            return output;
        }

        // Retrun a list of registered commands, ordered by their name
        public static List<ConsoleCommand> GetOrderedCommands() => _commands.Values.OrderBy(c => c.Name).ToList();

    #endregion
    }
}