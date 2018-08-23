using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Combat.Swiping {
	public class SwipeSetTester : MonoBehaviour {
		public SwipeSet SwipeSet;
		public int Level;

		public SwipeDrawer SwipePrefab;

		public SwipeDrawer child;

		[ContextMenu("Generate Swipe")]
		public void GenerateSwipe() {
			if (child == null) {
				child = Instantiate(SwipePrefab, transform);
			}
			Swipe s = SwipeSet.GetSwipe(Level);
			child.Swipe = s;
			child.AnimateSwipe();
		}
	}

}

