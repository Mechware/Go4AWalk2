using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using G4AW2.Dialogue;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Followers {

	public class FollowerDisplayController : MonoBehaviour {
	    public RuntimeSetFollowerData ListOfCurrentFollowers;
		public RuntimeSetQuest ListOfOpenQuests;

	    public FollowerDisplay DisplayPrefab;
		private List<FollowerDisplay> AllFollowers = new List<FollowerDisplay>();

		public UnityEvent FightFollower;

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

		public void FollowerClicked(FollowerData fd) {
			// Show some info on them.
			
			if (ListOfCurrentFollowers[0] == fd) {
				if (fd is EnemyData) {
					//EnemyData ed = (EnemyData) fd;
					// TODO: Include stats
					PopUp.SetPopUp("Fight follower?", new[] {"Yes", "No"}, new Action[] { FightFollower.Invoke, () => { }});
				} else if (fd is QuestGiver) {
					QuestGiver qg = (QuestGiver) fd;
					PopUp.SetPopUp("Accept quest from quest giver? Title: " + qg.QuestToGive.DisplayName, new[] { "Yes", "No" }, new Action[] {
						() => {
							ListOfOpenQuests.Add(qg.QuestToGive);
							ListOfCurrentFollowers.Remove(fd);
						}, () => { } });
				}
			} 
		}
	}
}

