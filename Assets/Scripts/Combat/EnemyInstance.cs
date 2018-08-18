using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.Combat;
using UnityEngine;

namespace G4AW2.Combat {
	public class EnemyInstance : ScriptableObject {

		public Action OnDeath;

		public EnemyData Data;

		public int Level;
		public int CurrentHealth;
		public int CurrentArmor;
		public int CurrentAttackSpeed;
		public int CurrentDamage;

		public void SetLevel( int level ) {
			Level = level;
			CurrentHealth = Data.GetHealth(level);
			CurrentArmor = Data.GetArmor(level);
			CurrentAttackSpeed = Data.GetAttackSpeed(level);
			CurrentDamage = Data.GetDamage(level);
		}

		public void ApplyDamage(int damage) {
			Debug.Log("Applying damage: "+ damage);
			CurrentHealth -= damage;
			CurrentHealth = Mathf.Max(CurrentHealth, 0);
			if(CurrentHealth == 0 && OnDeath != null) OnDeath();
		}
	}
}

