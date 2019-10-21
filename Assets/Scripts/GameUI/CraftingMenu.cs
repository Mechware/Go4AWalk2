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

    public Sprite QuestionMark;

    public void RefreshList() {
        foreach (GameObject child in Children) {
            Destroy(child);
        }

        Children.Clear();

        foreach (var r in CT.Recipes) {
            var go = Instantiate(ItemPrefab, ParentOfItems.transform);
            Children.Add(go);
            
            var holder = go.GetComponent<IconWithTextController>();
            SetItem(holder, r);
            
            if (!r.IsCraftable(CT.Inventory)) {
                AlphaOfAllChildren.SetAlphaOfAllChildren(go, 1, Color.gray);
            }
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
                    RefreshList();
                }
            }, QuestionMark, false);
        }
        
    }

    //public QuickPopUp PopUp;

    private bool MakeRecipe(CraftingRecipe cr) {
        Item it = CT.Make(cr);
        if (it == null) return false;

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
