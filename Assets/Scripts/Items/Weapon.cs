using System;
using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    [CreateAssetMenu(menuName = "Data/Items/Weapon")]
    public class Weapon : Item, ISaveable, ITrashable {
        public int TapsWithWeapon = 1;
        public int ActualDamage => Mathf.RoundToInt( (Level == 99 ? 2.15f * Damage :  Damage * ( 1 + Level / 100f)) * mod);
        public int Level => Mathf.RoundToInt( ConfigObject.GetLevel(Rarity, TapsWithWeapon));
        public int Damage;

        [NonSerialized]
        public bool MarkedAsTrash = false;

        private int random = -1;
        private float mod;
        private string nameMod;

        public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
            return true;
        }

        public override void OnAfterObtained(Item original) {

            ID = original.ID;
            name = original.name;
            Image = original.Image;
            Value = original.Value;
            Description = original.Description;
            Rarity = original.Rarity;
            Damage = ((Weapon)original).Damage;

            if(random != -1) {
                SetValuesBasedOnRandom();
            } else {
                random = UnityEngine.Random.Range(0, 101);
                SetValuesBasedOnRandom();
                TapsWithWeapon = 0;
            }
        }

        public override string GetName() {
            return $"{nameMod} {name}";
        }

        public override string GetDescription() {
            return $"Level: {Level}\nDamage: {ActualDamage}\n{Description}";
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
        }

        public string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() {ID = ID, Random = random, Taps = TapsWithWeapon, Trash = MarkedAsTrash});
        }

        public void SetData(string saveString, params object[] otherData) {

            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            ID = ds.ID;
            random = ds.Random;
            TapsWithWeapon = ds.Taps;
            MarkedAsTrash = ds.Trash;

            Weapon original;

            if (otherData[0] is PersistentSetItem) {
                PersistentSetItem allItems = (PersistentSetItem) otherData[0];
                original = allItems.First(it => it.ID == ID) as Weapon;
            }
            else {
                original = otherData[0] as Weapon;
            }

            // Copy Original Values
            OnAfterObtained(original);
        }

        public bool IsTrash() {
            return MarkedAsTrash;
        }

        public void SetTrash(bool isTrash) {
            MarkedAsTrash = isTrash;
        }
    }
}


