using UnityEngine;

namespace G4AW2.Data
{

    public abstract class FollowerConfig : ScriptableObject {

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
