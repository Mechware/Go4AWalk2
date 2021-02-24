using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;

[System.Obsolete("Idk what this is but it seems smelly. please remove it D:")]
public static class ItemDropManager {

    public static List<ItemAndRarity> Items = new List<ItemAndRarity>();

    public static void AddToList(ItemAndRarity Item) {
        Items.Add(Item);
    }

    public static void RemoveFromList(ItemAndRarity Item) {
        Items.Remove(Item);
    }

    public static List<ItemInstance> GetRandomDrop() {
        List<ItemInstance> droppedItems = new List<ItemInstance>();
        foreach(var item in Items) {
            float value = Random.value;
            if(item.dropChance > value) {
                droppedItems.Add(ItemFactory.GetInstance(item.ItemConfig, -1));
            }
        }
        return droppedItems;
    }
}
