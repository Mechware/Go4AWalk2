using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using G4AW2.Dialogue;
using G4AW2.Events;
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

		public UnityEventEnemyData FightFollower;
		public UnityEvent QuestGiverClicked;


        [Header("Shop")]
	    public LerpToPosition ShopperWalk;
	    public ShopGiverDisplay Shopper;
        public LerpToPosition WorldCameraLerper;

		void Awake() {
            // Remove listeneres
			ListOfCurrentFollowers.OnAdd.RemoveListener(FollowerAdded);
			ListOfCurrentFollowers.OnRemove.RemoveListener(FollowerRemoved);
			ListOfCurrentFollowers.OnChange.RemoveListener(FollowerRemoved);

            // Add listeners
			ListOfCurrentFollowers.OnAdd.AddListener(FollowerAdded);
			ListOfCurrentFollowers.OnRemove.AddListener(FollowerRemoved);
			ListOfCurrentFollowers.OnChange.AddListener(FollowerRemoved);
		}

		private void ResetFollowers() {
			// Should really re use these
			AllFollowers.ForEach(kvp => { Destroy(kvp.gameObject); });
			AllFollowers.Clear();

			for (int i = 0; i < ListOfCurrentFollowers.Value.Count; i++) {
				FollowerData fd = ListOfCurrentFollowers[i];
				FollowerDisplay display = Instantiate(DisplayPrefab, transform);
				AddDisplay(display, fd);
				AllFollowers.Add(display);
			}
		}

	    public void FollowerAdded(FollowerData d) {
			FollowerData fd = ListOfCurrentFollowers.Value.Last();
			FollowerDisplay display = Instantiate(DisplayPrefab, transform);
		    AddDisplay(display, fd);
			AllFollowers.Add(display);
		}

		private void AddDisplay(FollowerDisplay display, FollowerData d) {
			display.transform.SetAsFirstSibling();
		    Vector2 r = ((RectTransform) display.transform).sizeDelta;
		    r.x = d.SizeOfSprite.x;
		    r.y = d.SizeOfSprite.y;
		    ((RectTransform) display.transform).sizeDelta = r;

            display.SetData(d);
			display.FollowerClicked -= FollowerClicked;
			display.FollowerClicked += FollowerClicked;
		}

		public void FollowerRemoved(FollowerData d) {
			ResetFollowers();
		}

		public void FollowerClicked(FollowerDisplay fd) {
			// Show some info on them.
			
			if (AllFollowers[0] == fd) {
				if (fd.Data is EnemyData) {
					//EnemyData ed = (EnemyData) fd;
					// TODO: Include stats
					PopUp.SetPopUp("Fight follower?", new[] {"Yes", "No"}, new Action[] { () => { FightFollower.Invoke((EnemyData)fd.Data); }, () => { }});
				} else if (fd.Data is QuestGiver) {
					QuestGiver qg = (QuestGiver) fd.Data;
					PopUp.SetPopUp("Accept quest from quest giver? Title: " + qg.QuestToGive.DisplayName, new[] { "Yes", "No" }, new Action[] {
						() => {
							ListOfOpenQuests.Add(qg.QuestToGive);
							ListOfCurrentFollowers.Remove(fd.Data);
						}, () => { } });
				} else if (fd.Data is ShopFollower) {

				    WorldCameraLerper.StartLerping(() => {
				        Shopper.SetData(fd.Data);
                        Shopper.StartWalking();
				        ShopperWalk.StartLerping(() => {
				            Shopper.StopWalking();
                            Shopper.OnClick();
				        });
                    });
                }
			} 
		}
	}
}

