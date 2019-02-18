using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.Crafting;
using UnityEngine;
using UnityEngine.Events;

public class RecipeFilter : MonoBehaviour {

    public CraftingTable CT;
    public GameObject ItemPrefab;

    public List<GameObject> Children;

    public UnityEventCraftingRecipe OnMake;

    public void AddRecipeAsChilden() {
        foreach (GameObject child in Children) {
            Destroy(child);
        }

        Children.Clear();

        foreach (var r in CT.GetPossibleRecipes()) {
            var go = GameObject.Instantiate(ItemPrefab, transform);
            Children.Add(go);
            var holder = go.GetComponent<ItemHolderWithText>();
            SetItem(holder, r);
        }
    }

    private void SetItem(ItemHolderWithText holder, CraftingRecipe recipe) {
        holder.SetItem(recipe.Result.Item, it => {
            CT.Make(recipe);
            OnMake?.Invoke(recipe);
        });
    }
}
