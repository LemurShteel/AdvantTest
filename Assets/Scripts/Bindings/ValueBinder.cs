using System;
using UnityEngine;

namespace Bindings
{
    [Serializable]
    public class ValueBinder<T> where T : struct
    {
        [SerializeField]
        private Binding<T>[] _bindings;

        public void SetValue(T value)
        {
            foreach (var binder in _bindings)
            {
                binder.SetValue(value);
            }
        }
    }
}
