using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Followers {

	public class FollowerDisplayController : MonoBehaviour {
	    public RuntimeSetFollowerData ListOfCurrentFollowers;

	    public FollowerDisplay DisplayPrefab;
		private List<FollowerDisplay> AllFollowers = new List<FollowerDisplay>();

		void Start() {
			FollowersChanged(null);
		}

	    public void FollowersChanged(FollowerData d) {

            // Should really re use these
	        AllFollowers.ForEach(kvp => { Destroy(kvp.gameObject); });
            AllFollowers.Clear();

	        foreach (var follower in ListOfCurrentFollowers.Value) {
	            FollowerDisplay display = Instantiate(DisplayPrefab, transform);
	            display.SetData(follower);
	            AllFollowers.Add(display);
            }
	    }

	    void OnEnable() {
            ListOfCurrentFollowers.OnChange.AddListener(FollowersChanged);
	    }

	    void OnDisable() {
            ListOfCurrentFollowers.OnChange.RemoveListener(FollowersChanged);
	    }
	}
}

