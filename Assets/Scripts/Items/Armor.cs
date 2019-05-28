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
        public int Level = 1;

        [NonSerialized]
        public int Random = -1;
        private float mod = 1;
        private string nameMod;

        
        private float NoBlockModifierWithMod => Mathf.Max(1 - ARMValue / 100, 0);
        private float PerfectBlockModifierWithMod => Mathf.Max(0.25f - ARMValue / 400, 0); // at arm = 0, damage reduction is 75%
        private float MistimedBlockModifierWithMod => Mathf.Max(0.5f - ARMValue / 200, 0); // at arm = 0, damage reduction is 50%
        public float ARMValue => Mathf.RoundToInt(BaseARMValue * mod * (1 + Level / 100f));

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

            Random = UnityEngine.Random.Range(0, 101);
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
            if(Random == 0) {
                mod = 0.5f;
                nameMod = "Broken";
            } else if(Random >= 1 && Random <= 10) {
                mod = 0.7f;
                nameMod = "Damaged";
            } else if(Random >= 11 && Random <= 30) {
                mod = 0.85f;
                nameMod = "Inferior";
            } else if(Random >= 31 && Random <= 70) {
                mod = 1;
                nameMod = "Normal";
            } else if(Random >= 71 && Random <= 90) {
                mod = 1.15f;
                nameMod = "Fine";
            } else if(Random >= 91 && Random <= 99) {
                mod = 1.3f;
                nameMod = "Exquisite";
            } else if(Random == 100) {
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
            return JsonUtility.ToJson(new DummySave() { ID = ID, Random = Random, Trash = IsMarkedTrash, Level = Level});
        }

        public void SetData(string saveString, params object[] otherData) {

            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            ID = ds.ID;
            Random = ds.Random;
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


