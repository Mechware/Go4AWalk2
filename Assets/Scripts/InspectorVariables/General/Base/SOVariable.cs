using System.Collections;
using System.Collections.Generic;
using G4AW2.Events;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Variables {

	public abstract class SOVariable<T> : ScriptableObject {
#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
#endif

		[System.Serializable]
		public class UnityEventGeneric : UnityEvent<T> { }

		public T Value;
		[SerializeField] public UnityEventGeneric OnChange;



		public void SetValue( T value ) {
			Value = value;
			OnChange.Invoke(value);
		}

		public void SetValue( SOReference<T> value ) {
			Value = value.Value;
			OnChange.Invoke(value);

		}

		/* For some stupid ass reason you can't do this in this class
#if UNITY_EDITOR
		[ContextMenu("Raise Event")]
		public void RaiseChangedEvent() {
			OnChange.Invoke(Value);
		}
#endif
		*/
		//public abstract void ApplyChange(T amount);

		//public abstract void ApplyChange(SOReference<T> amount);
	}
}
