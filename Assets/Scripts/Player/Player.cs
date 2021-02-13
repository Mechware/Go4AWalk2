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

        /// <summary>
        /// Float parameter is the gold loss
        /// </summary>
        public Action<float> OnDeath;

        /// <summary>
        /// Param is damage dealt
        /// </summary>
        public Action<int, ElementalType> OnDamageTaken;

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

        public void DamagePlayer(float dmg, ElementalType type)
        {
            float mod = 1;
            if (Armor.Value.ElementalWeakness.Value != null && type != null)
                mod = Armor.Value.ElementalWeakness.Value[type];
            dmg *= mod;

            int damage = Mathf.RoundToInt(dmg);

            OnDamageTaken?.Invoke(damage, type);

            if (damage >= Health) {
                Health.Value = 0;
                Die();
            }
            else {
                Health.Value -= damage;
            }
        }

        public void Die()
        {
            int oldAmount = Gold;
            int newAmount = oldAmount - Mathf.RoundToInt(oldAmount * 0.2f);
            newAmount = Mathf.Max(newAmount, 0);
            Gold.Value = newAmount;

            Health.Value = MaxHealth;
            OnDeath?.Invoke(oldAmount - newAmount);
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
