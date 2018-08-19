using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Events;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Variables {

    [Serializable]
	public abstract class SOVariable<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T>, new() {
#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
#endif
		public T Value;
        public TEvent OnChange = new TEvent();

		public void SetValue( T value ) {
			Value = value;
		}

		public void SetValue( SOReference<T, SOVariable<T, TEvent>, TEvent> value ) {
			Value = value.Value;
		    OnChange.Invoke(Value);
		}
    }
}
