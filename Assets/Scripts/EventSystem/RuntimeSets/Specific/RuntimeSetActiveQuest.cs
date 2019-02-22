
using G4AW2.Questing;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "Runtime Set/Specific/ActiveQuest")]
		public class RuntimeSetActiveQuest : RuntimeSetGeneric<ActiveQuest, UnityEventActiveQuest> {
		}
	}
