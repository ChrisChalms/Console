using UnityEngine;

namespace CC.Console
{
    public class ConsoleLog
    {
        public string Log { get; }
        public string StackTrace { get; }
        public LogType LogType { get; }

        public bool HasStackTrace => !string.IsNullOrEmpty(StackTrace);

        public ConsoleLog(string log, string stackTrace, LogType logType)
        {
            Log = log;
            StackTrace = stackTrace;
            LogType = logType;
        }
    }
}
