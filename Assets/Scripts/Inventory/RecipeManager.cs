using System;
using CustomEvents;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using G4AW2.Utils;

namespace G4AW2.Managers
{
    [CreateAssetMenu(menuName = "Managers/Recipes")]
    public class RecipeManager : ScriptableObject
    {
        [SerializeField] private QuestManager _quests;
        [SerializeField] private ItemManager _items;

        public Action<RecipeConfig> RecipeUnlocked;
        private List<RecipeConfig> currentRecipes = null;

        public List<RecipeConfig> AllRecipes;


        public List<string> MadeRecipes => GlobalSaveData.SaveData.CraftingRecipesMade;

        [ContextMenu("Add all recipes in project")]
        private void SearchForAllItems()
        {
            EditorUtils.AddAllOfType(AllRecipes);
        }

        public List<RecipeConfig> GetPossibleRecipes()
        {
            return GetPossibleRecipesWhereResultIs<ItemConfig>();
        }

        public List<RecipeConfig> GetPossibleRecipesWhereResultIs<T>() where T : ItemConfig
        {

            List<RecipeConfig> recipes = new List<RecipeConfig>();

            foreach (var recipe in AllRecipes)
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

        public List<ItemInstance> Make(RecipeConfig cr)
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
                    if (!_items.Remove(item))
                    {
                        Debug.LogError("Tried to remove item from inventory but was unable to. id: " + item.name);
                        return null;
                    }
                    amount--;
                }
            }

            List<ItemInstance> items = new List<ItemInstance>();
            for (int i = 0; i < cr.Result.Amount; i++)
            {
                ItemInstance it = _items.CreateInstance(cr.Result.Item, _quests._currentQuest.Config.Level);
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
                List<RecipeConfig> recipes = GetPossibleRecipes();
                foreach (var recipe in recipes)
                {
                    if (!currentRecipes.Contains(recipe) && !MadeRecipes.Contains(recipe.name))
                    {
                        RecipeUnlocked?.Invoke(recipe);
                    }
                }
                currentRecipes = recipes;
            }
        }
    }

}
