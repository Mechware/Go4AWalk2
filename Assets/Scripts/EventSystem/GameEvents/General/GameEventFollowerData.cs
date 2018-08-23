
using G4AW2.Data;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Events/General/FollowerData")]
	public class GameEventFollowerData : GameEventGeneric<FollowerData, GameEventFollowerData, UnityEventFollowerData> {
	}
}
