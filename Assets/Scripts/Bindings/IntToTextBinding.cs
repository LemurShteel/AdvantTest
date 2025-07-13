using UnityEngine;
using UnityEngine.UI;

namespace Bindings
{
    public class IntToTextBinding : Binding<int>
    {
        [SerializeField]
        private Text text;

        [SerializeField]
        private string prefix;

        [SerializeField]
        private string postfix;

        protected override void UpdateValue(int value)
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
