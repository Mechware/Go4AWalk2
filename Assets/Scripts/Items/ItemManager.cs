using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Managers {
    [CreateAssetMenu(menuName = "Managers/Items")]
    public class ItemManager : ScriptableObject, IEnumerable<ItemInstance>
    {
        private List<ItemInstance> items = new List<ItemInstance>();

        public Action<ItemInstance> OnItemObtained;

        public bool IsCraftable(CraftingRecipe crafting)
        {
            return crafting.Components.All(component => GetAmountOf(component.Item) >= component.Amount);
        }

        public void Add(ItemInstance item)
        {
            SaveGame.SaveData.Inventory.Add(item.SaveData);
            items.Add(item);
            OnItemObtained?.Invoke(item);
        }

        public void Remove(ItemInstance item)
        {
            SaveGame.SaveData.Inventory.Remove(item.SaveData);
            items.Remove(item);
        }

        public bool Remove(int id)
        {
            var toRemove = items.FirstOrDefault(it => it.Config.Id == id);
            if (toRemove == null) return false;
            return items.Remove(toRemove);
        }

        public bool Contains(ItemConfig c)
        {
            return items.Any(i => i.Config == c);
        }

        public int GetAmountOf(ItemConfig it)
        {
            return items.Count(i => i.Config == it);
        }

        public IEnumerator<ItemInstance> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
