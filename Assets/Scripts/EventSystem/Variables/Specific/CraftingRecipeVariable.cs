
using G4AW2.Data.Crafting;

namespace CustomEvents {
    [UnityEngine.CreateAssetMenu(menuName = "Variable/Specific/CraftingRecipe")]
	public class CraftingRecipeVariable : Variable<CraftingRecipe, UnityEventCraftingRecipe> {
	}
}
