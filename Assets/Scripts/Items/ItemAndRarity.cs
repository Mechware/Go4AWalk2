using Sirenix.OdinInspector;
using System;


namespace G4AW2.Data.DropSystem
{
    [Serializable]
    public class ItemAndRarity {
        public ItemConfig ItemConfig;
        [PropertyRange(0, 1)]
        public float dropChance;
    }
}
