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

	    [Header("Stats")]
		public AnimationCurve HealthScaling;
	    public AnimationCurve TimeBetweenHeavyAttacksScaling;
	    public AnimationCurve DamageScaling;

        public float AttackPrepTime;
        public float AttackExecuteTime;

		[Header("Misc")]
		public ItemDropper Drops;

	    [NonSerialized] public int Level;

	    public int MaxHealth => Mathf.RoundToInt(HealthScaling.Evaluate(Level));
	    public int TimeBetweenHeavyAttack => Mathf.RoundToInt(TimeBetweenHeavyAttacksScaling.Evaluate(Level));
	    public int Damage => Mathf.RoundToInt(DamageScaling.Evaluate(Level));

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

            base.SetData(saveString, original);
        }
	}
}


