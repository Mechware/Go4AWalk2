using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data.Combat
{
    [CreateAssetMenu(menuName = "Data/Follower/Enemy")]
    public class EnemyConfig : FollowerConfig {

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
	    
	    [Header("Stats")]
        public float HealthAtLevel0;
	    public float DamageAtLevel0;

	    public float TimeBetweenAttacks;
        public float AttackPrepTime;
        public float AttackExecuteTime;

        [Header("Elemental")]
	    public bool HasElementalDamage;
	    public float ElementalDamageAtLevel0;
	    public ElementalType ElementalDamageType;
	    public ElementalWeakness ElementalWeaknesses;

        [Header("Misc")]
		public ItemDropper Drops;
	    public bool OneAndDoneAttacker = false;
    }
}


