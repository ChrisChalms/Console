#pragma warning disable 649

using UnityEngine;

namespace CC.Console
{
    public class MobileConsoleResize : MonoBehaviour
    {
		[SerializeField] private RectTransform _resizeHandleContainer;
        [SerializeField] private RectTransform _consoleContainer;
        [SerializeField] private RectTransform _stackTraceContainer;
		[SerializeField] private Canvas _consoleCanvas;
		[SerializeField] private Vector2 _minMaxPosition = new Vector2(0.01f, 0.75f);

		private RectTransform _canvasRect;

		#region MonoBehaviour

		// Initialize
		private void Awake() => _canvasRect = _consoleCanvas.transform as RectTransform;

		#endregion

		// Resize the containers 
		public void ResizeDrag()
		{
			// Stop resizing when the mouse is off screen
			if (Input.mousePosition.x >= Screen.width || Input.mousePosition.x <= 0f || Input.mousePosition.y >= Screen.height || Input.mousePosition.y <= 0f)
				return;

			// The screen and canvas resolutions might be different, so get the percentage of the screen the drag has moved for, then apply that to the canvas size
			var percentMovedY = Input.mousePosition.y == 0 ? 0 : Input.mousePosition.y / _canvasRect.rect.height * Mathf.Abs(_canvasRect.rect.height / Screen.height);
			
			// Calculate the new container sizes
			percentMovedY = Mathf.Clamp(percentMovedY, _minMaxPosition.x, _minMaxPosition.y);
			var min = new Vector2(0, percentMovedY);
			var max = new Vector2(1, percentMovedY);

			_resizeHandleContainer.anchorMin = min;
			_resizeHandleContainer.anchorMax = max;
			_consoleContainer.anchorMin = min;
			_stackTraceContainer.anchorMax = max;
		}
	}
}
