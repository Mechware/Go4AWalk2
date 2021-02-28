using G4AW2;
using G4AW2.Data.Crafting;
using G4AW2.Managers;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour {

    [SerializeField] private ItemManager _items;

    public RecipeManager CT;
    public GameObject ItemPrefab;
    public Transform ParentOfItems;

    public List<GameObject> Children;

    public Sprite QuestionMark;

    public void RefreshList() {
        foreach (GameObject child in Children) {
            Destroy(child);
        }

        Children.Clear();

        foreach (var r in Configs.Instance.Recipes) {
            var go = Instantiate(ItemPrefab, ParentOfItems.transform);
            Children.Add(go);
            
            var holder = go.GetComponent<IconWithTextController>();
            SetItem(holder, r);
            
            if (!_items.IsCraftable(r)) {
                AlphaOfAllChildren.SetAlphaOfAllChildren(go, 1, Color.gray);
            }
        }
    }

    private void SetItem(IconWithTextController holder, CraftingRecipe recipe) {

        if (SaveGame.SaveData.CraftingRecipesMade.Contains(recipe.Id)) {
            string text = $"{recipe.Result.Item.Name}\n<size=50%>";
            foreach (var r in recipe.Components) {
                text += $"{r.Item.Name} - {_items.GetAmountOf(r.Item)} / {r.Amount}\n";
            }

            holder.SetDataConfig(recipe.Result.Item, 1, text, () => {
                MakeRecipe(recipe);
            }, showText:false);
        }
        else {
            string text = $"???\n<size=50%>";
            foreach(var r in recipe.Components) {
                text += $"{r.Item.Name} - {_items.GetAmountOf(r.Item)} / {r.Amount}\n";
            }

            holder.SetDataConfig(recipe.Result.Item, 1, text, () => {
                if (MakeRecipe(recipe)) {
                    SaveGame.SaveData.CraftingRecipesMade.Add(recipe.Id);
                    RefreshList();
                }
            }, QuestionMark, false);
        }
        
    }

    //public QuickPopUp PopUp;

    private bool MakeRecipe(CraftingRecipe cr) {
        var its = CT.Make(cr);
        if (its == null) return false;

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

        EquipItemProcessor.Instance.ProcessItem(its, null);
        RefreshList();

        return true;
    }
}
