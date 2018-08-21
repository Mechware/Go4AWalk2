
using UnityEngine;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Events/General/Vector3")]
	public class GameEventVector3 : GameEventGeneric<Vector3, GameEventVector3, UnityEventVector3> {
	}
}
