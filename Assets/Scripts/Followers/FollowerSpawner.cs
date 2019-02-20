using CustomEvents;
using G4AW2.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace G4AW2.Followers {
	public class FollowerSpawner : MonoBehaviour {

        public FollowerDropData DropData;
        public RuntimeSetFollowerData CurrentFollowers;

		public float RandomTimeMin;
		public float RandomTimeMax;
		public float RandomDistanceMin;
		public float RandomDistanceMax;

		private float currentTimeToReach;
		private float currentDistanceToReach;

		private float currentTime;
		private float currentDistance;

		void Awake() {
			currentTimeToReach = Random.Range(RandomTimeMin, RandomTimeMax);
			currentDistanceToReach = Random.Range(RandomDistanceMin, RandomDistanceMax);
		}

		void Update() {
			currentTime += Time.deltaTime;
			CheckSpawns();
		}

		public void UpdateDistance(float travelled) {
			currentDistance += travelled;
			CheckSpawns();
		}

		private void CheckSpawns() {
			if (currentTime > currentTimeToReach) {
				currentTime -= currentTimeToReach;
				currentTimeToReach = Random.Range(RandomTimeMin, RandomTimeMax);
				AddFollower();
				CheckSpawns();
			}
			if (currentDistance > currentDistanceToReach) {
				currentDistance -= currentDistanceToReach;
				currentDistanceToReach = Random.Range(RandomDistanceMin, RandomDistanceMax);
				AddFollower();
				CheckSpawns();
			}
		}

	    [ContextMenu("Add Follower")]
		public void AddFollower() {
            // randomly choose a follower!
	        if (CurrentFollowers.Value.Count >= 10) return;

		    CurrentFollowers.Add(DropData.GetRandomFollower());
        }

#if UNITY_EDITOR
	    [ContextMenu("Clear Followers")]
	    void ClearFollowers() {
		    CurrentFollowers.Value.Clear();
	    }

	    
#endif
	}
}


