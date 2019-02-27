using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data;
using UnityEngine;

namespace G4AW2.Followers {
	[System.Serializable]
	public class FollowerDropData {

		public List<FollowerDrop> Drops;

		public FollowerData GetRandomFollower(bool includeGlobal) {

		    List<FollowerDrop> drops = includeGlobal ? this.Drops.Concat(GlobalFollowerDrops.GlobalDrops).ToList() : this.Drops;

			int sum = drops.Sum(t => t.DropChance);
			int rand = Random.Range(0, sum);
			int count = 0;
			foreach (var t in drops) {
				count += t.DropChance;

				if (rand < count) {
					return t.Follower;
				}
			}
			return drops.Last().Follower;
		}

#if UNITY_EDITOR
		[ContextMenu("Print random follower")]
		public void Get100Followers() {
			FollowerData follower = GetRandomFollower(false);
			Debug.Log(follower.name);
		}

		[ContextMenu("Get 1000000 new followers")]
		public void Get1000000Followers() {
			PrintStatistics(1000000);
		}

		public void PrintStatistics( int amount ) {
			Dictionary<FollowerData, int> amounts = new Dictionary<FollowerData, int>();
			for (int i = 0; i < amount; i++) {
				var f = GetRandomFollower(false);
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

