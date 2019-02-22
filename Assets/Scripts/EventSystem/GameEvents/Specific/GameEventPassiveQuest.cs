
using G4AW2.Questing;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Events/Specific/PassiveQuest")]
	public class GameEventPassiveQuest : GameEventGeneric<PassiveQuest, GameEventPassiveQuest, UnityEventPassiveQuest> {
	}
}
