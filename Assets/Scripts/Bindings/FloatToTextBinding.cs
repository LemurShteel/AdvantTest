using UnityEngine;
using UnityEngine.UI;

namespace Bindings
{
    public class FloatToTextBinding : Binding<float>
    {
        [SerializeField]
        private Text text;

        [SerializeField]
        private string prefix;

        [SerializeField]
        private string postfix;

        protected override void UpdateValue(float value)
        {
            text.text = prefix + value + postfix;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying || text == null)
            {
                return;
            }
        
            UpdateValue(0);
        }
#endif
    }
}
