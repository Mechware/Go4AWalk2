using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DropSystem
{
    [CreateAssetMenu(fileName = "Item Dropper")]
    public class ItemDropper : ScriptableObject
    {
        public List<ItemAndRarity> Items;

        public List<Item> GetItems()
        {
            List<Item> droppedItems = new List<Item>();

            foreach (var IAR in Items)
            {
                float value = Random.value;
                if (IAR.dropChance > value)
                {
                    droppedItems.Add(IAR.item);
                }
            }

            return droppedItems;
        }

        public void TestGetItems()
        {
            int numTests = 100000;
            Dictionary<Item, int> counts = new Dictionary<Item, int>();
            for (int i = 0; i < numTests; i++)
            {
                List<Item> items = GetItems();
                foreach (var item in items)
                {
                    int count = 0;
                    if (counts.TryGetValue(item, out count))
                    {
                        counts.Remove(item);
                    }
                    counts.Add(item, count + 1);
                }
            }

            foreach (var kvp in counts)
            {
                Debug.Log(string.Format("Key: {0}, Value: {1}", kvp.Key.description, kvp.Value / (float) numTests));
            }
        }
    }
}
