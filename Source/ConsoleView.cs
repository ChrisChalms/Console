using UnityEngine;

namespace CC.Console
{
    public abstract class ConsoleView : MonoBehaviour
    {
        #region MonoBehaviour

        protected virtual void Awake() => Console.History.AddHistoryChangedListener(OnConsoleHistoryChanged);
        protected virtual void OnDestroy() => Console.History.RemoveHistoryChangedListener(OnConsoleHistoryChanged);

        #endregion

        public virtual void ToggleOpen() { }
        public virtual void Close() { }

        public virtual void ClearConsoleView() { }
        public virtual void OnConsoleHistoryChanged() { }

        public virtual void ShowStackTrace(ConsoleLog log) { }
    }
}