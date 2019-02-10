
using G4AW2.Data.DropSystem;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Events/Specific/Armor")]
	public class GameEventArmor : GameEventGeneric<Armor, GameEventArmor, UnityEventArmor> {
	}
}
