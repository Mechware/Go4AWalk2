using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Data;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace G4AW2.Data {

	public abstract class FollowerConfig : ScriptableObject {

		public int Id;
	    public string DisplayName;
	    public Sprite Portrait;

        public Vector2 SizeOfSprite = new Vector2(32,32);
        public int SpaceBetweenEnemies = 32;
		public AnimationClip SideIdleAnimation;
		public AnimationClip RandomAnimation;
		public bool HasRandomAnimation { get { return RandomAnimation != null; } }

		public float MinTimeBetweenRandomAnims = 30;
		public float MaxTimeBetweenRandomAnims = 180;
	}

}
