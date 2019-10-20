using System;
using CustomEvents;
using G4AW2.Data;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using G4AW2.Saving;
using UnityEngine;
using Random = UnityEngine.Random;

namespace G4AW2.Followers {
	public class FollowerSpawner : MonoBehaviour {

		public static FollowerSpawner Instance;
		
        public RuntimeSetFollowerData CurrentFollowers;
	    public ActiveQuestBaseVariable CurrentQuest;

		private float currentTimeToReach;
		private float currentDistanceToReach;

		private float currentTime;

		public int MAX_QUEUE_SIZE = 5;
		
		void Awake() {
			Instance = this;
            CurrentQuest.OnChange.AddListener(QuestChanged);
            Random.InitState(Mathf.RoundToInt(Time.deltaTime));
		}

	    public void LoadFinished() {
	        DateTime lastTimePlayedUTC = SaveManager.LastTimePlayedUTC;
	        TimeSpan TimeSinceLastPlayed = DateTime.UtcNow - lastTimePlayedUTC;
	        double secondsSinceLastPlayed = TimeSinceLastPlayed.TotalSeconds;
            Debug.Log("Time since last play: " + secondsSinceLastPlayed);

            if (secondsSinceLastPlayed > 5 * 60 && !CurrentFollowers.Any()) {
	            AddFollower();
            }
	    }

	    void QuestChanged(ActiveQuestBase quest) {
	        currentTime = 0;
	        currentTimeToReach = Random.Range(quest.MinEnemyDropTime, quest.MaxEnemyDropTime);
        }

		void Update() {
			currentTime += Time.deltaTime;
			CheckSpawns();
		}

		private void CheckSpawns() {
			if (currentTime > currentTimeToReach) {
				currentTime -= currentTimeToReach;
				currentTimeToReach = Random.Range(CurrentQuest.Value.MinEnemyDropTime, CurrentQuest.Value.MaxEnemyDropTime);
                AddFollower();
				CheckSpawns();
			}
		}

	    [ContextMenu("Add Follower")]
		public void AddFollower() {
            // randomly choose a follower!
	        if (CurrentFollowers.Value.Count >= MAX_QUEUE_SIZE) return;

	        FollowerData data = CurrentQuest.Value.Enemies.GetRandomFollower(true);
	        if (data == null) return;

            CurrentFollowers.Add(data);
        }

		public void Drop(FollowerData data) {
			if (CurrentFollowers.Value.Count >= MAX_QUEUE_SIZE) return;

			var follower = CurrentQuest.Value.Enemies.GetFollower(data, true);
			if (follower == null) return;

			CurrentFollowers.Add(follower);
		}

#if UNITY_EDITOR
	    [ContextMenu("Clear Followers")]
	    void ClearFollowers() {
		    CurrentFollowers.Value.Clear();
	    }

	    
#endif
	}
}


