using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    [CreateAssetMenu(menuName = "Data/Items/Item")]
    public class ItemConfig : ScriptableObject {

        public string Name = "";
        public Sprite Image;
        public int Value;
        public string Description;
        public Rarity Rarity;
        public bool SellWithTrash;
    }
}

