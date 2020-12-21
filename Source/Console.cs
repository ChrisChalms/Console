#pragma warning disable 649

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CC.Console
{
	public class Console : MonoBehaviour
	{
		// Fields
		public enum ConsoleViewType
        {
			AUTO_DETECT,
			DESKTOP,
			MOBILE
        }

		// Split the command string by spaces unless it's surrounded by double quotes
		private const string ARGUMENT_STRING_SPLIT = @"(""[^""\\]*(?:\\.[^""\\]*)*""|'[^'\\]*(?:\\.[^'\\]*)*'|[\S]+)+";
		// Pattern to test if an argument is wrapped in quoates
		private const string WRAPPED_IN_QUOTES = @"^"".*""$|^'.*'$";
		private static readonly Regex IsWrappedInQuotes = new Regex(WRAPPED_IN_QUOTES);
		// TODO: Add testing for parenthesis so it feels more natural typing Console.Log(Test) instead of Console.Log Test

		private static Console _instance;
		[SerializeField] private ConsoleConfig _config;
		[SerializeField] private ConsoleViewType _consoleType;

		private ConsoleView _console;
		private ConsoleHistory _consoleHistory;
		private bool _isDesktop;

		// Properties
		public static ConsoleHistory History => _instance._consoleHistory;
		public static ConsoleConfig Config => _instance._config;
		public static bool IsDesktop => _instance._isDesktop;

        #region MonoBehaviour

		// Apply singleton
        private void Awake()
        {
			if (_instance == null)
				_instance = this;
			else
            {
				Debug.Log("There's already an instance of CC.Console");
				Destroy(gameObject);
            }

			_consoleHistory = new ConsoleHistory(_config.LogHistoryCapacity, _config.CommandHistoryCapacity);
			ConsoleCommands.AddDefaultCommands();

			if (_config.DontDestroyOnLoad)
				DontDestroyOnLoad(gameObject);

			instantiateView();
        }

		// Handle Unity's logs
        private void Start() => Application.logMessageReceived += handleUnityLog;
		private void OnDestroy() => Application.logMessageReceived -= handleUnityLog;

		#endregion

		#region Logs

		// Entry points for logging to the console
		public static void Log(string log) => createLog(log);
		public static void LogWarning(string log) => createLog(log, LogType.Warning);
		public static void LogError(string log) => createLog(log, LogType.Error);

		// Create the log object
		private static void createLog(string log) => addLogToHistory(new ConsoleLog(log, string.Empty, LogType.Log));
		private static void createLog(string log, string stackTrace) => addLogToHistory(new ConsoleLog(log, stackTrace, LogType.Log));
		private static void createLog(string log, LogType logType) => addLogToHistory(new ConsoleLog(log, string.Empty, logType));
		private static void createLog(string log, string stackTrace, LogType logType) => addLogToHistory(new ConsoleLog(log, stackTrace, logType));

		// Add the created log to the history
		private static void addLogToHistory(ConsoleLog log) => _instance._consoleHistory.AddLogToHistory(log);

		#endregion

		#region Commands

		// Entrypoint for parsing and excecuting commands
		public static void ExecuteCommand(string command) => parseCommand(command);

		// Parse the command
		private static void parseCommand(string command)
        {
			_instance._consoleHistory.AddCommandToHistory(command);

			var commandSplit = parseArguments(command.Trim());
			var commandName = commandSplit[0];
			commandSplit.RemoveAt(0);

			// Show the command we executed
			var tempLog = new ConsoleLog($"> { command }", string.Empty, LogType.Log);
			_instance._consoleHistory.AddLogToHistory(tempLog);

			ConsoleCommands.ExecuteCommand(commandName, commandSplit.ToArray());
        }

		// Get the arguements from the string if any are present
		private static List<string> parseArguments(string command)
		{
			var args = new List<string>();

			foreach (Match match in Regex.Matches(command, ARGUMENT_STRING_SPLIT))
			{
				var value = match.Value.Trim();
				if (IsWrappedInQuotes.IsMatch(value))
					value = value.Substring(1, value.Length - 2);

				args.Add(value);
			}

			return args;
		}

		#endregion

		#region Command Implementations

		// Tell the view to clear the log
		public static void ClearLog() => _instance._console.ClearConsoleView();

		// Close the console view
		public static void CloseConsole() => _instance._console.Close();

		#endregion

		// Create view based on the viewType setting and config settings
		private void instantiateView()
        {
			var onDesktop = Application.isMobilePlatform ? false : true;
			var wantDesktop = (_consoleType == ConsoleViewType.AUTO_DETECT && onDesktop) || _consoleType == ConsoleViewType.DESKTOP;
			var haveDesktopView = _config.DesktopConsoleView != null;
			var haveMobileView = _config.MobileConsoleView != null;

			if (wantDesktop)
			{
				// We want desktop but we don't have a desktop console view assigned
				if (!haveDesktopView)
				{
					if (haveMobileView)
					{
						Debug.Log("Desktop wanted but only mobile assigned. Spawning mobile...");
						_console = Instantiate(_config.MobileConsoleView, transform, false);
					}
					else
						Debug.LogError("Desktop wanted, but no views are assigned");
				}
				else
				{
					_isDesktop = true;
					_console = Instantiate(_config.DesktopConsoleView, transform, false);
				}
			}
			// We want mobile
			else
			{
				// But we don't have a mobile console view
				if (!haveMobileView)
				{
					// Default to desktop if it's present and hope it works
					if (haveDesktopView)
					{
						Debug.LogWarning("Mobile wanted but only desktop assigned. Spawning Desktop...");
						_console = Instantiate(_config.DesktopConsoleView, transform, false);
						_isDesktop = true;
					}
					else
						Debug.LogError("Mobile wanted but no views are assigned");
				}
				else
					_console = Instantiate(_config.MobileConsoleView, transform, false);
			}
        }

		// Show Unity's log depending on the config settings
		private void handleUnityLog(string logString, string stackTrace, LogType logType)
		{
			switch(logType)
            {
				case LogType.Log:
					if (!_config.LogUnityLogs) return;
					break;
				case LogType.Warning:
					if (!_config.LogUnityWarnings) return;
					break;
				case LogType.Error:
					if (!_config.LogUnityErrors) return;
					break;
				case LogType.Exception:
					if (!_config.LogUnityExceptions) return;
					break;
				case LogType.Assert:
					if (!_config.LogUnityAsserts) return;
					break;
            }


			createLog(logString, stackTrace, logType);
		}
	}
}