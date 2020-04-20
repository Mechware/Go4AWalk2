using System;
using System.Diagnostics;
using CustomEvents;
using UnityEngine;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace G4AW2.Combat {

	public class Player : MonoBehaviour {

		public static Player Instance;

		[NonSerialized] public PlayerSaveData SaveData;
		
		[NonSerialized] public double MaxHealth;

        public WeaponInstance Weapon;
        public ArmorInstance Armor;
        public HeadgearInstance Headgear;
        
        [NonSerialized] public float DamageMultiplier = 1f;
        [NonSerialized] public float DamageAdditive = 0f;

        [NonSerialized] public float SpeedMultiplier = 1f;
        [NonSerialized] public float SpeedAdditive = 0f;

        [NonSerialized] public float Weight;
        
        public int HealthPerSecond = 1;
        private float updateTime = 1f;
        
        private void Awake() {
	        Instance = this;
        }
        
        public void Initialize() {

	        SaveData = SaveGame.SaveData.Player;
	        Weight = Weapon.Config.Weight + Armor.Config.Weight;
        }
        
		public void EquipHeadgear(HeadgearInstance headgear) {
			if(Headgear != null) 
				MaxHealth -= Headgear.ExtraHealth;
			
			Headgear = headgear;
			
			if(Headgear != null) 
				MaxHealth += Headgear.ExtraHealth;
		}

        public void DamagePlayer(double damage)
        {
            if (damage >= SaveData.Health) {
                SaveData.Health = 0;
                InteractionController.Instance.OnPlayerDeath();
            }
            else {
	            SaveData.Health -= damage;
            }
        }

	    public void OnDeathFinished() {
		    SaveData.Health = MaxHealth;
	    }

        public double GetDamage() {
	        return Weapon.GetRandomDamage();
        }

	    public int GetElementalDamage() {
	        return Weapon.GetEnchantDamage();
	    }

        public float GetAttackSpeed() {
            return Random.Range(Weapon.Config.SpeedMin, Weapon.Config.SpeedMax) * SpeedMultiplier + SpeedAdditive;
        }

        public bool Dodge(float incomingSpeed) {
	        return Formulas.Dodge(Weight, incomingSpeed, SaveData.PerkDodge); 
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
			SaveData.Health = MaxHealth;
		}
#endif
	}

}
