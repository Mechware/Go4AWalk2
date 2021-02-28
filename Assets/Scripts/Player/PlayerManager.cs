using G4AW2.Data;
using G4AW2.Data.DropSystem;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace G4AW2.Managers
{

    [CreateAssetMenu(menuName = "Managers/Player")]
    public class PlayerManager : ScriptableObject {

		[NonSerialized] public int MaxHealth = 2000;
        [NonSerialized] public int Gold;
        [NonSerialized] public int Level;
        [NonSerialized] public int Experience;
        public int Health { private set; get; }

        public Action<float> OnDeath;
        public Action<int, ElementalType> OnDamageTaken;
        public Action WeaponChanged;
        public Action ArmorChanged;
        public Action HeadgearChanged;

        public WeaponInstance Weapon { private set; get; }
        public ArmorInstance Armor { private set; get; }
        public HeadgearInstance Headgear { private set; get; }


        [SerializeField] private WeaponConfig StartWeapon;
        [SerializeField] private ArmorConfig StartArmor;

        public int HealthPerSecond = 1;
        private float updateTime = 1f;
        
        public void Initialize(bool newGame) {

            if(newGame)
            {
                Health = MaxHealth;

                Weapon = new WeaponInstance(StartWeapon, 1);
                Weapon.SaveData.Level = 1;
                Weapon.SaveData.Random = 30;

                Armor = new ArmorInstance(StartArmor, 1);
                Armor.SaveData.Level = 1;
                Armor.SaveData.Random = 50;
            }

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

            HeadgearChanged?.Invoke();
		}

        public void EquipWeapon(WeaponInstance weapon)
        {
            Weapon = weapon;
            WeaponChanged?.Invoke();
        }

        public void EquipArmor(ArmorInstance armor)
        {
            Armor = armor;
            ArmorChanged?.Invoke();
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
            Debug.Log($"health: {Health}");
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
            return Mathf.RoundToInt(Weapon.RawDamage);
		}

	    public int GetElementalDamage() {
	        return Weapon.GetEnchantDamage();
	    }

        public float GetAttackSpeed() {
            return Weapon.Config.TapSpeed;
        }

        
        // Update is called once per frame
        void Update () {
	        updateTime -= Time.deltaTime;
	        if (updateTime < 0) {
		        updateTime = 1f;
		        IncreaseHealthByTime(1);
	        }
        }

        public void GiveHealth(int amount)
        {
            Health = Mathf.Min(Health + amount, MaxHealth);
            Debug.Log($"health: {Health}");
        }

        public void IncreaseHealthByTime(float time) {
            if (time < 0 || time > int.MaxValue)
            {
                Health = MaxHealth;
                return;
            }
            GiveHealth(Mathf.RoundToInt(Health + time * HealthPerSecond));
        }

        private DateTime PauseTime = DateTime.MaxValue;

        private void OnApplicationFocus(bool focus) {
	        if (focus) {
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
