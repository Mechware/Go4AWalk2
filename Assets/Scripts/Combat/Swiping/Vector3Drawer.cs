using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Combat.Swiping {

	[RequireComponent(typeof(LineRenderer))]
	public class Vector3Drawer : MonoBehaviour {

		public float PixelsPerSecond;
		public Vector3ArrayVariable Swipe;
		private LineRenderer lr;

		void Awake() {
			lr = GetComponent<LineRenderer>();
		}

		public IEnumerator DoSwipe( Vector3[] array, float speed) {
			float maxTime = LineUtils.GetLengthOfLine(array) * (1f / speed);
			Vector3[] points = new Vector3[array.Length];
			lr.positionCount = array.Length;

			for (float time = 0; time < maxTime; time += Time.deltaTime) {
				LineUtils.GetPointAlongLine(array, speed, time, points);
				lr.SetPositions(points);
				yield return null;
			}
		}

		public void AnimateSwipe(Vector3[] array, float speed) {
			StopAllCoroutines();
			StartCoroutine(DoSwipe(array, speed));
		}

		public void ShowSwipe(Vector3[] array) {
			lr.positionCount = array.Length;
			lr.SetPositions(array);
		}

		[ContextMenu("Animate Line")]
		public void AnimateSwipe() {
			StopAllCoroutines();
			StartCoroutine(DoSwipe(Swipe, PixelsPerSecond));
		}

		[ContextMenu("Show Line")]
		public void ShowSwipe() {
			lr.positionCount = Swipe.Value.Length;
			lr.SetPositions(Swipe.Value);
		}
	}
}

