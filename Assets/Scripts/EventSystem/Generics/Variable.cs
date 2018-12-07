using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {

    [Serializable]
	public abstract class Variable<T, TEvent> : VariableBase where TEvent : UnityEvent<T>, ISerializationCallbackReceiver, new() {
		[Multiline]
		public string DeveloperDescription = "";
	    public T InitialValue;
	    public TEvent OnChange = new TEvent();

		[ReadOnly] [SerializeField] private T _value;

		public T Value {
            get { return _value; }
            set {
                _value = value;
                OnChange.Invoke(Value);
            }
        }

        public static implicit operator T( Variable<T, TEvent> val) {
            return val.Value;
        }

	    public void OnEnable() {
		    _value = InitialValue;
		}

		public void OnAfterDeserialization() {
			Debug.Log("After Deserialize");
			_value = InitialValue;
		}

	    public void OnBeforeSerialization() { }
    }
}
