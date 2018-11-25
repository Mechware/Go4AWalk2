using System;
using G4AW2.Data.Area;
using Sirenix.OdinInspector;
using UnityEngine;

namespace G4AW2.Questing {

	[CreateAssetMenu(menuName = "Data/Quest")]
	public class Quest : ScriptableObject {

		public string DisplayName;
		public Quest NextQuest;
		public Area Area;

		public float TotalDistanceToWalk;
	}
}


