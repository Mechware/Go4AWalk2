using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;

public class AchievementManager : MonoBehaviour {

    public List<Achievement> Achievements;

	// Use this for initialization
	void Start () {
	    foreach (var ach in Achievements) {
	        if (ach.NumberToReach >= ach.Number) {
	            ach.OnComplete += AchievementComplete;
	        }
	    }
	}

    private void AchievementComplete(Achievement ach) {
        Debug.Log("Achievement Completed! " + ach);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
