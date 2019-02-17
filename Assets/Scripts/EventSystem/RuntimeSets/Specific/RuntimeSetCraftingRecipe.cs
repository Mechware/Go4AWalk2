
using G4AW2.Data.Crafting;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "Runtime Set/Specific/CraftingRecipe")]
		public class RuntimeSetCraftingRecipe : RuntimeSetGeneric<CraftingRecipe, UnityEventCraftingRecipe> {
		}
	}
