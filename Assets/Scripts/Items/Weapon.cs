using System;
using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    [CreateAssetMenu(menuName = "Data/Items/Weapon")]
    public class Weapon : Item, ISaveable, ITrashable {

        public float TapSpeed = 2f;

        public int RawDamage => GetDamage();
        public int Mastery => Mathf.FloorToInt( ConfigObject.GetLevel(Rarity, MasteryLevels.GetTaps(ID)));
        public float RawMastery => ConfigObject.GetLevel(Rarity, MasteryLevels.GetTaps(ID));

        public float DamageAtLevel0;

        public int GetDamage(int? mastery = null, float? damageAtLevel0 = null, int? level = null, float? mod = null, float? additiveDamage = null, float? damageMultiple = null) {
            int imastery = mastery ?? Mastery;
            float fdamageAtLevel0 = damageAtLevel0 ?? DamageAtLevel0;
            float fmod = mod ?? this.mod;
            float ilevel = level ?? Level;
            
            float masteryMod = imastery == 99 ? 2.15f : 1 + imastery / 100f;
            float baseRawDamage = fdamageAtLevel0 * masteryMod * (1 + ilevel / 10f) * fmod;
            return Mathf.RoundToInt(baseRawDamage);
        }

        public int GetDamage() {
            float masteryMod = Mastery == 99 ? 2.15f : 1 + Mastery / 100f;
            float baseRawDamage = DamageAtLevel0 * masteryMod * (1 + Level / 10f) * mod;
            return Mathf.RoundToInt(baseRawDamage);
        }
        
        public bool IsEnchanted => Enchantment != null;
        public Enchanter Enchantment { get; private set; }

        public GameEventWeapon LevelUp;

        [NonSerialized]
        public bool MarkedAsTrash = false;
        [NonSerialized]
        public ObservableInt TapsWithWeapon = new ObservableInt(1);
        [NonSerialized]
        public int Level = 1;

        [NonSerialized]
        public int Random = -1;
        private float mod;
        private string nameMod;


        void OnEnable() {
            TapsWithWeapon.OnValueChange += TapsChanged;
            if(Name == "")
                Name = name;
        }

        private int lastLevel = -1;
        void TapsChanged(int amount) {

            if (!MasteryLevels.Loaded) return;

            if (lastLevel == -1) {
                lastLevel = Mastery;
                return;
            }

            MasteryLevels.Tap(ID);

            if (Mastery != lastLevel) {
                
                WalkingUIController.Instance.MasteryPopUp($"Mastery Level {Mastery}");
                DataChanged?.Invoke();
                lastLevel = Mastery;
            }
        }

        public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
            return true;
        }

        public override void OnAfterObtained() {

            Random = UnityEngine.Random.Range(0, 101);
            SetValuesBasedOnRandom();
            TapsWithWeapon.Value = 0;
        }

        public override string GetName() {
            return GetName(IsEnchanted, true);
        }

        public string GetName(bool enchantInclude, bool includeNameMod) {
            if (enchantInclude && includeNameMod) {
                return $"{Enchantment.GetPrefix()} {nameMod} {base.GetName()}";
            }
            if (enchantInclude) {
                return $"{Enchantment.GetPrefix()} {base.GetName()}";
            }
            if (includeNameMod) {
                return $"{nameMod} {base.GetName()}";
            }

            return base.GetName();
            
        }

        public override string GetDescription() {
            if (IsEnchanted) {
                return $"Level: {Level}\nMastery: {Mastery}\nDamage: {RawDamage}\n{Enchantment.Type.name} Damage: {GetEnchantDamage()}\nValue: {GetValue()}\n{Description}";
            }
            return $"Level: {Level}\nMastery: {Mastery}\nDamage: {RawDamage}\nValue: {GetValue()}\n{Description}";
        }

        public override int GetValue() {
            return Mathf.RoundToInt(Value * (1 + Level / 10f) * (1 + Random / 100f)) + (IsEnchanted ? Enchantment.Value : 0);
        }

        public void SetValuesBasedOnRandom() {
            mod = ModRoll.GetMod(Random);
            nameMod = ModRoll.GetName(Random);
        }


        private class DummySave {
            public int ID;
            public int Random;
            public int Taps = 0;
            public bool Trash = false;
            public int Level = 1;
            public int EnchantID = -1;
            public string EnchantSave = "";
        }

        public string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() {ID = ID, Random = Random, Taps = TapsWithWeapon, Trash = MarkedAsTrash, Level = Level, EnchantID = Enchantment == null ? -1 : Enchantment.ID, EnchantSave = Enchantment == null ? "" : Enchantment.GetSaveString()});
        }

        public void SetData(string saveString, params object[] otherData) {

            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            ID = ds.ID;
            Random = ds.Random;
            TapsWithWeapon.Value = ds.Taps;
            MarkedAsTrash = ds.Trash;
            Level = ds.Level;

            if(ds.EnchantID != -1 && CreatedFromOriginal) {
                Enchanter og = DataManager.Instance.AllItems.First(it => it.ID == ds.EnchantID) as Enchanter;
                Enchantment = Instantiate(og);
                Enchantment.CreatedFromOriginal = true;
                Enchantment.SetData(ds.EnchantSave, og);
            }

            SetValuesBasedOnRandom();

            if(CreatedFromOriginal)
                return;

            Weapon original;

            if (otherData[0] is PersistentSetItem) {
                PersistentSetItem allItems = (PersistentSetItem) otherData[0];
                original = allItems.First(it => it.ID == ID) as Weapon;
            }
            else {
                original = otherData[0] as Weapon;
            }

            // Copy Original Values
            base.CopyValues(original);
            DamageAtLevel0 = original.DamageAtLevel0;
            LevelUp = original.LevelUp;

            if(ds.EnchantID != -1) {
                Enchanter og = DataManager.Instance.AllItems.First(it => it.ID == ds.EnchantID) as Enchanter;
                Enchantment = Instantiate(og);
                Enchantment.CreatedFromOriginal = true;
                Enchantment.SetData(ds.EnchantSave, og);
            }
        }

        public bool IsTrash() {
            return MarkedAsTrash;
        }

        public void SetTrash(bool isTrash) {
            MarkedAsTrash = isTrash;
            DataChanged?.Invoke();
        }

        public void Enchant(Enchanter e) {
            Enchantment = e;
        }

        public int GetEnchantDamage() {
            return Enchantment == null ? 0 : Mathf.RoundToInt(Enchantment.GetAdditiveDamage(this));
        }
    }
}


