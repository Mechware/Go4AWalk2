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
#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
#endif
	    public T InitialValue;
	    [NonSerialized] public TEvent OnChange = new TEvent();

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

		public override void CopyValue( VariableBase other ) {
			_value = ((Variable<T, TEvent>) other)._value;
		}

	    public void OnEnable() {
			Debug.Log("Enabled");
		    _value = InitialValue;
		}

		public void OnAfterDeserialization() {
			Debug.Log("After Deserialize");
			_value = InitialValue;
		}

	    public void OnBeforeSerialization() { }
    }
}
