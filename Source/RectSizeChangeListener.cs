using System;
using UnityEngine;

namespace CC.Console
{
    public class RectSizeChangeListener : MonoBehaviour
    {
        private Action _sizeChanged;

        #region MonoBehaviour

        // Size has been changed, invoke action
        private void OnRectTransformDimensionsChange() => _sizeChanged?.Invoke();

        #endregion

        // Add/remove listeners
        public void AddListener(Action callback) => _sizeChanged += callback;
        public void RemoveListener(Action callback) => _sizeChanged -= callback;

    }
}
