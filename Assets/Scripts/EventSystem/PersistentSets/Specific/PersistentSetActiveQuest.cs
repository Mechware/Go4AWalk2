
using G4AW2.Questing;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "Persistent Set/Specific/ActiveQuest")]
		public class PersistentSetActiveQuest : PersistentSetGeneric<ActiveQuest, UnityEventActiveQuest> {
		}
	}
