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
        private float mod;
        private string nameMod;

        public void OnCreate() {
            int random;
            mod = 1;
            nameMod = "";

            random = Random.Range(0, 101);

            if (random == 0) {
                mod = 0.5f;
                nameMod = "Broken";
            }
            else if (random >= 1 && random <= 10) {
                mod = 0.7f;
                nameMod = "Damaged";
            }
            else if (random >= 11 && random <= 30) {
                mod = 0.85f;
                nameMod = "Inferior";
            }
            else if (random >= 31 && random <= 70) {
                mod = 1;
                nameMod = "Normal";
            }
            else if (random >= 71 && random <= 90) {
                mod = 1.15f;
                nameMod = "Fine";
            }
            else if (random >= 91 && random <= 99) {
                mod = 1.3f;
                nameMod = "Exquisite";
            }
            else if (random == 100) {
                mod = 1.5f;
                nameMod = "Masterwork";
            }

        }
    }
}


