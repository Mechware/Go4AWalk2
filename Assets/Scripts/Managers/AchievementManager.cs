using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;

namespace G4AW2.Managers
{
	public class AchievementManager : MonoBehaviour
	{

		public Action<Achievement> OnAchievementObtained;

		/*
		public static AchievementManager Instance;

		void Awake() {
			Instance = this;
		}

		public List<Achievement> Achievements;
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
		*/
	}

}
