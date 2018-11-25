using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Combat.Swiping {
	[CreateAssetMenu(menuName = "Data/Combat/SwipeSet")]
	public class SwipeSet : ScriptableObject {

		[System.Serializable]
		public class LeveledSwipe {
			public int LevelUnlocked;
			public PersistentSetVector3 PointList;
			public AnimationCurve MinSpeedCurve;
			public AnimationCurve MaxSpeedCurve;
		}

		public List<LeveledSwipe> LeveldSwipes;

		public Swipe GetSwipe(int level) {
			Swipe s= new Swipe();
			LeveledSwipe ls = LeveldSwipes.Where(swipe => swipe.LevelUnlocked <= level).ToList().GetRandom();

			s.Points = ls.PointList.Value.ToArray();
			s.PixelsPerSecond = Random.Range(ls.MinSpeedCurve.Evaluate(level), ls.MaxSpeedCurve.Evaluate(level));

			return s;
		}
	}

}

