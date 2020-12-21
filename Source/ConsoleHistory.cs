using System;
using System.Collections.Generic;

namespace CC.Console
{
    public class ConsoleHistory
    {
        // Fields
        private List<ConsoleLog> _logHistory = new List<ConsoleLog>();
        private List<string> _commandHistory = new List<string>();

        private Action _logHistoryChanged;
        private int _logCapacity;
        private int _commandCapacity;

        // Properties
        public ConsoleLog LatestLog => _logHistory[_logHistory.Count - 1];
        public int CommandHistoryCount => _commandHistory.Count;

        // Initialize
        public ConsoleHistory(int logCap, int commandCap)
        {
            _logCapacity = logCap;
            _commandCapacity = commandCap;
        }

        // Action register/deregister
        public void AddHistoryChangedListener(Action callback) => _logHistoryChanged += callback;
        public void RemoveHistoryChangedListener(Action callback) => _logHistoryChanged -= callback;

        // Adds a log to the history and invokes the action
        public void AddLogToHistory(ConsoleLog log)
        {
            // Remove the first element if we're over capacity
            if (_logCapacity != -1 && _logHistory.Count >= _logCapacity)
                _logHistory.RemoveAt(0);

            _logHistory.Add(log);
            _logHistoryChanged?.Invoke();
        }

        // Adds a command to the history
        public void AddCommandToHistory(string command)
        {
            // Remove the first element if we're over capacity
            if (_commandCapacity != -1 && _commandHistory.Count >= _commandCapacity)
                _commandHistory.RemoveAt(0);
            
            _commandHistory.Add(command);
        }

        // Get the command at the given index
        public string GetCommandAtIndex(int index) => _commandHistory[index];
    }
}
