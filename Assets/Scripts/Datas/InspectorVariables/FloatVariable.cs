using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Variables {

	public class FloatVariable : SOVariable<float> {

		public override void ApplyChange(float amount) {
			Value += amount;
		}

		public override void ApplyChange(SOReference<float> amount) {
			Value += amount;
		}
	}
}

