using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.Area;
using UnityEngine;

namespace G4AW2.Questing {

	[CreateAssetMenu(menuName = "Data/Questing/Area")]
	public class AreaQuest : Quest {

		public Area Area;

		public CustomEvents.UnityEventQuest QuestCompleted;

		public float TotalDistanceToWalk;
		public float DistanceWalked;

		public override void Start() {
			// Go to different area.


			// Listen to move event
		}

		public override void GPSUpdate(float distance) {
			DistanceWalked += distance;

			if (TotalDistanceToWalk == -1) return; // YOU CAN NEVER FINISH MWHAHHAAHA

			if(TotalDistanceToWalk >= DistanceWalked) QuestCompleted.Invoke(this);
		}
	}
}


