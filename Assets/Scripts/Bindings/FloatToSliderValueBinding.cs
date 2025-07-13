using UnityEngine;
using UnityEngine.UI;

namespace Bindings
{
    public class FloatToSliderValueBinding : Binding<float>
    {
        [SerializeField]
        Slider _slider;

        protected override void UpdateValue(float value)
        {
            _slider.value = value;
        }
    }
}
