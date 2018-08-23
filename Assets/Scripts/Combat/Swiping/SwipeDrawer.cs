using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;

namespace G4AW2.Combat.Swiping {
	public class SwipeDrawer : MonoBehaviour {

		public Vector3Drawer drawer;
		public Swipe Swipe;

		[ContextMenu("Animate Swipe")]
		public void AnimateSwipe() {
			drawer.AnimateSwipe(Swipe.Points, Swipe.PixelsPerSecond);
		}

		[ContextMenu("Show Swipe")]
		public void ShowSwipe() {
			drawer.ShowSwipe(Swipe.Points);
		}
	}

}

