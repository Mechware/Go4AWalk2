using System;
using System.Diagnostics;
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

        public IntReference Level;
        public IntReference Experience;

        public WeaponReference Weapon;
        public ArmorReference Armor;
        public HeadgearReference Headgear;
        
		public void OnEnable() {
            Headgear.Variable.BeforeChange += UnequipHeadgear;
            Headgear.Variable.OnChange.AddListener(EquipHeadgear);

        }

		public void OnDisable() {
            Headgear.Variable.BeforeChange -= UnequipHeadgear;
            Headgear.Variable.OnChange.RemoveListener(EquipHeadgear);
        }

        public void UnequipHeadgear() {
            if(Headgear.Value != null) 
                MaxHealth.Value -= Headgear.Value.ExtraHealth;
        }

        public void EquipHeadgear(Headgear hg) {
            MaxHealth.Value += Headgear.Value.ExtraHealth;
        }



        public void DamagePlayer(int damage)
        {
            if (damage >= Health) {
                Health.Value = 0;
                InteractionController.Instance.OnPlayerDeath();
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

        [NonSerialized] public float DamageMultiplier = 1f;
        [NonSerialized] public float DamageAdditive = 0f;

        [NonSerialized] public float SpeedMultiplier = 1f;
        [NonSerialized] public float SpeedAdditive = 0f;

        public int GetLightDamage() {
            return Mathf.RoundToInt(Weapon.Value.RawDamage * DamageMultiplier + DamageAdditive);
		}

	    public int GetElementalDamage() {
	        return Weapon.Value.GetEnchantDamage();
	    }

        public float GetAttackSpeed() {
            return Weapon.Value.TapSpeed * SpeedMultiplier + SpeedAdditive;
        }

#if UNITY_EDITOR
        [ContextMenu("Restore Health")]
		private void ResetHealth() {
			Health.Value = MaxHealth;
		}
#endif
	}

}
