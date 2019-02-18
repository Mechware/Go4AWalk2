using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    [CreateAssetMenu(menuName = "Data/Items/Weapon")]
    public class Weapon : Item
    {
        public int Damage;
        private int Random;
        private float Mod;
        private string NameMod;

        public void SetValuesBasedOnRandom() {
            if(Random == 0) {
                Mod = 0.5f;
                NameMod = "Broken";
            } else if(Random >= 1 && Random <= 10) {
                Mod = 0.7f;
                NameMod = "Damaged";
            } else if(Random >= 11 && Random <= 30) {
                Mod = 0.85f;
                NameMod = "Inferior";
            } else if(Random >= 31 && Random <= 70) {
                Mod = 1;
                NameMod = "Normal";
            } else if(Random >= 71 && Random <= 90) {
                Mod = 1.15f;
                NameMod = "Fine";
            } else if(Random >= 91 && Random <= 99) {
                Mod = 1.3f;
                NameMod = "Exquisite";
            } else if(Random == 100) {
                Mod = 1.5f;
                NameMod = "Masterwork";
            }
        }

        public override void Create(string additionalInfo) {
            if (int.TryParse(additionalInfo, out Random)) {
                SetValuesBasedOnRandom();
            }
            else {
                Random = UnityEngine.Random.Range(0, 101);
                SetValuesBasedOnRandom();
            }
        }

        public override string GetAdditionalInfo() {
            return Random.ToString();
        }
    }
}


