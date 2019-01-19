using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LerpToPosition : MonoBehaviour {

	public Vector2 EndPosition;
	public float TimeToLerp;
	public UnityEvent OnLerpingDone;
	public bool lerping;

	private Vector2 startPosition;
	private Vector2 steps;
	private float startTime;


	// Use this for initialization
	public void StartLerping () {
		lerping = true;
		startPosition = ((RectTransform)transform).localPosition;
		Vector3 distance = EndPosition - startPosition;
		steps.x = distance.x / TimeToLerp;
		steps.y = distance.y / TimeToLerp;
		startTime = Time.time;
	}

	public void StopLerping() {
		lerping = false;
	}

	// Update is called once per frame
	void Update () {
		if(lerping) {
			Vector3 Pos = startPosition + (steps * (Time.time - startTime) / TimeToLerp);
			Pos.z = ((RectTransform)transform).localPosition.z;
			((RectTransform) transform).localPosition = Pos;
			if(Time.time - startTime >= TimeToLerp) {
				lerping = false;
                OnLerpingDone.Invoke();
			}
		}
	}
}
