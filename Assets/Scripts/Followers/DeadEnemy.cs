using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.Combat;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DeadEnemy : MonoBehaviour {

    public BoolReference Scrolling;
    public float ScrollSpeed = 5;
    public int EndingPosition = -527;

    public Image Image;

    public void SetPosition(float x, float y, EnemyData s, Action<DeadEnemy> onFinish) {

        StopAllCoroutines();

        Image.sprite = s.DeadSprite;

        Vector2 r = ((RectTransform) transform).sizeDelta;
        r.x = s.SizeOfSprite.x;
        r.y = s.SizeOfSprite.y;
        ((RectTransform) transform).sizeDelta = r;

        RectTransform rt = GetComponent<RectTransform>();
        Vector3 pos = rt.anchoredPosition;
        pos.x = x;
        pos.y = y;
        rt.anchoredPosition = pos;

        StartCoroutine(ScrollOff(onFinish));
    }

    IEnumerator ScrollOff(Action<DeadEnemy> onFinish) {

        RectTransform rt = GetComponent<RectTransform>();


        while (rt.anchoredPosition.x > EndingPosition) {
            if(Scrolling) {
                Vector3 pos = rt.anchoredPosition;
                pos.x -= ScrollSpeed * Time.deltaTime;
                rt.anchoredPosition = pos;
            }
            yield return null;
        }

        onFinish?.Invoke(this);
    }
}
