using UnityEngine;
using UnityEngine.UI;

namespace Bindings
{
    public class DoubleToTextBinding : Binding<double>
    {
        [SerializeField]
        private Text text;

        [SerializeField]
        private string prefix;

        [SerializeField]
        private string postfix;

        protected override void UpdateValue(double value)
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
