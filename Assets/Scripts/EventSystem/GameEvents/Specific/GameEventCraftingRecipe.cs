
using G4AW2.Data.Crafting;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Events/Specific/CraftingRecipe")]
	public class GameEventCraftingRecipe : GameEventGeneric<CraftingRecipe, GameEventCraftingRecipe, UnityEventCraftingRecipe> {
	}
}
