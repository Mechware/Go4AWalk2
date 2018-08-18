using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Followers {

	public class FollowerDisplayController : MonoBehaviour {
		private FollowerDisplay DisplayPrefab;
		private Dictionary<FollowerData, FollowerDisplay> AllFollowers;

		public void AddFollower(FollowerData data) {
			print("Adding follower");
			FollowerDisplay display = Instantiate(DisplayPrefab, transform);
			AllFollowers.Add(data, display);
		}

		public void RemoveFollower(FollowerData data) {
			print("Removing follower");
			if (!AllFollowers.ContainsKey(data)) {
				Debug.LogError("Tried to remove a follower who does not exist in the follower list.");
				return;
			}
			Destroy(AllFollowers[data]);
			AllFollowers.Remove(data);
		}
	}
}

