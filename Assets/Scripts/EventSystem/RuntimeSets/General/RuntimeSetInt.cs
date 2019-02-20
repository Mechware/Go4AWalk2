

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "Runtime Set/General/Int")]
		public class RuntimeSetInt : RuntimeSetGeneric<int, UnityEventInt>, ISaveable {
		
	}
}
