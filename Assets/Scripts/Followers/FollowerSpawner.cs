using CustomEvents;
using G4AW2.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace G4AW2.Followers {
	public class FollowerSpawner : MonoBehaviour {

        public FollowerDropData DropData;
        public RuntimeSetFollowerData CurrentFollowers;

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


