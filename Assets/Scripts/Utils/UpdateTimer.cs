using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTimer {

    private float total;
    private float elapsed;
    
    private Action<float> onUpdate;
    private Action onFinish;

    /// <summary>
    /// Start the timer
    /// </summary>
    /// <param name="duration">How long the timer should run</param>
    /// <param name="onFinish">The function to call on finish</param>
    /// <param name="onUpdate">The function called each updated that reports a percentage done</param>
    public void Start(float duration, Action onFinish, Action<float> onUpdate) {
        total = duration;
        elapsed = 0;
        this.onUpdate = onUpdate;
        this.onFinish = onFinish;
    }

	public void Update (float dt) {
	    elapsed += dt;
	    if (elapsed >= total) {
            onFinish?.Invoke();
            return;
	    }

        onUpdate?.Invoke(elapsed / total);
	}
}
