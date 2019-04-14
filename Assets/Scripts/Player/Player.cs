using System;
using CustomEvents;
using UnityEngine;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;

namespace G4AW2.Combat {

	[CreateAssetMenu(menuName = "Data/Player")]
	public class Player : ScriptableObject {

		public IntReference MaxHealth;

		public IntReference Health;
	    public IntReference Gold;

		public FloatReference PowerPerBlock;
		public GameEvent OnPowerMax;

	    public GameEvent PlayerDeath;

        public WeaponReference Weapon;
        public ArmorReference Armor;

		public void OnEnable() {
		}

		public void OnDisable() {
		}

        public void DamagePlayer(int damage)
        {
            if (damage >= Health) {
                PlayerDeath.Raise();
                Health.Value = 0;
            }
            else {
                Health.Value -= damage;
            }
        }

	    public void OnDeathFinished() {
	        int oldAmount = Gold;
	        int newAmount = oldAmount - Mathf.RoundToInt(oldAmount * 0.2f);
	        newAmount = Mathf.Max(newAmount, 0);
	        Gold.Value = newAmount;

            Health.Value = MaxHealth;
	        PopUp.SetPopUp($"You died! You lost: {oldAmount - newAmount} gold.", new string[] {"Ok"}, new Action[] {() => { }});
	    }

		public int GetLightDamage() {
            return Weapon.Value.ActualDamage;
		}

#if UNITY_EDITOR
		[ContextMenu("Restore Health")]
		private void ResetHealth() {
			Health.Value = MaxHealth;
		}
#endif
	}

}
