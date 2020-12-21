#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace CC.Console
{
    public class DesktopStackTraceView : BaseStackTraceView
    {
        // Fields
        [Header("UI Elements")]
        [SerializeField] private RectSizeChangeListener _sizeChangedListener;
        [SerializeField] private Button _closeButton;

        #region BaseStaceTraceView

        // Initialize
        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
        }

        // Open the window if it's not already
        public override void Open(ConsoleLog log)
        {
            base.Open(log);
            _containerObject.SetActive(true);
        }

        // Close/Hide the stack trace view
        public override void Close() => _containerObject.SetActive(false);

        #endregion
    }
}