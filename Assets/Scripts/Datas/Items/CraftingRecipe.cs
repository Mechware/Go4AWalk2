using System.Collections;
using System.Collections.Generic;
using DropSystem;
using UnityEngine;

namespace Crafting
{
    [CreateAssetMenu(fileName = "Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public List<Item> components;
        public Item result;
    }
}
