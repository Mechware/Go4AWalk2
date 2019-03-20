using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data.Combat {
	[CreateAssetMenu(menuName = "Data/Follower/Enemy")]
    public class EnemyData : FollowerData {

		[Header("Animations")]
		public AnimationClip Idle;
		public AnimationClip Flinch;
        public AnimationClip BeforeAttack;
        public AnimationClip AttackExecute;
		public AnimationClip AfterAttack;
		public AnimationClip Death;
		public AnimationClip Dead;
        public AnimationClip Walking;

		[Header("Stats")]
		public int BaseHealth;
        public int BaseArmor;
	    public float TimeBetweenHeavyAttacks;
	    public int BaseHeavyDamage;

        public float AttackPrepTime;
        public float AttackExecuteTime;

		[Header("Misc")]
		public ItemDropper Drops;

	    public int GetHealth(int level) {
		    return BaseHealth;
	    }

	    public int GetArmor( int level ) {
		    return BaseArmor;
	    }

	    public float GetTimeBetweenHeavyAttacks(int level) {
		    return TimeBetweenHeavyAttacks;
	    }

        public int GetHeavyDamage( int level ) {
            return BaseHeavyDamage;
        }
    }
}


