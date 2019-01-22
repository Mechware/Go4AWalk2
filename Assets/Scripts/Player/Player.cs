using CustomEvents;
using G4AW2.Combat.Swiping;
using UnityEngine;
using G4AW2.Data.DropSystem;

namespace G4AW2.Combat {

	[CreateAssetMenu(menuName = "Data/Player")]
	public class Player : ScriptableObject {

		public IntReference MaxHealth;

		public IntReference Health;
        public FloatReference Armor;
		public IntReference Damage;

		public FloatReference PowerPerBlock;
		public GameEvent OnPowerMax;

        public RuntimeSetItem inventory;

        public Item hat, armor, weapon, boots, accessory;

		public void OnEnable() {
            if (weapon != null) Damage.Value = weapon.value;
		}

		public void OnDisable() {
		}

		public int GetLightDamage() {
			return Damage;
		}


#if UNITY_EDITOR
		[ContextMenu("Restore Health")]
		private void ResetHealth() {
			Health.Value = MaxHealth;
		}
#endif
	}

}
