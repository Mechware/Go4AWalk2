using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;

public class AchievementManager : MonoBehaviour {

    public List<Achievement> Achievements;
	public Action<Achievement> OnAchievementObtained;
	public void InitAchievements () {
	    foreach (var ach in Achievements) {
	        if (ach.NumberToReach >= ach.Number) {
	            ach.AchievementInit();
                ach.OnComplete += AchievementComplete;
	        }
	    }
	}

    private void AchievementComplete(Achievement ach) {
        Debug.Log("Achievement Completed! " + ach);
		OnAchievementObtained?.Invoke(ach);
    }
}
