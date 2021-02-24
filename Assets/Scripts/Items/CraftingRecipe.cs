using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.DropSystem;
using G4AW2.Managers;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Material = G4AW2.Data.DropSystem.Material;

namespace G4AW2.Data.Crafting
{
    [CreateAssetMenu(menuName = "Data/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject {
        public int Id = 0;
        public List<RecipeComponent> Components;
        public RecipeComponent Result;

        public bool IsCraftable() {
            return Components.All(component => ItemManager.Instance.GetAmountOf(component.Item) >= component.Amount);
        }

#if UNITY_EDITOR
        [ContextMenu("Pick ID")]
        public int PickID() {
            string[] paths = AssetDatabase.FindAssets("t:CraftingRecipe");
            for (int i = 0; i < paths.Length; i++) {
                paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
            }

            List<int> ids = paths.Select(AssetDatabase.LoadAssetAtPath<CraftingRecipe>).Select(q => q.Id).ToList();

            for (int i = 1; i < paths.Length + 1; i++) {
                if (!ids.Contains(i)) {
                    return i;
                }
            }
            Debug.LogError("Could not find a suitable id. List of ids: " + ids.StringSeparate());
            return -1;
        }
#endif
    }

    [Serializable]
    public class RecipeComponent {
        public ItemConfig Item;
        public int Amount;
    }
}
