using UnityEngine;

namespace Bindings
{
    public abstract class Binding<T> : MonoBehaviour where T : struct
    {
        private T currentValue;

        public void SetValue(T value)
        {
            if (currentValue.Equals(value))
            {
                return;
            }

            currentValue = value;
            UpdateValue(currentValue);
        }

        protected abstract void UpdateValue(T value);
    }
}
