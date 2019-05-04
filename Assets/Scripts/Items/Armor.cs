using System;
using CustomEvents;
using System.Linq;
using UnityEngine;

namespace G4AW2.Data.DropSystem {
    [CreateAssetMenu(menuName = "Data/Items/Armor")]
    public class Armor : Item, ISaveable, ITrashable {

        public float BaseARMValue;
        public ElementalWeaknessReference ElementalWeakness;

        [NonSerialized]
        public bool IsMarkedTrash = false;

        [NonSerialized]
        public int Level;

        private int random = -1;
        private float mod;
        private string nameMod;

        
        private float NoBlockModifierWithMod => Mathf.Max(1 - ARMValue / 100, 0);
        private float PerfectBlockModifierWithMod => Mathf.Max(1 - 1.5f * ARMValue / 100, 0);
        private float MistimedBlockModifierWithMod => Mathf.Max(1 - 2 * ARMValue / 100, 0);
        public float ARMValue => BaseARMValue * mod * (1 + Level / 100f);

        public enum BlockState { None, Blocking, PerfectlyBlocking, BadParry}

        public float GetDamage(int damage, BlockState state) {
            float fdamage = damage;
            fdamage = Mathf.Max(0, fdamage);

            if(state == BlockState.PerfectlyBlocking) {
                return fdamage * PerfectBlockModifierWithMod;
            }

            if(state == BlockState.Blocking) {
                return fdamage * MistimedBlockModifierWithMod;
            }

            if(state == BlockState.BadParry) {
                return fdamage;
            }

            return fdamage * NoBlockModifierWithMod;
        }

        public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
            return true;
        }

        public override void OnAfterObtained() {

            random = UnityEngine.Random.Range(0, 101);
            SetValuesBasedOnRandom();
        }

        public override string GetName() {
            return $"{nameMod} {base.GetName()}";
        }

        public override string GetDescription() {
            return $"ARM Value: {ARMValue}\n" +
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
            public int Level = 1;
        }

        public string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() { ID = ID, Random = random, Trash = IsMarkedTrash, Level = Level});
        }

        public void SetData(string saveString, params object[] otherData) {

            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            ID = ds.ID;
            random = ds.Random;
            IsMarkedTrash = ds.Trash;
            Level = ds.Level;

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
            BaseARMValue = original.BaseARMValue;
            ElementalWeakness = original.ElementalWeakness;
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


