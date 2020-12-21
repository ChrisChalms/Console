#pragma warning disable 649

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CC.Console
{
    public class MobileConsoleView : BaseConsoleView
    {
        [SerializeField] private MobileOpenConsoleButton _openButton;
        [SerializeField] private Button _submitButton;

        #region MonoBehaviour

        private void Update()
        {
            // If we're active and the input field is selected, then test for inputs
            if (_container.gameObject.activeSelf && EventSystem.current.currentSelectedGameObject == _commandInputField.gameObject)
            {
                // Makes it easier to test the  mobile layout on desktop
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                    submitCommand();
            }
        }

        #endregion

        #region BaseConsoleView

        // Initialize
        protected override void Awake()
        {
            base.Awake();

            _submitButton.onClick.AddListener(submitCommand);
            _commandInputField.onEndEdit.AddListener((s) => EndedCommandInput());
        }

        #endregion

        #region ConsoleView

        // Show the open button on mobile
        public override void Close()
        {
            base.Close();

            _openButton.ShowButton();
        }

        // Update the stacktrace text
        public override void ShowStackTrace(ConsoleLog log) => _stackTraceView.Open(log);

        #endregion

        // Show the console and hide the open button
        public void OpenConsole() => _container.gameObject.SetActive(true);

        // The input has ended, check the keyboard to see if it was submitted or cancelled
        private void EndedCommandInput()
        {
            // We don't want to submit the command if we've just lost focus
            if (_commandInputField.touchScreenKeyboard?.status == TouchScreenKeyboard.Status.Done)
                submitCommand();
        }

        #region Helpers

        // onClick.AddListener needs a method without arguments, but we need to overwrite the default submitCommand to keep the inputfield deactive
        private void submitCommand() => submitCommand(false);

        #endregion
    }
}