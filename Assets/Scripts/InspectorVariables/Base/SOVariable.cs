using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Events;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Variables {

    [Serializable]
	public abstract class SOVariable<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T>, new() {
#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
#endif
        private T _value;

        public T Value {
            get { return _value; }
            set {
                _value = value;
                OnChange.Invoke(Value);
            }
        }

        public T ConstantValue;

        void OnEnable() {
            _value = Value;
        }

        public TEvent OnChange;

        public static implicit operator T( SOVariable<T, TEvent> val) {
            return val.Value;
        }
    }
}
