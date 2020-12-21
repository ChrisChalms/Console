#pragma warning disable 649

using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Console
{
    public abstract class BaseConsoleView : ConsoleView
    {
        [Header("BaseConsoleView")]
        [SerializeField] protected RectTransform _container;
        [SerializeField] private Button _closeButton;
        [SerializeField] protected DefaultLogView _logView;
        [SerializeField] protected InputField _commandInputField;
        [SerializeField] protected BaseStackTraceView _stackTraceView;

        private int _historyIndex;

        #region MonoBehaviour

        // Initialize
        protected override void Awake()
        {
            base.Awake();

            _closeButton.onClick.AddListener(Close);
            _logView.Initialize();
        }

        #endregion

        #region ConsoleView

        // Close/hide the console
        public override void Close() => _container.gameObject.SetActive(false);

        // Tell the LogView to clear
        public override void ClearConsoleView() => _logView.ClearLogs();

        // Add the log to the list
        public override void OnConsoleHistoryChanged() => _logView.AddItemToLog(Console.History.LatestLog);

        #endregion

        // Pass the command to the console for processing
        protected void submitCommand(bool makeInputActive = true)
        {
            if (string.IsNullOrEmpty(_commandInputField.text))
                return;

            Console.ExecuteCommand(_commandInputField.text);

            _commandInputField.text = string.Empty;
            _historyIndex = 0;

            if (makeInputActive)
                _commandInputField.ActivateInputField();
        }

        // Sets the inputfield text to be a command from the history if valid e.g. If you press up while the input field is selected you expect it to display that last entered command
        protected void getCommandFromHistory(int direction)
        {
            var historyCount = Console.History.CommandHistoryCount;

            if (historyCount == 0)
                return;

            _historyIndex += direction;
            _historyIndex = Mathf.Clamp(_historyIndex, 0, historyCount);

            var index = Mathf.Clamp((historyCount - 1) - (_historyIndex - 1), 0, historyCount - 1);

            if (_historyIndex == 0)
                _commandInputField.text = string.Empty;
            else
            {
                _commandInputField.text = Console.History.GetCommandAtIndex(index);
                _commandInputField.caretPosition = _commandInputField.text.Length;
            }
        }

        // Attempts to complete what's already been typed if it matches an existing command
        protected void autoCompleteCommand()
        {
            var input = _commandInputField.text.Trim();

            if (string.IsNullOrEmpty(input))
                return;

            var commands = ConsoleCommands.GetOrderedCommands().Where(c => c.Name.StartsWith(input, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if(commands.Count == 1)
            {
                _commandInputField.text = commands[0].Name;
                _commandInputField.MoveTextEnd(false);
            }
            // If there's more than one available, list them in a single log
            else if(commands.Count > 1)
            {
                var output = new StringBuilder();

                for (var i = 0; i < commands.Count; i++)
                {
                    output.Append(commands[i].Name);

                    if (i != commands.Count - 1)
                        output.Append("\t\t");
                }

                Console.Log(output.ToString());
            }
        }
    }
}