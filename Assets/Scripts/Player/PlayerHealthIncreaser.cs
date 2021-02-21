using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Saving;
using UnityEngine;

public class PlayerHealthIncreaser : MonoBehaviour {

    [Obsolete("Just get a reference to player")]
    public IntVariable PlayerHealth;
    public IntVariable PlayerMaxHealth;

    public void OnGameStateLoaded() {

        DateTime lastTimePlayedUTC = SaveManager.LastTimePlayedUTC;
        TimeSpan TimeSinceLastPlayed = DateTime.UtcNow - lastTimePlayedUTC;
        double secondsSinceLastPlayed = TimeSinceLastPlayed.TotalSeconds;
        IncreaseHealthByTime((int)secondsSinceLastPlayed);
    }

    public int HealthPerSecond = 1;
    private float updateTime = 1f;

	// Update is called once per frame
	void Update () {
	    if (Time.time > updateTime) {
	        updateTime += 1f;
	        IncreaseHealthByTime(1);
	    }
	}

    public void IncreaseHealthByTime(float time) {
	    PlayerHealth.Value = Mathf.Min(Mathf.RoundToInt(PlayerHealth.Value + time * HealthPerSecond), PlayerMaxHealth.Value);
    }

    private DateTime PauseTime = DateTime.MaxValue;

    private void OnApplicationFocus(bool focus) {
        if (focus) {
            // Played
            if (PauseTime == DateTime.MaxValue) {
                return;
            }

            TimeSpan diff = DateTime.Now - PauseTime;
            IncreaseHealthByTime((float)diff.TotalSeconds);
        }
        else {
            // Paused
            PauseTime = DateTime.Now;
        }
    }

    private void OnApplicationPause(bool pause) {
        OnApplicationFocus(!pause);
    }
}
