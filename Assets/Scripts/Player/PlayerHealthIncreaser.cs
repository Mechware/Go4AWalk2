using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;

public class PlayerHealthIncreaser : MonoBehaviour {

    public IntVariable PlayerHealth;
    public IntVariable PlayerMaxHealth;
    public FloatVariable DistanceWalked;

    private float distWalkedInternal;

    public void OnGameStateLoaded() {
        DistanceWalked.OnChange.AddListener(AfterDistWalkedUpdate);
        distWalkedInternal = DistanceWalked;
    }

    private void AfterDistWalkedUpdate(float amt) {
        float change = amt - distWalkedInternal;
        distWalkedInternal = amt;
        PlayerHealth.Value = Mathf.Min(Mathf.RoundToInt(PlayerHealth.Value + change), PlayerMaxHealth.Value);
    }

    public int HealthPerSecond = 1;
    private float updateTime = 1f;

	// Update is called once per frame
	void Update () {
	    if (Time.time > updateTime) {
	        updateTime += 1f;
	        PlayerHealth.Value = Mathf.Min(Mathf.RoundToInt(PlayerHealth.Value + HealthPerSecond), PlayerMaxHealth.Value);
	    }
	}
}
