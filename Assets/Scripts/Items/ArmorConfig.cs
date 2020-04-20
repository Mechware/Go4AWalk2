using System;
using CustomEvents;
using System.Linq;
using UnityEngine;

namespace G4AW2.Data.DropSystem {
    [CreateAssetMenu(menuName = "Data/Items/Armor")]
    public class ArmorConfig : ItemConfig {

        [Range(0, 50)]
        public float ArmorAtLevel0;
        public float ArmorScaling;
        
        public ElementalWeakness ElementalWeakness;

        public float Weight;
    }
}


