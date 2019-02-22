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

	[CreateAssetMenu(menuName = "Data/Quests/ActiveQuest")]
	public class ActiveQuest : Quest {

		public ActiveQuest NextQuest;
		public Area Area;

		public Conversation StartConversation;
		public Conversation EndConversation;

		public float TotalDistanceToWalk;


#if UNITY_EDITOR
	    [ContextMenu("Pick ID")]
	    public void PickID() {
	        ID = IDUtils.PickID<Quest>();
	    }
#endif

    }
}


