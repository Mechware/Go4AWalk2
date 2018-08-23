using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Combat.Swiping {
	[Serializable]
	public class Swipe {

		public Vector3[] Points;
		public float PixelsPerSecond;

		public void GetSwipeAmount(float time, Vector3[] points) {
			LineUtils.GetPointAlongLine(Points, PixelsPerSecond, time, points);
		}

		public float GetEntireSwipeTime() {
			return LineUtils.GetLengthOfLine(Points) * (1 / PixelsPerSecond);
		}

		
	}
}

