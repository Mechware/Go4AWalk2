
using G4AW2.Questing;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "Persistent Set/Specific/PassiveQuest")]
		public class PersistentSetPassiveQuest : PersistentSetGeneric<PassiveQuest, UnityEventPassiveQuest> {
		}
	}
