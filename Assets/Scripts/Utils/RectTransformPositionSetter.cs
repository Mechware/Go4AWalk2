using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTransformPositionSetter : MonoBehaviour {

    private RectTransform Transform;

    void Awake() {
        Transform = GetComponent<RectTransform>();
    }

    public void SetX(float x) {
        Vector3 vec = Transform.anchoredPosition;
        vec.x = x;
        Transform.anchoredPosition = vec;
    }

    public void SetY(float y) {
        if (Transform == null) return;

        Vector3 vec = Transform.anchoredPosition;
        vec.y = y;
        Transform.anchoredPosition = vec;
    }

    public void SetScaleX(float x) {
        Vector3 scale = Transform.localScale;
        scale.x = x;
        Transform.localScale = scale;
    }

    public void SetScaleY(float y) {
        Vector3 scale = Transform.localScale;
        scale.y = y;
        Transform.localScale = scale;
    }
}
