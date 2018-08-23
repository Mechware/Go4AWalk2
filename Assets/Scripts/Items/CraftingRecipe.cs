using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data.Crafting
{
    [CreateAssetMenu(menuName = "Data/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public List<Item> components;
        public Item result;
    }
}
