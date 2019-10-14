

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Events/Specific/SongData")]
	public class GameEventSongData : GameEventGeneric<SongData, GameEventSongData, UnityEventSongData> {
	}
}
