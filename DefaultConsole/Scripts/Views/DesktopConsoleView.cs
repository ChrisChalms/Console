#pragma warning disable 649

using UnityEngine;
using UnityEngine.EventSystems;

namespace CC.Console
{
    public class DesktopConsoleView : BaseConsoleView
    {
        // Fields
        [Header("UI Elements")]
        [SerializeField] private RectSizeChangeListener _sizeChangedListener;

        #region MonoBehaviour

        // Initialize
        protected override void Awake()
        {
            base.Awake();

            _sizeChangedListener.AddListener(resizeLogView);
        }

        // Get inputs
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                ToggleOpen();
                removeBackQuote();
            }

            // If we're active and the input field is selected, then test for inputs
            if(_container.gameObject.activeSelf && EventSystem.current.currentSelectedGameObject == _commandInputField.gameObject)
            {
                // Process command
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                    submitCommand();

                // Command history controls
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                    getCommandFromHistory(1);
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    getCommandFromHistory(-1);

                // Autocomplete command
                else if (Input.GetKeyDown(KeyCode.Tab))
                    autoCompleteCommand();
            }
        }

        #endregion

        #region ConsoleView

        // Hide/Show the console
        public override void ToggleOpen()
        {
            if (_container.gameObject.activeSelf)
                Close();
            else
            {
                _container.gameObject.SetActive(true);
                _commandInputField.ActivateInputField();
            }
        }

        // Shows the stacktrace for the supplied log and show the window if it's not alreay open
        public override void ShowStackTrace(ConsoleLog log) => _stackTraceView.Open(log);

        #endregion

        #region Helpers

        // Tell the log view to resize
        private void resizeLogView() => _logView.ResizeLogView();

        // Removes the backquote from opening/closing the console
        // TODO: Doesn't work on the initial opening for some reason
        private void removeBackQuote()
        {
            if (_commandInputField.text.EndsWith("`"))
                _commandInputField.text = _commandInputField.text.Remove(_commandInputField.text.Length - 1, 1);
        }

        #endregion
    }
}