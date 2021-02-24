using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Data.DropSystem {
    [System.Serializable]
    public class ItemDropper {
        public List<ItemAndRarity> Items;

        public List<ItemInstance> GetItems(bool addGlobalItems, int level) {
            List<ItemInstance> droppedItems = new List<ItemInstance>();

            foreach(ItemAndRarity item in Items) {
                float value = Random.value;
                if(item.dropChance > value) {
                    droppedItems.Add(ItemFactory.GetInstance(item.ItemConfig, level));
                }
            }

            if(addGlobalItems) droppedItems.AddRange(ItemDropManager.GetRandomDrop());

            return droppedItems;
        }
    }
}
