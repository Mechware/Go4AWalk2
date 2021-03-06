using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    [CreateAssetMenu(menuName = "Data/Items/Armor")]
    public class ArmorConfig : ItemConfig {

        [Range(0, 50)]
        public float ArmorAtLevel0;
        public ElementalWeakness ElementalWeakness;
    }
}


