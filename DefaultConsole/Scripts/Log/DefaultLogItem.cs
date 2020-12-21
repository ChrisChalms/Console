#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace CC.Console
{
    public class DefaultLogItem : MonoBehaviour
    {
        // Fields
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Text _logText;
        [SerializeField] private Image _stackTraceIcon;
        [SerializeField] private Button _stackTraceButton;

        private BaseConsoleView _view;

        // Properties
        public ConsoleLog Log { get; private set; }
        public float Height => _rectTransform.sizeDelta.y;

        // Get the log item ready for use
        public void Initialize(BaseConsoleView view, ConsoleLog log, Color backgroundColor, int fontSize)
        {
            _view = view;
            Log = log;
            _logText.fontSize = fontSize;
            _logText.text = log.Log;
            _backgroundImage.color = backgroundColor;

            if (log.HasStackTrace)
            {
                _stackTraceIcon.enabled = true;
                _stackTraceButton.interactable = true;
                _stackTraceButton.onClick.AddListener(showStackTrace);
            }
            else
            {
                _stackTraceIcon.enabled = false;
                _stackTraceButton.interactable = false;
            }
        }

        // Shows the stack trace window
        private void showStackTrace() => _view.ShowStackTrace(Log);
    }
}
