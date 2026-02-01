using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace BT.ScriptablesObject
{
    public class RuntimeScriptableObject<T> : ScriptableObject
    {
        public event Action<T> onValueChanged;

        [SerializeField] private T _value = default;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;

                onValueChanged?.Invoke(_value);
            }
        }

        [Button("Call Event")]
        private void InvokeOnValueChanged()
        {
            onValueChanged?.Invoke(_value);
        }
    }
}