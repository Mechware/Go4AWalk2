using CustomEvents;
using UnityEngine;
using G4AW2.Data.DropSystem;

namespace G4AW2.Combat {

	[CreateAssetMenu(menuName = "Data/Player")]
	public class Player : ScriptableObject {

		public IntReference MaxHealth;

		public IntReference Health;

		public FloatReference PowerPerBlock;
		public GameEvent OnPowerMax;

        public WeaponReference Weapon;
        public ArmorReference Armor;

		public void OnEnable() {
		}

		public void OnDisable() {
		}

        public void DamagePlayer(int damage)
        {
            Health.Value -= damage;
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
