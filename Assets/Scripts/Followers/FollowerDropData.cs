using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data;
using G4AW2.Data.Combat;
using UnityEngine;

namespace G4AW2.Followers {
	[System.Serializable]
	public class FollowerDropData {

		public List<FollowerDrop> Drops;

		public EnemyInstance GetFollower(FollowerConfig enemy, bool includeGlobal) {
			
			List<FollowerDrop> drops = includeGlobal ? Drops.Concat(GlobalFollowerDrops.GlobalDrops).ToList() : Drops;
			
			foreach (var drop in drops) {
				if (drop.Follower == enemy) {
					return GetInstanceFromFollowerDrop(drop);
				}
			}

			return null;
		}
		
		public EnemyInstance GetRandomFollower(bool includeGlobal) {

		    List<FollowerDrop> drops = includeGlobal ? Drops.Concat(GlobalFollowerDrops.GlobalDrops).ToList() : Drops;

		    if (drops.Count == 0) return null;

			int sum = drops.Sum(t => t.DropChance);
			int rand = Random.Range(0, sum);
			int count = 0;
			FollowerDrop d = drops.Last();
			foreach (var t in drops) {
				count += t.DropChance;

				if (rand < count) {
					d = t;
					break;
				}
			}

			return GetInstanceFromFollowerDrop(d);
		}

		public EnemyInstance GetInstanceFromFollowerDrop(FollowerDrop drop) {

			int Level = Mathf.RoundToInt(Random.value * (drop.MaxLevel - drop.MinLevel) + drop.MinLevel);
			return new EnemyInstance(drop.Follower, Level);
		}
	}
}

