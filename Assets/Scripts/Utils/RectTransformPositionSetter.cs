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
        Vector3 vec = Transform.anchoredPosition;
        vec.y = y;
        Transform.anchoredPosition = vec;
    }

}
