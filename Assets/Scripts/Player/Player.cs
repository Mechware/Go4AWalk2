using CustomEvents;
using G4AW2.Combat.Swiping;
using UnityEngine;
using G4AW2.Data.DropSystem;

namespace G4AW2.Combat {

	[CreateAssetMenu(menuName = "Data/Player")]
	public class Player : ScriptableObject {

		public IntReference MaxHealth;

		public IntReference Health;
		public FloatReference Power;
        public FloatReference Armor;
		public IntReference Damage;

		public FloatReference PowerPerBlock;
		public GameEvent OnPowerMax;

        public Item hat, armor, weapon, boots, accessory;

		public void OnEnable() {
			MaxHealth.Value = PlayerPrefs.GetInt("PlayerMaxHealth", 100);
			Health.Value = PlayerPrefs.GetInt("PlayerHealth", 100);
			Power.Value = PlayerPrefs.GetInt("PlayerPower", 0);
			Damage.Value = PlayerPrefs.GetInt("PlayerDamage", 1);
		}

		public void OnDisable() {
			PlayerPrefs.SetInt("PlayerMaxHealth", MaxHealth.Value);
			PlayerPrefs.SetInt("PlayerHealth", Health.Value);
			PlayerPrefs.SetFloat("PlayerPower", Power.Value);
			PlayerPrefs.SetInt("PlayerDamage", Damage.Value);
		}

		public int GetLightDamage() {
			return Damage;
		}

		public void Hit( int damage ) {
			Health.Value -= Mathf.RoundToInt(damage*Armor/100);
		}

		public void Block( Swipe s ) {
			Power.Value = Mathf.Min(Power.Value + PowerPerBlock, 100f);
			if (Power.Value == 100f) {
				OnPowerMax.Raise();
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Restore Health")]
		private void ResetHealth() {
			Health.Value = MaxHealth;
		}
#endif
	}

}
