using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Variables {
	[CreateAssetMenu]
	public abstract class SOVariable<T> : ScriptableObject {
		public T Value;


#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
#endif

		public void SetValue( T value ) {
			Value = value;
		}

		public void SetValue( SOReference<T> value ) {
			Value = value.Value;
		}

		public abstract void ApplyChange(T amount);

		public abstract void ApplyChange(SOReference<T> amount);
	}
}
