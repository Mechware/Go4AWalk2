using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			ResetFollowers();
		}

		private void ResetFollowers() {
			// Should really re use these
			AllFollowers.ForEach(kvp => { Destroy(kvp.gameObject); });
			AllFollowers.Clear();

			for (int i = 0; i < ListOfCurrentFollowers.Value.Count; i++) {
				FollowerData fd = ListOfCurrentFollowers[i];
				FollowerDisplay display = Instantiate(DisplayPrefab, transform);
				display.transform.SetAsFirstSibling();
				display.SetData(fd);
				AllFollowers.Add(display);
			}
		}

	    public void FollowerAdded(FollowerData d) {
			FollowerData fd = ListOfCurrentFollowers.Value.Last();
			FollowerDisplay display = Instantiate(DisplayPrefab, transform);
		    display.transform.SetAsFirstSibling();
		    display.SetData(fd);
		    AllFollowers.Add(display);
		}

		public void FollowerRemoved(FollowerData d) {
			ResetFollowers();
		}

	    void OnEnable() {
            ListOfCurrentFollowers.OnAdd.AddListener(FollowerAdded);
            ListOfCurrentFollowers.OnRemove.AddListener(FollowerRemoved);
            ListOfCurrentFollowers.OnChange.AddListener(FollowerRemoved);
		}

	    void OnDisable() {
            ListOfCurrentFollowers.OnAdd.RemoveListener(FollowerAdded);
		    ListOfCurrentFollowers.OnRemove.RemoveListener(FollowerRemoved);
		}
	}
}

