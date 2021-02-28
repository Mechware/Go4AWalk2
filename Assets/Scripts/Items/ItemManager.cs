﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Managers {
    [CreateAssetMenu(menuName = "Managers/Items")]
    public class ItemManager : ScriptableObject, IEnumerable<ItemInstance>
    {
        private List<ItemInstance> items = new List<ItemInstance>();

        public Action<ItemInstance> OnItemObtained;

        public List<ItemConfig> AllItems;
        [ContextMenu("Add all items in project")]
        private void SearchForAllItems()
        {
            EditorUtils.AddAllOfType(AllItems);
        }

        public bool IsCraftable(RecipeConfig crafting)
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

        public bool Remove(ItemConfig config)
        {
            var toRemove = items.FirstOrDefault(it => it.Config == config);
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

        public ItemInstance CreateInstance(ItemConfig config, int level, int random = -1)
        {
            if (config is WeaponConfig w)
            {
                var wi = new WeaponInstance(w, level);
                if (random != -1) wi.SaveData.Random = random;
                return wi;
            }

            if (config is ArmorConfig a)
            {
                var wi = new ArmorInstance(a, level);
                if (random != -1) wi.SaveData.Random = random;
                return wi;
            }

            if (config is HeadgearConfig hg)
            {
                var wi = new HeadgearInstance(hg, level);
                if (random != -1) wi.SaveData.Random = random;
                return wi;
            }

            if (config is EnchanterConfig e)
            {
                var wi = new EnchanterInstance(e);
                if (random != -1) wi.SaveData.Random = random;
                return wi;
            }

            return new ItemInstance(config);
        }

        public ItemInstance CreateInstance(ItemSaveData saveData)
        {
            var config = AllItems.First(item => item.Id == saveData.Id);

            if (saveData is WeaponSaveData w)
            {
                return new WeaponInstance(w, (WeaponConfig)config, AllItems.OfType<EnchanterConfig>());
            }

            if (saveData is ArmorSaveData a)
            {
                return new ArmorInstance(a, (ArmorConfig)config);
            }

            if (saveData is HeadgearSaveData hg)
            {
                return new HeadgearInstance(hg, (HeadgearConfig)config);
            }

            if (saveData is EnchanterSaveData e)
            {
                return new EnchanterInstance(e, (EnchanterConfig)config);
            }

            return new ItemInstance(saveData, config);
        }
    }

    

}

public enum Rarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    VeryRare = 3,
    Legendary = 4,
    Mythical = 5
}