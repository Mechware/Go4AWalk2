
using G4AW2.Data;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<FollowerConfig, FollowerDataVariable, UnityEventFollowerData>))]
		public class FollowerDataPropertyDrawer : AbstractReferenceDrawer { }
}
