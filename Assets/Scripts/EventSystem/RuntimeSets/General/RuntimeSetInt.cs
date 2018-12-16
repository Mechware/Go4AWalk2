

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "Runtime Set/General/Int")]
		public class RuntimeSetInt : RuntimeSetGeneric<int, UnityEventInt> {
		[System.Serializable]
		private struct SaveObject {
			public List<int> List;
		}

		public override string GetSaveString() {
			return JsonUtility.ToJson(new SaveObject { List = Value });
		}

		public override void SetData( string saveString, params object[] otherData ) {
			Clear();

			Value.AddRange(JsonUtility.FromJson<SaveObject>(saveString).List);
		}
	}
	}
