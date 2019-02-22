
using G4AW2.Questing;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<ActiveQuest, ActiveQuestVariable, UnityEventActiveQuest>))]
		public class ActiveQuestPropertyDrawer : AbstractReferenceDrawer { }
}
