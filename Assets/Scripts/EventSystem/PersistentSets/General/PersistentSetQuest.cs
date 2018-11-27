
using System.Linq;
using G4AW2.Data;
using G4AW2.Questing;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "Persistent Set/General/Quest")]
		public class PersistentSetQuest : PersistentSetGeneric<Quest, UnityEventQuest> {
#if UNITY_EDITOR
		[ContextMenu("Add all quests")]
		public void AddAllOfType() {
			string[] paths = AssetDatabase.FindAssets("t:" + typeof(Quest).Name);
			for (int i = 0; i < paths.Length; i++) {
				paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
			}

			paths.Select(AssetDatabase.LoadAssetAtPath<Quest>).ForEach(Add);
		}
#endif
	}
}
