using System;
using System.Diagnostics;
using CustomEvents;
using UnityEngine;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using Debug = UnityEngine.Debug;

namespace G4AW2.Combat {

	public class Player : MonoBehaviour {

		public static Player Instance;


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
        
        public int HealthPerSecond = 1;
        private float updateTime = 1f;
        
        private void Awake() {
	        Instance = this;
        }
        
        public void Initialize() {

	        // Fin
	        DateTime lastTimePlayedUTC = SaveGame.SaveData.LastTimePlayedUTC;
	        TimeSpan TimeSinceLastPlayed = DateTime.UtcNow - lastTimePlayedUTC;
        }
        
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
            Health = MaxHealth;
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

        
        // Update is called once per frame
        void Update () {
	        updateTime -= Time.deltaTime;
	        if (updateTime < 0) {
		        updateTime = 1f;
	        }
        }

        private DateTime PauseTime = DateTime.MaxValue;

        private void OnApplicationFocus(bool focus) {
	        if (focus) {
		        // Played
		        if (PauseTime == DateTime.MaxValue) {
			        Debug.LogWarning("Just played without pausing");
			        return;
		        }
	        }
	        else {
		        // Paused
		        PauseTime = DateTime.Now;
	        }
        }

        private void OnApplicationPause(bool pause) {
	        OnApplicationFocus(!pause);
        }
        
        
#if UNITY_EDITOR
        [ContextMenu("Restore Health")]
		private void ResetHealth() {
			Health = MaxHealth;
		}
#endif
	}

}
