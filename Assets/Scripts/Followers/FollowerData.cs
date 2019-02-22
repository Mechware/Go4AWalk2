using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Questing;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace G4AW2.Data {

	public abstract class FollowerData : ScriptableObject, IID {

		public int ID;

		public AnimationClip SideIdleAnimation;
		public AnimationClip RandomAnimation;
		public bool HasRandomAnimation { get { return RandomAnimation != null; } }

		public float MinTimeBetweenRandomAnims = 30;
		public float MaxTimeBetweenRandomAnims = 180;

#if UNITY_EDITOR
		[ContextMenu("Pick ID")]
		public void PickID() {
			ID = IDUtils.PickID<FollowerData>();
		}
#endif

		public int GetID() {
			return ID;
		}
	}

}
