#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace CC.Console
{
    public class DefaultFilterMenuItem : MonoBehaviour
    {
        // Fields
        [SerializeField] private Image _icon;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Color _onColour;
        [SerializeField] private Color _offColour;

        // Properties
        public Toggle Toggle => _toggle;

        // Changes the colour of the icon to reflect the active state
        public void ChangeActiveState(bool active) => _icon.color = active ? _onColour : _offColour;
    }
}
