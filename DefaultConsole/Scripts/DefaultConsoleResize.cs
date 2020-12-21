#pragma warning disable 649

using UnityEngine;

namespace CC.Console
{
	public class DefaultConsoleResize : MonoBehaviour
	{
		// Fields
		[SerializeField] private RectTransform _container;
		[SerializeField] private Vector2 _minSize = new Vector2(256, 128);
		[SerializeField] private Canvas _consoleCanvas;
		private RectTransform _canvasRect;

		private Vector3 _resizeMousePosBegin;
		private Vector2 _resizeSizeBegin;

        #region MonoBehaviour

		// Initialize
        private void Awake() => _canvasRect = _consoleCanvas.transform as RectTransform;

        #endregion

        // Set the offsets at the beginning of the drag
        public void BeindResizeDrag()
		{
			_resizeMousePosBegin = Input.mousePosition;
			_resizeSizeBegin = _container.sizeDelta;
		}

		public void ResizeDrag()
		{
			// Stop resizing when the mouse is off screen
			if (Input.mousePosition.x >= Screen.width || Input.mousePosition.x <= 0f || Input.mousePosition.y >= Screen.height || Input.mousePosition.y <= 0f)
				return;

			// The screen and canvas resolutions might be different, so get the percentage of the screen the drag has moved for, then apply that percentage to the console canvas size
			var movedDeltaX = Input.mousePosition.x - _resizeMousePosBegin.x;
			var movedDeltaY = Input.mousePosition.y - _resizeMousePosBegin.y;
			var percentMovedX = movedDeltaX == 0 ? 0 : (movedDeltaX / Screen.width * _canvasRect.rect.width);
			var percentMovedY = movedDeltaY == 0 ? 0 : (movedDeltaY / Screen.height * _canvasRect.rect.height);
			
			// Calculate new size
			var xSize = _resizeSizeBegin.x + percentMovedX;
			var ySize = _resizeSizeBegin.y - percentMovedY;
			xSize = Mathf.Clamp(xSize, _minSize.x, Screen.width);
			ySize = Mathf.Clamp(ySize, _minSize.y, Screen.height);
			_container.sizeDelta = new Vector2(xSize, ySize);
		}
	}
}