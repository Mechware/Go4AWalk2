
using G4AW2.Questing;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Events/Specific/ActiveQuest")]
	public class GameEventActiveQuest : GameEventGeneric<ActiveQuest, GameEventActiveQuest, UnityEventActiveQuest> {
	}
}
