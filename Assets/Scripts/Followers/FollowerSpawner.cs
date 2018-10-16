using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Followers {
    public class FollowerSpawner : MonoBehaviour {

        public RuntimeSetFollowerData AllPossibleFollowers;
        public RuntimeSetFollowerData CurrentFollowers;

	    [ContextMenu("Add Follower")]
		public void AddFollower() {
            // randomly choose a follower!
	        if (CurrentFollowers.Value.Count >= 10) return;
            CurrentFollowers.Add(AllPossibleFollowers.Value.GetRandom());
        }

#if UNITY_EDITOR
	    [ContextMenu("Clear Followers")]
	    void ClearFollowers() {
		    CurrentFollowers.Value.Clear();
	    }
#endif
	}
}


