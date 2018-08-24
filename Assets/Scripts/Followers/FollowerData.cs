using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Data {

	public abstract class FollowerData : ScriptableObject {

		public AnimationClip SittingAnimation;
		public AnimationClip RandomAnimation;
		public bool HasRandomAnimation { get { return RandomAnimation != null; } }

		public float MinTimeBetweenRandomAnims = 30;
		public float MaxTimeBetweenRandomAnims = 180;

	}

}
