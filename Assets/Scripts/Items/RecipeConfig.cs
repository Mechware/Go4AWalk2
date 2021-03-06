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
    public class RecipeConfig : ScriptableObject {
        public List<RecipeComponent> Components;
        public RecipeComponent Result;
    }

    [Serializable]
    public class RecipeComponent {
        public ItemConfig Item;
        public int Amount;
    }
}
