using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timer {

	public static void StartTimer(MonoBehaviour actor, float time, Action OnFinish)
    {
        actor.StartCoroutine(StartTimer(time, OnFinish));
    }

    private static IEnumerator StartTimer(float time, Action OnFinish)
    {
        yield return new WaitForSeconds(time);
        OnFinish();
    }
}
