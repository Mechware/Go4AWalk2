using System;
using G4AW2.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using G4AW2.Data.Combat;
using G4AW2.Followers;
using G4AW2.Data.DropSystem;
using G4AW2.Utils;
using UnityEditor;

namespace G4AW2.Managers {
	public class FollowerManager : MonoBehaviour {

        [NonSerialized] private List<FollowerInstance> _followers = new List<FollowerInstance>();
		[SerializeField] private QuestManager _quests;
		[SerializeField] private ItemManager _items;
		[SerializeField] private SaveManager _saveManager;

		public Action Loaded;

		public Action<FollowerInstance> FollowerAdded;
        public Action<FollowerInstance> FollowerRemoved;

		private float currentTimeToReach;
		private double currentTime;

		public int MAX_QUEUE_SIZE = 5;

		private QuestConfig _lastQuest;

		void Awake() {
            Random.InitState(Mathf.RoundToInt(DateTime.Now.Ticks % int.MaxValue));
			_quests.QuestStarted += q => SetQuest(q.Config);
			_saveManager.RegisterSaveFunction("FollowerData", GetSaveData);
			_saveManager.RegisterLoadFunction("FollowerData", LoadSaveData);
		}

		private class SaveData
        {
			public List<FollowerSaveData> _followerData;
        }

		private void LoadSaveData(object data)
        {
			if (data == null)
			{
				Loaded?.Invoke();
				return;
			}

			var saveData = (SaveData)data;

			_followers = saveData._followerData.Select(f => GetInstance(f)).ToList();
			Loaded?.Invoke();
		}

		private object GetSaveData()
        {
			return new SaveData()
			{
				_followerData = _followers.Select(f => f.SaveData).ToList()
			};
        }

		public List<FollowerConfig> AllFollowers;
		[ContextMenu("Add all followers in project")]
		private void SearchForAllItems()
		{
			EditorUtils.AddAllOfType(AllFollowers);
		}

		public int Count()
        {
			return _followers.Count;
        }

		public IEnumerable<FollowerInstance> GetFollowers()
        {
			return _followers;
        }

	    public void Initialize(bool newGame) {
			_lastQuest = _quests._currentQuest.Config;

			if(!newGame)
            {
				double secondsSinceLastPlayed = GlobalSaveData.SaveData.GetTimeSinceLastPlayed();

				Debug.Log("Time since last play: " + secondsSinceLastPlayed);

				CheckSpawns(secondsSinceLastPlayed);
			}
	    }

	    public void SetQuest(QuestConfig questConfig) {
			if (_lastQuest?.Area != questConfig.Area)
				ClearFollowers();

			_lastQuest = questConfig;
			currentTime = 0;
	        currentTimeToReach = Random.Range(questConfig.MinEnemyDropTime, questConfig.MaxEnemyDropTime);
        }

		public void CheckSpawns(double timeDelta) {
			currentTime += timeDelta;
			if (currentTime < currentTimeToReach) return;
			currentTime -= currentTimeToReach;
			currentTimeToReach = Random.Range(_quests._currentQuest.Config.MinEnemyDropTime, _quests._currentQuest.Config.MaxEnemyDropTime);
			if (_followers.Count == MAX_QUEUE_SIZE)
			{
				currentTime = 0;
				return;
			}
			AddRandomFollower();
			CheckSpawns(0);
		}

	    [ContextMenu("Add Follower")]
		public void AddRandomFollower() {
			FollowerInstance follower = GetRandomFollower(_quests._currentQuest.Config.Enemies.Drops);
			AddFollower(follower);
        }

		public bool AddFollower(FollowerConfig config, out FollowerInstance follower) {
			follower = GetFollower(config, _quests._currentQuest.Config.Enemies.Drops);
			return AddFollower(follower);
		}

		public bool Remove(FollowerInstance follower)
		{
			if (!_followers.Remove(follower)) return false;

			Debug.Log($"Follower removed: {follower.SaveData.Id}");
			FollowerRemoved?.Invoke(follower);

			return true;
		}
		private bool AddFollower(FollowerInstance follower)
        {
			if (_followers.Count >= MAX_QUEUE_SIZE || follower == null) return false;

			Debug.Log($"Follower added: {follower.Config.DisplayName}");
			_followers.Add(follower);
			FollowerAdded?.Invoke(follower);
			return true;
		}


        #region Creating Followers
        private FollowerInstance CreateSavedInstance(FollowerConfig config, int level)
		{
			FollowerInstance val;
			if (config is EnemyConfig e)
			{
				val = new EnemyInstance(e, level);
			}
			else if (config is QuestGiverConfig q)
			{
				val = new QuestGiverInstance(q);
			}
			else if (config is ShopFollowerConfig s)
			{
				val = new ShopFollowerInstance(s, _items);
			} 
			else
            {
				Debug.LogError("Tried to create a follower instance from config: " + config.name);
				return new FollowerInstance();
			}

			return val;
		}

		private FollowerInstance GetInstance(FollowerSaveData saveData)
		{
			FollowerInstance val;
			var config = AllFollowers.First(follower => follower.name == saveData.Id);
			if (saveData is EnemySaveData e)
			{
				val = new EnemyInstance(e, (EnemyConfig)config);
			}
			else if (saveData is QuestGiverSaveData q)
			{
				val = new QuestGiverInstance(q, (QuestGiverConfig)config);
			}
			else if (saveData is ShopFollowerSaveData s)
			{
				val = new ShopFollowerInstance(s, (ShopFollowerConfig)config, _items);
			}
			else
			{
				Debug.LogError("Tried to create a follower instance from config: " + saveData.Id);
				return new FollowerInstance();
			}

			return val;
		}

		private FollowerInstance GetFollower(FollowerConfig follower, List<FollowerDrop> drops)
		{
			var drop = drops.FirstOrDefault(d => d.Follower == follower);
			if(drop == null)
            {
				Debug.LogWarning($"No drop defined for follower: {follower.DisplayName}");
				return null;
            }
	
			return GetInstanceFromFollowerDrop(drop);
		}

		private FollowerInstance GetRandomFollower(List<FollowerDrop> drops)
		{
			if (drops.Count == 0) return null;

			int sum = drops.Sum(t => t.DropChance);
			int rand = Random.Range(0, sum);
			int count = 0;
			FollowerDrop d = drops.Last();
			foreach (var t in drops)
			{
				count += t.DropChance;

				if (rand < count)
				{
					d = t;
					break;
				}
			}

			return GetInstanceFromFollowerDrop(d);
		}

		private FollowerInstance GetInstanceFromFollowerDrop(FollowerDrop drop)
		{
			int Level = Mathf.RoundToInt(Random.value * (drop.MaxLevel - drop.MinLevel) + drop.MinLevel);
			return CreateSavedInstance(drop.Follower, Level);
		}
		#endregion

		public void DebugAdd(FollowerConfig config, int level)
		{
			AddFollower(CreateSavedInstance(config, level));
		}

#if UNITY_EDITOR
		[ContextMenu("Clear Followers")]
	    void ClearFollowers() {
		    _followers.Clear();
	    }
#endif
	}
}


