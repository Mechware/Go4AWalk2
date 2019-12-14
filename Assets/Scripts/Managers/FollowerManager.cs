using System;
using CustomEvents;
using G4AW2.Data;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Combat;
using G4AW2.Questing;
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

	    public void Initialize() {
	        DateTime lastTimePlayedUTC = SaveGame.SaveData.LastTimePlayedUTC;
	        TimeSpan TimeSinceLastPlayed = DateTime.UtcNow - lastTimePlayedUTC;
	        double secondsSinceLastPlayed = TimeSinceLastPlayed.TotalSeconds;

	        Debug.Log("Time since last play: " + secondsSinceLastPlayed);

	        // If you have no followers and you were idling away, add a new follower (???)
            if (secondsSinceLastPlayed > 5 * 60 && !Followers.Any()) {
	            AddFollower();
            }
	    }

	    public void SetQuest(QuestConfig questConfig) {
	        currentTime = 0;
	        currentTimeToReach = Random.Range(questConfig.MinEnemyDropTime, questConfig.MaxEnemyDropTime);
        }

		void Update() {
			currentTime += Time.deltaTime;
			CheckSpawns();
		}

		private void CheckSpawns() {
			if (currentTime > currentTimeToReach) {
				currentTime -= currentTimeToReach;
				currentTimeToReach = Random.Range(QuestManager.Instance.CurrentQuest.Config.MinEnemyDropTime, QuestManager.Instance.CurrentQuest.Config.MaxEnemyDropTime);
                AddFollower();
				CheckSpawns();
			}
		}

	    [ContextMenu("Add Follower")]
		public void AddFollower() {
            // randomly choose a follower!
	        if (Followers.Count >= MAX_QUEUE_SIZE) return;

	        FollowerInstance instance = QuestManager.Instance.CurrentQuest.Config.Enemies.GetRandomFollower(true);
	        if (instance == null) return;

	        Followers.Add(instance);
	        SaveGame.SaveData.CurrentFollowers.Add(instance.SaveData);
        }

		public void Drop(FollowerConfig config) {
			if (Followers.Count >= MAX_QUEUE_SIZE) return;

			var follower = QuestManager.Instance.CurrentQuest.Config.Enemies.GetFollower(config, true);
			if (follower == null) return;

			Followers.Add(follower);
			SaveGame.SaveData.CurrentFollowers.Add(follower.SaveData);
		}

#if UNITY_EDITOR
	    [ContextMenu("Clear Followers")]
	    void ClearFollowers() {
		    Followers.Clear();
	    }
#endif
	}
}


