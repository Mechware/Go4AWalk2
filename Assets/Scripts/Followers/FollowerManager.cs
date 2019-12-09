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
	public class FollowerManager : MonoBehaviour {

		public static FollowerManager Instance;
		
        public List<FollowerInstance> Followers;

        public Action<FollowerInstance> FollowerAdded;
        public Action<FollowerInstance> FollowerRemoved;
        
		private float currentTimeToReach;

		private float currentTime;

		public int MAX_QUEUE_SIZE = 5;
		
		void Awake() {
			Instance = this;
            Random.InitState(Mathf.RoundToInt(Time.deltaTime));
		}

	    public void LoadFinished() {
	        DateTime lastTimePlayedUTC = SaveManager.LastTimePlayedUTC;
	        TimeSpan TimeSinceLastPlayed = DateTime.UtcNow - lastTimePlayedUTC;
	        double secondsSinceLastPlayed = TimeSinceLastPlayed.TotalSeconds;

	        Debug.Log("Time since last play: " + secondsSinceLastPlayed);

	        // If you have no followers and you were idling away, add a new follower (???)
            if (secondsSinceLastPlayed > 5 * 60 && !Followers.Any()) {
	            AddFollower();
            }
	    }

	    public void QuestChanged(ActiveQuestBase quest) {
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
				currentTimeToReach = Random.Range(QuestManager.Instance.CurrentQuest.MinEnemyDropTime, QuestManager.Instance.CurrentQuest.MaxEnemyDropTime);
                AddFollower();
				CheckSpawns();
			}
		}

	    [ContextMenu("Add Follower")]
		public void AddFollower() {
            // randomly choose a follower!
	        if (Followers.Count >= MAX_QUEUE_SIZE) return;

	        FollowerInstance instance = QuestManager.Instance.CurrentQuest.Enemies.GetRandomFollower(true);
	        if (instance == null) return;

	        Followers.Add(instance);
        }

		public void Drop(FollowerConfig config) {
			if (Followers.Count >= MAX_QUEUE_SIZE) return;

			var follower = QuestManager.Instance.CurrentQuest.Enemies.GetFollower(config, true);
			if (follower == null) return;

			Followers.Add(follower);
		}

#if UNITY_EDITOR
	    [ContextMenu("Clear Followers")]
	    void ClearFollowers() {
		    Followers.Clear();
	    }
#endif
	}
}


