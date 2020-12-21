#pragma warning disable 649

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Console
{
    public class DefaultFilterMenu : MonoBehaviour
    {
        // Fields
        [SerializeField] private GameObject _menuContainer;
        [SerializeField] private Button _filterMenuButton;
        [SerializeField] private DefaultFilterMenuItem _logItem;
        [SerializeField] private DefaultFilterMenuItem _warningItem;
        [SerializeField] private DefaultFilterMenuItem _assertItem;
        [SerializeField] private DefaultFilterMenuItem _errorItem;
        [SerializeField] private DefaultFilterMenuItem _exceptionItem;

        private bool _isOpen;
        private Dictionary<LogType, bool> _filterStates = new Dictionary<LogType, bool>
        {
            {LogType.Log, true},
            {LogType.Warning, true},
            {LogType.Assert, true},
            {LogType.Error, true},
            {LogType.Exception, true},
        };

        // Actions
        private Action _filterSettingsChanged;

        #region MonoBehaviour

        // Initialize
        private void Awake()
        {
            _filterMenuButton.onClick.AddListener(toggleOpen);
            _logItem.Toggle.onValueChanged.AddListener(logToggled);
            _warningItem.Toggle.onValueChanged.AddListener(warningToggled);
            _assertItem.Toggle.onValueChanged.AddListener(assertToggled);
            _errorItem.Toggle.onValueChanged.AddListener(errorToggled);
            _exceptionItem.Toggle.onValueChanged.AddListener(exceptionToggled);
        }

        #endregion

        // Open/Closes the filter menu
        private void toggleOpen()
        {
            _isOpen = !_isOpen;

            _menuContainer.SetActive(_isOpen);
        }

        // Get the setting for the supplied log type
        public bool GetFilterSetting(LogType type) => _filterStates[type];

        // Add/Remove listeners to the _filterSettingsChanged action
        public void AddFilterChangedListener(Action callback) => _filterSettingsChanged += callback;
        public void RemoveFilterChangedListener(Action callback) => _filterSettingsChanged -= callback;

        #region Toggles

        // The log item has been toggled
        private void logToggled(bool state)
        {
            _logItem.ChangeActiveState(state);
            _filterStates[LogType.Log] = state;
            _filterSettingsChanged?.Invoke();
        }

        // The warning item has been toggled
        private void warningToggled(bool state)
        {
            _warningItem.ChangeActiveState(state);
            _filterStates[LogType.Warning] = state;
            _filterSettingsChanged?.Invoke();
        }

        // The assert item has been toggled
        private void assertToggled(bool state)
        {
            _assertItem.ChangeActiveState(state);
            _filterStates[LogType.Assert] = state;
            _filterSettingsChanged?.Invoke();
        }

        // The error item has been toggled
        private void errorToggled(bool state)
        {
            _errorItem.ChangeActiveState(state);
            _filterStates[LogType.Error] = state;
            _filterSettingsChanged?.Invoke();
        }

        // The exception item has been toggled
        private void exceptionToggled(bool state)
        {
            _exceptionItem.ChangeActiveState(state);
            _filterStates[LogType.Exception] = state;
            _filterSettingsChanged?.Invoke();
        }

        #endregion
    }
}
