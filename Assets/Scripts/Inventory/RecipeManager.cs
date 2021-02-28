using System;
using CustomEvents;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace G4AW2.Managers
{
    [CreateAssetMenu(menuName = "Managers/Recipes")]
    public class RecipeManager : ScriptableObject
    {
        [SerializeField] private QuestManager _quests;
        [SerializeField] private ItemManager _items;

        public Action<CraftingRecipe> RecipeUnlocked;
        private List<CraftingRecipe> currentRecipes = null;

        public List<CraftingRecipe> GetPossibleRecipes()
        {
            return GetPossibleRecipesWhereResultIs<ItemConfig>();
        }

        public List<CraftingRecipe> GetPossibleRecipesWhereResultIs<T>() where T : ItemConfig
        {

            List<CraftingRecipe> recipes = new List<CraftingRecipe>();

            foreach (var recipe in Configs.Instance.Recipes)
            {
                if (!(recipe.Result.Item is T))
                    continue;

                bool canMake = true;

                foreach (var component in recipe.Components)
                {
                    var amt = _items.GetAmountOf(component.Item);
                    if (amt < component.Amount)
                    {
                        canMake = false;
                        break;
                    }
                }
                if (canMake)
                    recipes.Add(recipe);
            }

            return recipes;
        }

        public List<ItemInstance> Make(CraftingRecipe cr)
        {
            if (cr.Components.Any(comp => !_items.Contains(comp.Item)))
            {
                Debug.LogError("Tried to craft something you could not make");
                return null;
            }

            foreach (var comp in cr.Components)
            {
                int amount = comp.Amount;
                var item = comp.Item;
                while (amount > 0)
                {
                    if (!_items.Remove(item.Id))
                    {
                        Debug.LogError("Tried to remove item from inventory but was unable to. id: " + item.Id);
                        return null;
                    }
                    amount--;
                }
            }

            List<ItemInstance> items = new List<ItemInstance>();
            for (int i = 0; i < cr.Result.Amount; i++)
            {
                ItemInstance it = ItemFactory.GetInstance(cr.Result.Item, _quests.CurrentQuest.Config.Level);
                _items.Add(it);
                items.Add(it);
            }

            return items;
        }

        public void CheckNewRecipes()
        {
            // Check if a new recipe is makeable
            if (currentRecipes == null)
            {
                currentRecipes = GetPossibleRecipes();
            }
            else
            {
                List<CraftingRecipe> recipes = GetPossibleRecipes();
                foreach (var recipe in recipes)
                {
                    if (!currentRecipes.Contains(recipe) && !SaveGame.SaveData.CraftingRecipesMade.Contains(recipe.Id))
                    {
                        RecipeUnlocked?.Invoke(recipe);
                    }
                }
                currentRecipes = recipes;
            }
        }
    }

}
