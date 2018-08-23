using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {

    [Serializable]
	public abstract class Variable<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T>, new() {
#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
#endif
        [ShowInInspector] private T _value;

        public T Value {
            get { return _value; }
            set {
                _value = value;
                OnChange.Invoke(Value);
            }
        }

        void OnEnable() {
            _value = Value;
        }

        public TEvent OnChange;

        public static implicit operator T( Variable<T, TEvent> val) {
            return val.Value;
        }
    }
}
