using System;
using G4AW2.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace G4AW2.Managers {
	[CreateAssetMenu(menuName ="Managers/Followers")]
	public class FollowerManager : ScriptableObject {

		[Obsolete("Singleton")] public static FollowerManager Instance;
		
        [NonSerialized] public List<FollowerInstance> Followers = new List<FollowerInstance>();
		[SerializeField] private QuestManager _quests;

        public Action<FollowerInstance> FollowerAdded;
        public Action<(FollowerInstance follower, bool suicide)> FollowerRemoved;

		private float currentTimeToReach;
		private float currentTime;

		public int MAX_QUEUE_SIZE = 5;

		private QuestConfig _lastQuest;

		void OnEnable() {
			Instance = this;
            Random.InitState(Mathf.RoundToInt(DateTime.Now.Ticks % int.MaxValue));
			_quests.QuestStarted += q => SetQuest(q.Config);
		}

	    public void Initialize() {
			_lastQuest = _quests.CurrentQuest.Config;
			
			DateTime lastTimePlayedUTC = SaveGame.SaveData.LastTimePlayedUTC;
	        TimeSpan TimeSinceLastPlayed = DateTime.UtcNow - lastTimePlayedUTC;
	        double secondsSinceLastPlayed = TimeSinceLastPlayed.TotalSeconds;

	        Debug.Log("Time since last play: " + secondsSinceLastPlayed);

            foreach(var sd in SaveGame.SaveData.CurrentFollowers) {
                Followers.Add(FollowerFactory.GetInstance(sd));
            }

            if (secondsSinceLastPlayed > 5 * 60 && !Followers.Any()) {
	            AddRandomFollower();
            }

	    }

	    public void SetQuest(QuestConfig questConfig) {
			if (_lastQuest.Area != questConfig.Area)
				ClearFollowers();

			_lastQuest = questConfig;
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
				currentTimeToReach = Random.Range(_quests.CurrentQuest.Config.MinEnemyDropTime, _quests.CurrentQuest.Config.MaxEnemyDropTime);
                AddRandomFollower();
				CheckSpawns();
			}
		}

	    [ContextMenu("Add Follower")]
		public void AddRandomFollower() {
	        FollowerInstance follower = _quests.CurrentQuest.Config.Enemies.GetRandomFollower();
			AddFollower(follower);
        }

		public bool AddFollower(FollowerConfig config, out FollowerInstance follower) {
			follower = _quests.CurrentQuest.Config.Enemies.GetFollower(config);
			return AddFollower(follower);
		}

		private bool AddFollower(FollowerInstance follower)
        {
			if (Followers.Count >= MAX_QUEUE_SIZE || follower == null) return false;

			Followers.Add(follower);
			SaveGame.SaveData.CurrentFollowers.Add(follower.SaveData);
			FollowerAdded?.Invoke(follower);
			return true;
		}

		public bool RemoveFollower(FollowerInstance follower)
        {
			bool successfullyRemoved = SaveGame.SaveData.CurrentFollowers.Remove(follower.SaveData);
			FollowerRemoved?.Invoke((follower, false)); 
			return successfullyRemoved;
		}

#if UNITY_EDITOR
	    [ContextMenu("Clear Followers")]
	    void ClearFollowers() {
		    Followers.Clear();
	    }
#endif
	}
}


