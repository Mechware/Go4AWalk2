using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;
using Material = G4AW2.Data.DropSystem.Material;

namespace G4AW2.Data.Crafting
{
    [CreateAssetMenu(menuName = "Data/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public List<InventoryEntry> Components;
        public InventoryEntry Result;
    }
}
