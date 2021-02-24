using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace G4AW2.Managers
{

    public class PlayerManager : MonoBehaviour {

		public static PlayerManager Instance;


		[NonSerialized] public int MaxHealth;

		[NonSerialized] public int Health;
		[NonSerialized] public int Gold;
        public Action<float> OnDeath;
        public Action<int, ElementalType> OnDamageTaken;

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
	        double secondsSinceLastPlayed = TimeSinceLastPlayed.TotalSeconds;
	        IncreaseHealthByTime((int)secondsSinceLastPlayed);
        }
        
		public void EquipHeadgear(HeadgearInstance headgear) {
			if(Headgear != null) 
				MaxHealth -= Headgear.ExtraHealth;
			
			Headgear = headgear;
			
			if(Headgear != null) 
				MaxHealth += Headgear.ExtraHealth;
		}

        public void DamagePlayer(float dmg, ElementalType type)
        {
            float mod = 1;
            if (Armor.Config.ElementalWeakness != null && type != null)
                mod = Armor.Config.ElementalWeakness[type];
            dmg *= mod;

            int damage = Mathf.RoundToInt(dmg);

            OnDamageTaken?.Invoke(damage, type);

            if (damage >= Health) {
                Health = 0;
                Die();
            }
            else {
                Health -= damage;
            }
        }

        public void Die()
        {
            int oldAmount = Gold;
            int newAmount = oldAmount - Mathf.RoundToInt(oldAmount * 0.2f);
            newAmount = Mathf.Max(newAmount, 0);
            Gold = newAmount;

            Health = MaxHealth;
            OnDeath?.Invoke(oldAmount - newAmount);
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
		        IncreaseHealthByTime(1);
	        }
        }

        public void IncreaseHealthByTime(float time) {
	        Health = Mathf.Min(Mathf.RoundToInt(Health + time * HealthPerSecond), MaxHealth);
        }

        private DateTime PauseTime = DateTime.MaxValue;

        private void OnApplicationFocus(bool focus) {
	        if (focus) {
		        // Played
		        if (PauseTime == DateTime.MaxValue) {
			        Debug.LogWarning("Just played without pausing");
			        return;
		        }

		        TimeSpan diff = DateTime.Now - PauseTime;
		        IncreaseHealthByTime((float)diff.TotalSeconds);
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
