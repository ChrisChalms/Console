#pragma warning disable 649

using UnityEngine;

namespace CC.Console
{
    [CreateAssetMenu(menuName = "Console/Console Configuration", fileName = "ConsoleConfig")]
    public class ConsoleConfig : ScriptableObject
    {
        #region Fields

        [Header("Scope")]
        [SerializeField] private bool _dontDestroyOnLoad = true;

        [Header("View")]
        [SerializeField] private ConsoleView _desktopConsoleView;
        [SerializeField] private ConsoleView _mobileConsoleView;
        [SerializeField] private int _desktopFontSize = 12;
        [SerializeField] private int _mobileFontSize = 18;

        [Header("History")]
        [SerializeField] private int _logHistoryCapacity = -1;
        [SerializeField] private int _commandHistoryCapacity = -1;

        [Header("Unity Logs")]
        [SerializeField] private bool _logUnityLogs = true;
        [SerializeField] private bool _logUnityWarnings = true;
        [SerializeField] private bool _logUnityErrors = true;
        [SerializeField] private bool _logUnityExceptions = true;
        [SerializeField] private bool _logUnityAsserts = true;

        #endregion

        #region Properties

        // Scope
        public new bool DontDestroyOnLoad => _dontDestroyOnLoad;

        // View
        public ConsoleView DesktopConsoleView => _desktopConsoleView;
        public ConsoleView MobileConsoleView => _mobileConsoleView;
        public int DesktopFontSize => _desktopFontSize;
        public int MobileFontSize => _mobileFontSize;

        // History
        public int LogHistoryCapacity => _logHistoryCapacity;
        public int CommandHistoryCapacity => _commandHistoryCapacity;

        // Unity logs
        public bool LogUnityLogs => _logUnityLogs;
        public bool LogUnityWarnings => _logUnityWarnings;
        public bool LogUnityErrors => _logUnityErrors;
        public bool LogUnityExceptions => _logUnityExceptions;
        public bool LogUnityAsserts => _logUnityAsserts;

        #endregion
    }
}