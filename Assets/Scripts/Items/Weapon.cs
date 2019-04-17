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

        public PersistentSetItem AllItems;

        public int ActualDamage => Mathf.RoundToInt( Damage * mod * MasteryDamageMod * LevelDamageMod);
        public int Mastery => Mathf.RoundToInt( ConfigObject.GetLevel(Rarity, MasteryLevels.GetTaps(ID)));
        private float MasteryDamageMod => Mastery == 99 ? 2.15f : 1 + Mastery / 100f;
        private float LevelDamageMod => 1 + Level / 10f; 

        public int Damage;

        public bool IsEnchanted { get { return enchantment != null; } }
        public Enchanter Enchantment { get { return enchantment; } }

        public GameEventWeapon LevelUp;

        [NonSerialized]
        public bool MarkedAsTrash = false;
        [NonSerialized]
        public ObservableInt TapsWithWeapon = new ObservableInt(1);
        [NonSerialized]
        public int Level = 1;

        private int random = -1;
        private float mod;
        private string nameMod;

        private Enchanter enchantment;

        void OnEnable() {
            TapsWithWeapon.OnValueChange += TapsChanged;
        }

        private int lastLevel = -1;
        void TapsChanged(int amount) {
            if (lastLevel == -1) {
                lastLevel = Mastery;
                return;
            }

            MasteryLevels.Tap(ID);

            if (Mastery != lastLevel) {
                LevelUp.Raise(this);
                DataChanged?.Invoke();
                lastLevel = Mastery;
            }
        }

        public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
            return true;
        }

        public override void OnAfterObtained() {

            random = UnityEngine.Random.Range(0, 101);
            SetValuesBasedOnRandom();
            TapsWithWeapon.Value = 0;
        }

        public override string GetName() {
            return $"{nameMod} {name}";
        }

        public override string GetDescription() {
            return $"Level: {Level}\nMastery: {Mastery}\nDamage: {ActualDamage}\n{Description}";
        }

        public void SetValuesBasedOnRandom() {
            if(random == 0) {
                mod = 0.5f;
                nameMod = "Broken";
            } else if(random >= 1 && random <= 10) {
                mod = 0.7f;
                nameMod = "Damaged";
            } else if(random >= 11 && random <= 30) {
                mod = 0.85f;
                nameMod = "Inferior";
            } else if(random >= 31 && random <= 70) {
                mod = 1;
                nameMod = "Normal";
            } else if(random >= 71 && random <= 90) {
                mod = 1.15f;
                nameMod = "Fine";
            } else if(random >= 91 && random <= 99) {
                mod = 1.3f;
                nameMod = "Exquisite";
            } else if(random == 100) {
                mod = 1.5f;
                nameMod = "Masterwork";
            }
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
            return JsonUtility.ToJson(new DummySave() {ID = ID, Random = random, Taps = TapsWithWeapon, Trash = MarkedAsTrash, Level = Level, EnchantID = enchantment == null ? -1 : enchantment.ID, EnchantSave = enchantment == null ? "" : enchantment.GetSaveString()});
        }

        public void SetData(string saveString, params object[] otherData) {

            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            ID = ds.ID;
            random = ds.Random;
            TapsWithWeapon.Value = ds.Taps;
            MarkedAsTrash = ds.Trash;
            Level = ds.Level;

            if(ds.EnchantID != -1 && CreatedFromOriginal) {
                Enchanter og = AllItems.First(it => it.ID == ds.EnchantID) as Enchanter;
                enchantment = Instantiate(og);
                enchantment.CreatedFromOriginal = true;
                enchantment.SetData(ds.EnchantSave, og);
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
            CopyValues(original);
            Damage = original.Damage;
            LevelUp = original.LevelUp;
            AllItems = original.AllItems;

            if(ds.EnchantID != -1) {
                Enchanter og = AllItems.First(it => it.ID == ds.EnchantID) as Enchanter;
                enchantment = Instantiate(og);
                enchantment.CreatedFromOriginal = true;
                enchantment.SetData(ds.EnchantSave, og);
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
            enchantment = e;
        }

        public int GetEnchantDamage() {
            return enchantment == null ? 0 : Mathf.RoundToInt(enchantment.GetAdditiveDamage(this));
        }
    }
}


