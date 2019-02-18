using CustomEvents;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CraftingTable")]
public class CraftingTable : ScriptableObject {

    public PersistentSetCraftingRecipe Recipes;
    public Inventory Inventory;

    public List<CraftingRecipe> GetPossibleRecipes() {
        return GetPossibleRecipesWhereResultIs<Item>();
    }

    public List<CraftingRecipe> GetPossibleRecipesWhereResultIs<T>() where T : Item {

        List<CraftingRecipe> recipes = new List<CraftingRecipe>();

        foreach(var recipe in Recipes) {
            if(!(recipe.Result.Item is T))
                continue;

            bool canMake = true;

            foreach(var component in recipe.Components) {
                if(Inventory.FirstOrDefault(e => e.Item == component.Item) == default(InventoryEntry)) {
                    canMake = false;
                    break;
                }
            }
            if(canMake)
                recipes.Add(recipe);
        }

        return recipes;
    }

    public bool Make(CraftingRecipe cr) {
        if (cr.Components.Any(comp => !Inventory.Contains(comp))) {
            return false;
        }

        foreach(var comp in cr.Components) {
            Inventory.Remove(comp);
        }

        ScriptableObject.Instantiate(cr.Result.Item);

        Inventory.Add(cr.Result);
        return true;
    }
}
