using G4AW2.Data;
using G4AW2.Data.DropSystem;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace G4AW2.Managers
{
    public class PlayerManager : MonoBehaviour {

		public int MaxHealth { private set; get; } = 2000;
        public int Gold { private set; get; }
        public int Level { private set; get; }
        public int Experience { private set; get; }
        public int Health { private set; get; }
        public WeaponInstance Weapon { private set; get; }
        public ArmorInstance Armor { private set; get; }
        public HeadgearInstance Headgear { private set; get; }

        public Action<float> OnDeath;
        public Action<int, ElementalType> OnDamageTaken;
        public Action WeaponChanged;
        public Action ArmorChanged;
        public Action HeadgearChanged;

        [SerializeField] private WeaponConfig _startWeapon;
        [SerializeField] private ArmorConfig _startArmor;
        [SerializeField] private SaveManager _saveManager;
        [SerializeField] private ItemManager _itemManager;
        [SerializeField] private int _healthPerUnitTime = 1;
        [SerializeField] private float _unitTime = 1f;
        [SerializeField] private int _startMaxHealth = 2000;

        private void Awake()
        {
            _saveManager.RegisterSaveFunction("Player", Save);
            _saveManager.RegisterLoadFunction("Player", Load);
        }

        private class SaveData
        {
            public int Gold;
            public int Level;
            public int Experience;
            public int Health;
            public WeaponSaveData Weapon;
            public ArmorSaveData Armor;
            public HeadgearSaveData Headgear;
        }

        private void Load(object o)
        {
            MaxHealth = _startMaxHealth;

            if (o == null)
            {
                Health = MaxHealth;

                Weapon = new WeaponInstance(_startWeapon, 1);
                Weapon.SaveData.Level = 1;
                Weapon.SaveData.Random = 30;

                Armor = new ArmorInstance(_startArmor, 1);
                Armor.SaveData.Level = 1;
                Armor.SaveData.Random = 50;
                return;
            }

            SaveData sd = (SaveData)o;
            Gold = sd.Gold;
            Level = sd.Level;
            Experience = sd.Experience;
            Health = sd.Health;
            if (sd.Weapon != null) EquipWeapon((WeaponInstance)_itemManager.CreateInstance(sd.Weapon));
            if (sd.Armor != null) EquipArmor((ArmorInstance)_itemManager.CreateInstance(sd.Armor));
            if(sd.Headgear != null) EquipHeadgear((HeadgearInstance)_itemManager.CreateInstance(sd.Headgear));
        }

        private object Save()
        {
            return new SaveData()
            {
                Gold = Gold,
                Level = Level,
                Experience = Experience,
                Health = Health,
                Weapon = Weapon?.SaveData,
                Armor = Armor?.SaveData,
                Headgear = Headgear?.SaveData
            };
        }

        public void Initialize(bool newGame) {
            if(!newGame)
            {
                var secondsSinceLastPlayed = GlobalSaveData.SaveData.GetTimeSinceLastPlayed();
                IncreaseHealthByTime((int)secondsSinceLastPlayed);
            }
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
            int reduceAmount = Mathf.RoundToInt(Gold * 0.2f);
            TakeGold(reduceAmount);

            Health = MaxHealth;
            OnDeath?.Invoke(reduceAmount);
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

        private float _updateTime = 0;
        // Update is called once per frame
        void Update () {
            _updateTime -= Time.deltaTime;
	        if (_updateTime < 0) {
                _updateTime = _unitTime;
		        IncreaseHealthByTime(_healthPerUnitTime);
	        }
        }

        public void GiveGold(int amount)
        {
            Gold += amount;
        }

        public void TakeGold(int amount)
        {
            Gold -= amount;
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
            GiveHealth(Mathf.RoundToInt(Health + time * (_healthPerUnitTime / _unitTime)));
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
