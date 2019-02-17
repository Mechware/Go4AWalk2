
using G4AW2.Data.Crafting;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "Persistent Set/Specific/CraftingRecipe")]
		public class PersistentSetCraftingRecipe : PersistentSetGeneric<CraftingRecipe, UnityEventCraftingRecipe> {
		}
	}
