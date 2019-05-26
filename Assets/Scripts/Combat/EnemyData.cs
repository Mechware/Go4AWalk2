using System;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data.Combat {
	[CreateAssetMenu(menuName = "Data/Follower/Enemy")]
    public class EnemyData : FollowerData, ISaveable {

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
	    
        private const float BASE_ENEMY_HEALTH_SCALING = 5000;
	    private const float BASE_ENEMY_DAMAGE_SCALING = 50;
	    private const float BASE_ENEMY_ELEM_DAMAGE_SCALING = 2.5f;

	    [Header("Stats")]
        [Range(0, 500)]
        public float BaseHealth;
        [Range(0, 500)]
	    public float BaseDamage;

	    public float TimeBetweenAttacks;
        public float AttackPrepTime;
        public float AttackExecuteTime;

        [Header("Elemental")]
	    public bool HasElementalDamage;
	    public float BaseElementalDamage;
	    public ElementalType ElementalDamageType;
	    public ElementalWeaknessReference ElementalWeaknesses;

        [Header("Misc")]
		public ItemDropper Drops;
	    public bool OneAndDoneAttacker = false;

	    [NonSerialized] public int Level;

	    public int MaxHealth => Mathf.RoundToInt(BASE_ENEMY_HEALTH_SCALING * (1 + BaseHealth / 100) * (1 + Level / 10f));
	    public int Damage => Mathf.RoundToInt(BASE_ENEMY_DAMAGE_SCALING *(1 + BaseDamage / 100) * (1 + Level / 10f));
	    public int ElementalDamage => Mathf.RoundToInt(BASE_ENEMY_ELEM_DAMAGE_SCALING * (1 + BaseElementalDamage / 100) * (1 + Level / 10f));

	    public float GetElementalWeakness(ElementalType type) {
	        return ElementalWeaknesses.Value?[type] ?? 1;
	    }


        private class SaveObject {
	        public int ID;
	        public int Level;
	    }

	    public override string GetSaveString() {
	        return JsonUtility.ToJson(new SaveObject() { ID = ID, Level = Level});
        }


	    public override void SetData(string saveString, params object[] otherData) {

	        SaveObject ds = JsonUtility.FromJson<SaveObject>(saveString);

	        ID = ds.ID;
	        Level = ds.Level;

	        EnemyData original;

	        if(otherData[0] is PersistentSetFollowerData) {
	            PersistentSetFollowerData allFollowers = (PersistentSetFollowerData) otherData[0];
	            original = allFollowers.First(it => it.ID == ID) as EnemyData;
	        } else {
	            original = otherData[0] as EnemyData;
	            if(Idle == original.Idle)
	                return; // This object may have been create based on the original. In which case, we don't need to do any copying
            }

            // Copy Original Values
	        Idle = original.Idle;
            Flinch = original.Flinch;
            BeforeAttack = original.BeforeAttack;
            AttackExecute = original.AttackExecute;
            AfterAttack = original.AfterAttack;
            Death = original.Death;
            Dead = original.Dead;
            Walking = original.Walking;
	        ElementalWeaknesses = original.ElementalWeaknesses;
	        ElementalDamageType = original.ElementalDamageType;

            base.SetData(saveString, original);
        }
	}
}


