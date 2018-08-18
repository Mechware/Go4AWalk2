using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Variables {

	[CreateAssetMenu(menuName = "Variable/Float")]
	public class FloatVariable : SOVariable<float> {
#if UNITY_EDITOR
		[ContextMenu("Raise Event")]
		public void RaiseChangedEvent() {
			OnChange.Invoke(Value);
		}
#endif
	}
}

