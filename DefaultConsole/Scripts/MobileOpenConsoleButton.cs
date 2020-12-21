#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace CC.Console
{
    public class MobileOpenConsoleButton : MonoBehaviour
    {
        [SerializeField] private MobileConsoleView _view;
        [SerializeField] private GameObject _containerObject;
        [SerializeField] private Button _openButton;
        [SerializeField] private Canvas _consoleCanvas;
        [SerializeField] private Transform _moveParent;

        private float _startingX;
        private float _dragOffset;
        private bool _dragging;

        #region MonoBehaviour

        // Initialize
        private void Awake()
        {
            _openButton.onClick.AddListener(openConsole);

            _startingX = _moveParent.position.x;
        }

        #endregion

        #region Dragging

        public void BeginDrag()
        {
            _dragOffset = _moveParent.position.y - Input.mousePosition.y;
            _dragging = true;
        }

        // Move the button up and down so it's not always in the way
        public void OnMoveDrag()
        {
            // Stop resizing when the mouse is off screen
            if (Input.mousePosition.x >= Screen.width || Input.mousePosition.x <= 0f || Input.mousePosition.y >= Screen.height || Input.mousePosition.y <= 0f)
                return;

            var y = Input.mousePosition.y + _dragOffset;
            _moveParent.position = new Vector3(_startingX, y, 0f);
        }

        public void EndDrag() => _dragging = false;

        #endregion

        // The console has been closed, show this button so you cna open it again
        public void ShowButton() => _containerObject.SetActive(true);

        // Open the console and hide this button
        private void openConsole()
        {
            if (_dragging)
                return;

            _view.OpenConsole();

            _containerObject.SetActive(false);
        }
    }
}
