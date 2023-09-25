using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace UI.Common {
    public class MenuWindow :MonoBehaviour {
        public bool IsOpened { get; private set; }

        protected bool _initialized = false;
        public virtual void Init(bool startOpened = false) {
            IsOpened = startOpened;
            gameObject.SetActive(startOpened);
            _initialized = true;
        }
        void Start() {
            if (!_initialized) Init();
        }
        public void Open() => SetOpenedInternal(true);
        public void Close() => SetOpenedInternal(false);
        protected virtual void SetOpenedInternal(bool value) {
            if (!_initialized) Init();
            if (value == IsOpened) return;
            IsOpened = value;
            gameObject.SetActive(value);
        }
    }
}