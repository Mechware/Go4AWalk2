using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;

namespace G4AW2.Combat.Swiping {
	public class SwipeRecorder : MonoBehaviour {

		public Vector3ArrayVariable Swipe;

		public bool RecordSwipe = false;

		public void OnSwipe(Vector3[] var) {
			if (RecordSwipe) {
				Swipe.Value = var;
			}
			RecordSwipe = false;
		}
	}

}

