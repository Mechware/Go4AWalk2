using System.Collections;
using System.Collections.Generic;
using G4AW2.DropSystem;
using UnityEngine;

namespace G4AW2.Crafting
{
    [CreateAssetMenu(fileName = "Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public List<Item> components;
        public Item result;
    }
}
