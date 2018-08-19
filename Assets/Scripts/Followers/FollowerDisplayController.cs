using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using G4AW2.Variables;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Followers {

	public class FollowerDisplayController : MonoBehaviour {
	    public FollowerDataListVariable ListOfCurrentFollowers;

	    public FollowerDisplay DisplayPrefab;
		private List<FollowerDisplay> AllFollowers = new List<FollowerDisplay>();

	    public void FollowersChanged(List<FollowerData> d) {
            print("Followers changed");

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

