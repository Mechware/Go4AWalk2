using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.Combat;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Combat {
    [Serializable]
	public class EnemyInstance : ScriptableObject {

		public Action OnDeath;

		public EnemyData Data;

		public int Level;
        public int MaxHealth;
		public ObservableInt CurrentHealth = new ObservableInt(100);
        public ObservableInt CurrentCrit = new ObservableInt(0);


		public int CurrentArmor;
		public float CurrentAttackSpeed;
		public int CurrentDamage;

        public void Reset() {
            CurrentHealth.Value = MaxHealth;
            CurrentCrit.Value = 0;
        }

		public void SetLevel( int level ) {
			Level = level;
		    MaxHealth = Data.GetHealth(level);
		    CurrentHealth.Value = MaxHealth;
            CurrentArmor = Data.GetArmor(level);
			CurrentAttackSpeed = Data.GetAttackSpeed(level);
			CurrentDamage = Data.GetDamage(level);
		}

		public void ApplyDamage(int damage) {
			CurrentHealth.Value -= damage;
			if(CurrentHealth.Value <= 0 && OnDeath != null) OnDeath();
		}
	}
}

