using System;
using CustomEvents;
using System.Linq;
using UnityEngine;

namespace G4AW2.Data.DropSystem {
    [CreateAssetMenu(menuName = "Data/Items/Armor")]
    public class Armor : Item, ISaveable, ITrashable {


        public float NoBlockModifierWithMod {
            get {
                return mod * NoBlockModifier;
            }
        }

        public float PerfectBlockModifierWithMod {
            get {
                return mod * PerfectBlockModifier;
            }
        }

        public float MistimedBlockModifierWithMod {
            get {
                return mod * MistimedBlockModifier;
            }
        }

        public float DamageAdditiveModifierWithMod {
            get {
                return mod * DamageAdditiveModifier;
            }
        }

        public float NoBlockModifier;
        public float PerfectBlockModifier;
        public float MistimedBlockModifier;
        public float DamageAdditiveModifier;

        [NonSerialized]
        public bool IsMarkedTrash = false;

        public float GetDamage(int damage, bool perfectBlock, bool mistimedBlock, bool badParry) {
            float fdamage = damage - DamageAdditiveModifierWithMod;
            fdamage = Mathf.Max(0, fdamage);

            if(perfectBlock) {
                return fdamage * PerfectBlockModifierWithMod;
            }

            if(mistimedBlock) {
                return fdamage * MistimedBlockModifierWithMod;
            }

            if(badParry) {
                return fdamage;
            }

            return fdamage * NoBlockModifierWithMod;
        }

        private int random = -1;
        private float mod;
        private string nameMod;

        public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
            return true;
        }

        public override void OnAfterObtained() {

            random = UnityEngine.Random.Range(0, 101);
            SetValuesBasedOnRandom();
        }

        public override string GetName() {
            return $"{nameMod} {name}";
        }

        public override string GetDescription() {
            return $"No Block Armor: {NoBlockModifierWithMod}\n" +
                   $"Perfect Block Armor: {PerfectBlockModifierWithMod}\n" +
                   $"Mistimed Block Armor: {MistimedBlockModifierWithMod}\n" +
                   $"Damage Subtraction: {DamageAdditiveModifierWithMod}\n" +
                   $"{Description}";
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
            public bool Trash;
        }

        public string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() { ID = ID, Random = random, Trash = IsMarkedTrash });
        }

        public void SetData(string saveString, params object[] otherData) {

            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            ID = ds.ID;
            random = ds.Random;
            IsMarkedTrash = ds.Trash;

            SetValuesBasedOnRandom();

            if(CreatedFromOriginal)
                return;

            Armor original;

            if(otherData[0] is PersistentSetItem) {
                PersistentSetItem allItems = (PersistentSetItem) otherData[0];
                original = allItems.First(it => it.ID == ID) as Armor;
            } else {
                original = otherData[0] as Armor;
            }

            // Copy Original Values
            //OnAfterObtained(original);
            base.CopyValues(original);
            NoBlockModifier = original.NoBlockModifier;
            PerfectBlockModifier = original.PerfectBlockModifier;
            MistimedBlockModifier = original.MistimedBlockModifier;
            DamageAdditiveModifier = original.DamageAdditiveModifier;
        }

        public bool IsTrash() {
            return IsMarkedTrash;
        }

        public void SetTrash(bool isTrash) {
            IsMarkedTrash = isTrash;
            DataChanged?.Invoke();
        }
    }
}


