#pragma warning disable 649

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Console
{
    public class DefaultLogView : MonoBehaviour
    {
        [SerializeField] private int _logPoolSize = 64;
        [SerializeField] private Transform _poolParent;
        [SerializeField] private RectTransform _logContent;
        [SerializeField] private DefaultLogItem _logItemPrefab;
        [SerializeField] protected DefaultFilterMenu _filterMenu;
        [SerializeField] private BaseConsoleView _view;

        [Header("Colours")]
        [SerializeField] private Color _primaryLogColour;
        [SerializeField] private Color _secondaryLogColour;
        [SerializeField] private Color _errorColour;
        [SerializeField] private Color _warningColour;

        private List<DefaultLogItem> _itemsCurrentlyInLog = new List<DefaultLogItem>();
        private Queue<DefaultLogItem> _logPool = new Queue<DefaultLogItem>();

        private bool _useSecondaryColour;

        // Ready to start
        public void Initialize()
        {
            _filterMenu.AddFilterChangedListener(filterSettingsChanged);
            populatePool();
        }

        // A filter setting was changed, toggle items on and off
        private void filterSettingsChanged()
        {
            for(var i = 0; i < _itemsCurrentlyInLog.Count; i++)
            {
                switch(_itemsCurrentlyInLog[i].Log.LogType)
                {
                    case LogType.Log:
                        _itemsCurrentlyInLog[i].gameObject.SetActive(_filterMenu.GetFilterSetting(LogType.Log));
                        break;
                    case LogType.Warning:
                        _itemsCurrentlyInLog[i].gameObject.SetActive(_filterMenu.GetFilterSetting(LogType.Warning));
                        break;
                    case LogType.Assert:
                        _itemsCurrentlyInLog[i].gameObject.SetActive(_filterMenu.GetFilterSetting(LogType.Assert));
                        break;
                    case LogType.Error:
                        _itemsCurrentlyInLog[i].gameObject.SetActive(_filterMenu.GetFilterSetting(LogType.Error));
                        break;
                    case LogType.Exception:
                        _itemsCurrentlyInLog[i].gameObject.SetActive(_filterMenu.GetFilterSetting(LogType.Exception));
                        break;
                }
            }
        }

        // Spawn the object pool
        private void populatePool()
        {
            // Add one extra in the pool
            for(var i = 0; i < _logPoolSize + 1; i++)
            {
                var tempLog = Instantiate(_logItemPrefab, _poolParent, false);
                _logPool.Enqueue(tempLog);
            }
        }

        // Get the next object from the pool and change the parent
        public void AddItemToLog(ConsoleLog log)
        {
            var logItem = _logPool.Dequeue();
            logItem.Initialize(_view, log, getLogColour(log.LogType), getFontSize());
            logItem.transform.SetParent(_logContent, false);
            _itemsCurrentlyInLog.Add(logItem);

            // Repool item if max is reached
            if (_itemsCurrentlyInLog.Count >_logPoolSize)
                repoolItem(_itemsCurrentlyInLog[0]);

            ResizeLogView();
        }

        // Give item in the view back to the pool
        private void repoolItem(DefaultLogItem item)
        {
            _logPool.Enqueue(item);
            item.transform.SetParent(_poolParent, false);
            _itemsCurrentlyInLog.Remove(item);
        }

        // Recalculate the required height of the logview
        public void ResizeLogView()
        {
            // I'm not too keen on this, but it's better than waiting a frame and getting an ugly frame
            LayoutRebuilder.ForceRebuildLayoutImmediate(_logContent);

            var newHeight = 0f;
            foreach (var item in _itemsCurrentlyInLog)
                newHeight += item.Height;

            _logContent.sizeDelta = new Vector2(_logContent.sizeDelta.x, newHeight);
        }

        // Clear/RePool the logview
        public void ClearLogs()
        {
            for (var i = 0; i < _itemsCurrentlyInLog.Count; i++)
            {
                repoolItem(_itemsCurrentlyInLog[i]);
                i--; // We've removed an item during the repooling, move the index back
            }

            ResizeLogView();
        }

        #region Helpers

        // Return the correct colour if it's an error, warning, or exception, or toggle between the two log colours
        private Color getLogColour(LogType logType)
        {
            switch(logType)
            {
                case LogType.Error:
                case LogType.Exception:
                    return _errorColour;
                case LogType.Warning:
                    return _warningColour;
                default:
                    _useSecondaryColour = !_useSecondaryColour;
                    return !_useSecondaryColour ? _secondaryLogColour : _primaryLogColour;
            }
        }

        // Returns the font size based on platform
        private int getFontSize() => Console.IsDesktop ? Console.Config.DesktopFontSize : Console.Config.MobileFontSize;

        #endregion
    }
}