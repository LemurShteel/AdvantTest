using UnityEngine;

namespace Bindings
{
    public class BoolElementActiveBinding : Binding<bool>
    {
        [SerializeField]
        RectTransform _rectTransform;
        [SerializeField]
        private bool _invert;

        protected override void UpdateValue(bool value)
        {
            _rectTransform.gameObject.SetActive(_invert ? !value : value);
        }
    }
}
