using System;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data.Combat {
	[CreateAssetMenu(menuName = "Data/Follower/Enemy")]
    public class EnemyConfig : ScriptableObject {

	    public string DisplayName;
	    public Sprite Portrait;

	    public Vector2 SizeOfSprite = new Vector2(32,32);
	    public AnimationClip SideIdleAnimation;
	    public AnimationClip RandomAnimation;
	    
		[Header("Animations")]
		public AnimationClip Idle;
		public AnimationClip Flinch;
        public AnimationClip BeforeAttack;
        public AnimationClip AttackExecute;
		public AnimationClip AfterAttack;
		public AnimationClip Death;
		public AnimationClip Dead;
        public AnimationClip Walking;

	    public Sprite DeadSprite;

	    public float AttackDurationUntilDamage = 0.2f;
    }
}


