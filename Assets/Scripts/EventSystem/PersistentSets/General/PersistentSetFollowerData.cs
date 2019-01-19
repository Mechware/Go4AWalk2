
using System.Linq;
using G4AW2.Data;
using Sirenix.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "Persistent Set/General/FollowerData")]
		public class PersistentSetFollowerData : PersistentSetGeneric<FollowerData, UnityEventFollowerData> {
#if UNITY_EDITOR
		[ContextMenu("Add all followers")]
		public void AddAllOfType() {
			string[] paths = AssetDatabase.FindAssets("t:" + typeof(FollowerData).Name);
			for (int i = 0; i < paths.Length; i++) {
				paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
			}

			paths.Select(AssetDatabase.LoadAssetAtPath<FollowerData>).ForEach(Add);
		}
#endif
	}
}
