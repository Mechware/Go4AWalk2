using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data;
using UnityEngine;

namespace G4AW2.Followers {
	[CreateAssetMenu(menuName = "Data/Follower/DropData")]
	public class FollowerDropData : ScriptableObject {

		[System.Serializable]
		public class FollowerDrop {
			public FollowerData Follower;
			public int DropChance;
		}

		public List<FollowerDrop> Drops;

		public FollowerData GetRandomFollower() {
			int sum = Drops.Sum(t => t.DropChance);
			int rand = Random.Range(0, sum);
			int count = 0;
			foreach (var t in Drops) {
				count += t.DropChance;

				if (rand < count) {
					return t.Follower;
				}
			}
			return Drops.Last().Follower;
		}

#if UNITY_EDITOR
		[ContextMenu("Print random follower")]
		public void Get100Followers() {
			FollowerData follower = GetRandomFollower();
			Debug.Log(follower.name);
		}

		[ContextMenu("Get 1000000 new followers")]
		public void Get1000000Followers() {
			PrintStatistics(1000000);
		}

		public void PrintStatistics( int amount ) {
			Dictionary<FollowerData, int> amounts = new Dictionary<FollowerData, int>();
			for (int i = 0; i < amount; i++) {
				var f = GetRandomFollower();
				if (amounts.ContainsKey(f)) {
					int timesSeen = amounts[f] + 1;
					amounts.Remove(f);
					amounts.Add(f, timesSeen);
				} else {
					amounts.Add(f, 1);
				}
			}

			foreach (var kvp in amounts) {
				Debug.Log("Key: " + kvp.Key + " Percent spawned: " + kvp.Value / (float)amount);
			}
		}
#endif
	}
}

