using System;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Area;
using G4AW2.Dialogue;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace G4AW2.Questing {

	[CreateAssetMenu(menuName = "Data/Quest")]
	public class Quest : ScriptableObject {

		public int ID;

		public string DisplayName;
		public Quest NextQuest;
		public Area Area;

		public Conversation StartConversation;
		public Conversation EndConversation;

		public float TotalDistanceToWalk;

#if UNITY_EDITOR
		[ContextMenu("Pick ID")]
		public void PickID() {
			string[] paths = AssetDatabase.FindAssets("t:Quest");
			for (int i = 0; i < paths.Length; i++) {
				paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
			}

			List<int> ids = paths.Select(AssetDatabase.LoadAssetAtPath<Quest>).Select(q => q.ID).ToList();

			for (int i = 1; i < paths.Length + 1; i++) {
				if (!ids.Contains(i)) {
					ID = i;
					return;
				}
			}
			Debug.Log("How is that possible?");
		}

		void OnEnable() {
			if(ID == 0) PickID();
		}
#endif
	}
}


