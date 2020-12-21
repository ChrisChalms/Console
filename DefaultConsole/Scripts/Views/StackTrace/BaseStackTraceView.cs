#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace CC.Console
{
    public abstract class BaseStackTraceView : MonoBehaviour
    {
        // Fields
        [Header("BaseStackTraceView")]
        [SerializeField] protected GameObject _containerObject;
        [SerializeField] protected Text _text;

        // Just change the text, anything more can be handled by the inheriting children
        public virtual void Open(ConsoleLog log) => _text.text = $"{log.Log}\n\n{log.StackTrace}";
        public virtual void Close() { }

    }
}