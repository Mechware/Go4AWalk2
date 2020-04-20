using System;
using CustomEvents;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CraftingTable {

    private static List<CraftingRecipe> currentRecipes = null;
    
    public static List<CraftingRecipe> GetPossibleRecipes() {
        return GetPossibleRecipesWhereResultIs<ItemConfig>();
    }

    public static List<CraftingRecipe> GetPossibleRecipesWhereResultIs<T>() where T : ItemConfig {

        List<CraftingRecipe> recipes = new List<CraftingRecipe>();

        foreach(var recipe in Configs.Instance.Recipes) {
            if(!(recipe.Result.Item is T))
                continue;

            bool canMake = true;

            foreach(var component in recipe.Components) {
                var amt = Inventory.Instance.GetAmountOf(component.Item);
                if(amt < component.Amount) {
                    canMake = false;
                    break;
                }
            }
            if(canMake)
                recipes.Add(recipe);
        }

        return recipes;
    }

    public static List<ItemInstance> Make(CraftingRecipe cr) {
        if (cr.Components.Any(comp => !Inventory.Instance.Contains(comp.Item))) {
            Debug.LogError("Tried to craft something you could not make");
            return null;
        }

        foreach(var comp in cr.Components) {
            int amount = comp.Amount;
            var item = comp.Item;
            while (amount > 0) {
                if (!Inventory.Instance.Remove(item.Id)) {
                    Debug.LogError("Tried to remove item from inventory but was unable to. id: " + item.Id);
                    return null;
                }
                amount--;
            }
        }

        List<ItemInstance> items = new List<ItemInstance>();
        for (int i = 0; i < cr.Result.Amount; i++) {
            ItemInstance it = ItemFactory.GetInstance(cr.Result.Item, QuestManager.Instance.CurrentQuest.Config.Level);
            Inventory.Instance.Add(it);
            items.Add(it);
        }
        
        return items;
    }
    
    public static void CheckNewRecipes() {
        // Check if a new recipe is makeable
        if (currentRecipes == null) {
            currentRecipes = GetPossibleRecipes();
        } else {
            List<CraftingRecipe> recipes = GetPossibleRecipes();
            foreach(var recipe in recipes) {
                if(!currentRecipes.Contains(recipe) && !SaveGame.SaveData.CraftingRecipesMade.Contains(recipe.Id)) {
                    string postText = "";
                    foreach(var component in recipe.Components) {
                        postText +=
                            $"{component.Amount} {component.Item.Name}{(component.Amount > 1 ? "s" : "")}\n";
                    }
                    QuickPopUp.Show(Configs.Instance.QuestionMark, $"<size=150%>New Craftable Recipe!</size>\nA new recipe is now craftable!\nRequires:{postText}");
                }
            }
            currentRecipes = recipes;
        }
    }
}
