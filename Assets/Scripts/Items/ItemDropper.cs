using G4AW2.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Data.DropSystem {
    [System.Serializable]
    public class ItemDropper {
        public List<ItemAndRarity> Items;

        public List<ItemInstance> GetItems(int level, ItemManager items) {
            List<ItemInstance> droppedItems = new List<ItemInstance>();

            foreach(ItemAndRarity item in Items) {
                float value = Random.value;
                if(item.dropChance > value) {
                    droppedItems.Add(items.CreateInstance(item.ItemConfig, level));
                }
            }

            return droppedItems;
        }
    }
}
