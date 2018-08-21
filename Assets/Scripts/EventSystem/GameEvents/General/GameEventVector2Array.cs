
using UnityEngine;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Events/General/Vector2Array")]
	public class GameEventVector2Array : GameEventGeneric<Vector2[], GameEventVector2Array, UnityEventVector2Array> {
	}
}
