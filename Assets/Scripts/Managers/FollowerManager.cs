using System;
using G4AW2.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace G4AW2.Managers {
	public class FollowerManager : MonoBehaviour {

		public static FollowerManager Instance;
		
        [NonSerialized] public List<FollowerInstance> Followers = new List<FollowerInstance>();

        public Action<FollowerInstance> FollowerAdded;
        
		private float currentTimeToReach;
		private float currentTime;

		public int MAX_QUEUE_SIZE = 5;

		private QuestConfig _currentQuest;

		void Awake() {
			Instance = this;
            Random.InitState(Mathf.RoundToInt(Time.deltaTime));
		}

	    public void Initialize(GameEvents events, QuestManager quests) {

			events.OnQuestSet += q => SetQuest(q.Config);
			SetQuest(quests.CurrentQuest.Config);
			events.AreaChanged += a => ClearFollowers();

	        DateTime lastTimePlayedUTC = SaveGame.SaveData.LastTimePlayedUTC;
	        TimeSpan TimeSinceLastPlayed = DateTime.UtcNow - lastTimePlayedUTC;
	        double secondsSinceLastPlayed = TimeSinceLastPlayed.TotalSeconds;

	        Debug.Log("Time since last play: " + secondsSinceLastPlayed);

            foreach(var sd in SaveGame.SaveData.CurrentFollowers) {
                Followers.Add(FollowerFactory.GetInstance(sd));
            }

            // If you have no followers and you were idling away, add a new follower (???)
            if (secondsSinceLastPlayed > 5 * 60 && !Followers.Any()) {
	            AddRandomFollower();
            }
	    }

	    public void SetQuest(QuestConfig questConfig) {
			_currentQuest = questConfig;
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
				currentTimeToReach = Random.Range(_currentQuest.MinEnemyDropTime, _currentQuest.MaxEnemyDropTime);
                AddRandomFollower();
				CheckSpawns();
			}
		}

	    [ContextMenu("Add Follower")]
		public void AddRandomFollower() {
	        FollowerInstance follower = _currentQuest.Enemies.GetRandomFollower();
			AddFollower(follower);
        }

		public void AddFollower(FollowerConfig config) {
			var follower = _currentQuest.Enemies.GetFollower(config);
			AddFollower(follower);
		}

		private void AddFollower(FollowerInstance follower)
        {
			if (Followers.Count >= MAX_QUEUE_SIZE || follower == null) return;

			Followers.Add(follower);
			SaveGame.SaveData.CurrentFollowers.Add(follower.SaveData);
			FollowerAdded?.Invoke(follower);
		}

#if UNITY_EDITOR
	    [ContextMenu("Clear Followers")]
	    void ClearFollowers() {
		    Followers.Clear();
	    }
#endif
	}
}


