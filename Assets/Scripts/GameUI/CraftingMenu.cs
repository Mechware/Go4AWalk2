using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using UnityEngine;
using UnityEngine.Events;

public class CraftingMenu : MonoBehaviour {

    public CraftingTable CT;
    public GameObject ItemPrefab;
    public Transform ParentOfItems;

    public List<GameObject> Children;

    public InventoryItemDisplay ItemDisplay;

    public ItemViewer ItemSelector;

    public Sprite QuestionMark;

    private Item itemFilter;

    private enum FilterType {
        Craftable = 0,
        HaveSomeMaterials = 1
    };

    private FilterType filter = FilterType.Craftable;

    void Start() {
        ItemDisplay.SetData(null, 0, OnItemDisplayClicked);
    }

    void OnItemDisplayClicked(InventoryItemDisplay iid) {
        ItemSelector.ShowAllMaterialFromInventory("Materials", false, OnItemFilterItemSelected, true);
    }

    void OnItemFilterItemSelected(Item it) {
        itemFilter = it;
        ItemDisplay.SetData(itemFilter, 0, OnItemDisplayClicked);
        RefreshList();
        ItemSelector.Close();
    }

    public void SortOptionChanged(int i) {
        filter = (FilterType)i;
        RefreshList();
    }

    public IEnumerable<CraftingRecipe> GetItemsToShow() {

        IEnumerable<CraftingRecipe> recipesToShow;

        if (itemFilter == null) {
            recipesToShow = CT.Recipes;
        }
        else {
            recipesToShow = CT.Recipes.Where(r => r.Components.Any(c => c.Item.ID == itemFilter.ID));
        }

        if(filter == FilterType.Craftable) {
            recipesToShow = recipesToShow.Where(r => r.IsCraftable(CT.Inventory));
        } else if (filter == FilterType.HaveSomeMaterials) {
            if (itemFilter == null) {
                recipesToShow =
                    recipesToShow.Where(
                        recipe => recipe.Components.Any(component => CT.Inventory.Contains(component.Item)));
            }
        }
        else {
            throw  new Exception("Invalid filter selection");
        }

        return recipesToShow;
    }

    public void RefreshList() {
        foreach (GameObject child in Children) {
            Destroy(child);
        }

        Children.Clear();

        foreach (var r in GetItemsToShow()) {
            var go = GameObject.Instantiate(ItemPrefab, ParentOfItems.transform);
            Children.Add(go);
            var holder = go.GetComponent<IconWithTextController>();
            SetItem(holder, r);
        }
    }

    private void SetItem(IconWithTextController holder, CraftingRecipe recipe) {

        if (CraftingRecipesMade.RecipesMade.Contains(recipe.ID)) {
            string text = $"{recipe.Result.Item.GetName()}\n<size=50%>";
            foreach (var r in recipe.Components) {
                text += $"{r.Item.GetName()} - {CT.Inventory.GetAmountOf(r.Item)} / {r.Amount}\n";
            }

            holder.SetData(recipe.Result.Item, 1, text, () => {
                MakeRecipe(recipe);
            }, showText:false);
        }
        else {
            string text = $"???\n<size=50%>";
            foreach(var r in recipe.Components) {
                text += $"{r.Item.GetName()} - {CT.Inventory.GetAmountOf(r.Item)} / {r.Amount}\n";
            }

            holder.SetData(recipe.Result.Item, 1, text, () => {
                if (MakeRecipe(recipe)) {
                    CraftingRecipesMade.RecipesMade.Add(recipe.ID);
                }
            }, QuestionMark, false);
        }
        
    }

    //public QuickPopUp PopUp;

    private bool MakeRecipe(CraftingRecipe cr) {
        Item it = CT.Make(cr);
        if (it == null) return false;
        RefreshList();

        /*
        string desc = "";
        if (it is Weapon) {
            desc = $"\nDAM: {((Weapon) it).GetDescription()}";
        } else if (it is Armor) {
            desc = $"\nARM: {((Armor) it).GetDescription()}";
        } else if (it is Headgear) {
            desc = $"\nHP: {((Headgear) it).GetDescription()}";
        }*/

        //PopUp.ShowSprite(cr.Result.Item.Image, $"<size=150%>Crafted!</size>\nYou successfully crafted a {it.GetName()}!{PostText}");

        EquipItemProcessor.Instance.ProcessItem(it, null);
        
        return true;
    }
}