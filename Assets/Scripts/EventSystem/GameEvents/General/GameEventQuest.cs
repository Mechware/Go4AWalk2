
using G4AW2.Questing;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Events/General/Quest")]
	public class GameEventQuest : GameEventGeneric<Quest, GameEventQuest, UnityEventQuest> {
	}
}
