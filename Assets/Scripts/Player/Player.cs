using System;
using System.Diagnostics;
using CustomEvents;
using UnityEngine;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;

namespace G4AW2.Combat {

	public class Player : MonoBehaviour {

		public static Player Instance;
		private void Awake() {
			Instance = this;
		}

		[NonSerialized] public int MaxHealth;

		[NonSerialized] public int Health;
		[NonSerialized] public int Gold;

		[NonSerialized] public int Level;
		[NonSerialized] public int Experience;

        public WeaponInstance Weapon;
        public ArmorInstance Armor;
        public HeadgearInstance Headgear;
        
        [NonSerialized] public float DamageMultiplier = 1f;
        [NonSerialized] public float DamageAdditive = 0f;

        [NonSerialized] public float SpeedMultiplier = 1f;
        [NonSerialized] public float SpeedAdditive = 0f;
        
		public void EquipHeadgear(HeadgearInstance headgear) {
			if(Headgear != null) 
				MaxHealth -= Headgear.ExtraHealth;
			
			Headgear = headgear;
			
			if(Headgear != null) 
				MaxHealth += Headgear.ExtraHealth;
		}

        public void DamagePlayer(int damage)
        {
            if (damage >= Health) {
                Health = 0;
                InteractionController.Instance.OnPlayerDeath();
            }
            else {
                Health -= damage;
            }
        }

	    public void OnDeathFinished() {
	        int oldAmount = Gold;
	        int newAmount = oldAmount - Mathf.RoundToInt(oldAmount * 0.2f);
	        newAmount = Mathf.Max(newAmount, 0);
	        Gold = newAmount;

            Health = MaxHealth;
	        PopUp.SetPopUp($"You died! You lost: {oldAmount - newAmount} gold.", new string[] {"Ok"}, new Action[] {() => { }});
	    }



        public int GetLightDamage() {
            return Mathf.RoundToInt(Weapon.RawDamage * DamageMultiplier + DamageAdditive);
		}

	    public int GetElementalDamage() {
	        return Weapon.GetEnchantDamage();
	    }

        public float GetAttackSpeed() {
            return Weapon.Config.TapSpeed * SpeedMultiplier + SpeedAdditive;
        }

#if UNITY_EDITOR
        [ContextMenu("Restore Health")]
		private void ResetHealth() {
			Health = MaxHealth;
		}
#endif
	}

}
