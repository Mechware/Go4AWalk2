using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEditor;
using UnityEngine;

namespace G4AW2.Combat.Swiping {
	public class SwipeRecorder : MonoBehaviour {

		public PersistentSetVector3 Swipe;

		public bool RecordSwipe = false;

		public void OnSwipe(Vector3[] var) {
			if (RecordSwipe) {
				Swipe.Value.Clear();
				Swipe.Value.AddRange(var);
#if UNITY_EDITOR
				EditorUtility.SetDirty(Swipe);
#endif
			}
			RecordSwipe = false;
		}
	}

}

