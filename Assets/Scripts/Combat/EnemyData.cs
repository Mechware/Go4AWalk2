using G4AW2.Combat.Swiping;
using UnityEngine;

namespace G4AW2.Data.Combat {
	[CreateAssetMenu(menuName = "Data/Enemy")]
    public class EnemyData : FollowerData {

		public AnimationClip Idle;
		public AnimationClip Flinch;
		public AnimationClip LightAttack;
        public AnimationClip BeforeSwipeAttack;
		public AnimationClip SwipeAttack;
		public AnimationClip Death;
		public AnimationClip Dead;

		public int BaseHealth;
        public int BaseArmor;
	    public float TimeBetweenLightAttacks;
	    public float TimeBetweenHeavyAttacks;
	    public int BaseLightDamage;
	    public int BaseHeavyDamage;
        public SwipeSet Swipes;

	    public int GetHealth(int level) {
		    return BaseHealth;
	    }

	    public int GetArmor( int level ) {
		    return BaseArmor;
	    }

	    public float GetTimeBetweenLightAttacks( int level ) {
		    return TimeBetweenLightAttacks;
	    }

	    public float GetTimeBetweenHeavyAttacks(int level) {
		    return TimeBetweenHeavyAttacks;
	    }

	    public int GetLightDamage(int level) {
		    return BaseLightDamage;
	    }

        public int GetHeavyDamage( int level ) {
            return BaseHeavyDamage;
        }
    }
}


