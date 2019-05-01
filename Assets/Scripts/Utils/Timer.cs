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


    public static void StartTimer(MonoBehaviour actor, float time, Action OnFinish, Action<float> OnUpdate) {
        actor.StartCoroutine(StartTimer(time, OnFinish, OnUpdate));
    }

    private static IEnumerator StartTimer(float time, Action OnFinish, Action<float> OnUpdate) {
        float timeElapsed = 0;
        while (true) {
            timeElapsed += Time.deltaTime;
            if (time - timeElapsed <= 0) break;
            OnUpdate(timeElapsed);
            yield return null;
        }

        OnFinish();
    }
}
