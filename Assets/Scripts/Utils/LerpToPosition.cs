using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class LerpToPosition : MonoBehaviour {

    public AnimationCurve XCurve;
    public AnimationCurve YCurve;

	public float TimeToLerp;
	public UnityEvent OnLerpingDone;

    private Vector2 steps;
	private float duration;
    private bool lerping = false;
    private RectTransform RectTransform;

    private void Awake() {
        RectTransform = GetComponent<RectTransform>();
    }

    // Use this for initialization
    public void StartLerping () {

        Keyframe frame = XCurve.keys[0];
        frame.value = RectTransform.anchoredPosition.x;
        XCurve.MoveKey(0, frame);

        frame = YCurve.keys[0];
        frame.value = RectTransform.anchoredPosition.y;
        YCurve.MoveKey(0, frame);

        lerping = true;
        duration = 0;
	}

	public void StopLerping() {
        Vector2 pos;
        pos.x = XCurve.keys[1].value;
        pos.y = YCurve.keys[1].value;
        RectTransform.anchoredPosition = pos;

        lerping = false;
        OnLerpingDone.Invoke();
    }

	// Update is called once per frame
	void Update () {
		if(lerping) {
            duration += Time.deltaTime;

            if(duration > TimeToLerp) {
                StopLerping();
                return;
            }

            Vector2 pos = new Vector2();
            pos.x = XCurve.Evaluate(duration);
            pos.y = YCurve.Evaluate(duration);
            RectTransform.anchoredPosition = pos;
        }
    }
}
