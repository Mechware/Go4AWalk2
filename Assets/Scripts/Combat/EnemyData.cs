using G4AW2.Combat.Swiping;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data.Combat {
	[CreateAssetMenu(menuName = "Data/Enemy")]
    public class EnemyData : FollowerData {

		[Header("Animations")]
		public AnimationClip Idle;
		public AnimationClip Flinch;
		public AnimationClip LightAttack;
        public AnimationClip BeforeSwipeAttack;
		public AnimationClip SwipeAttack;
		public AnimationClip Death;
		public AnimationClip Dead;

		[Header("Stats")]
		public int BaseHealth;
        public int BaseArmor;
	    public float TimeBetweenLightAttacks;
	    public float TimeBetweenHeavyAttacks;
	    public int BaseLightDamage;
	    public int BaseHeavyDamage;

		[Header("Misc")]
        public SwipeSet Swipes;
		public ItemDropper Drops;

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


