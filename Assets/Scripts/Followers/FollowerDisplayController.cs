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

        [Header("Misc References")]
        public RuntimeSetFollowerData ListOfCurrentFollowers;
	    public FollowerDisplay DisplayPrefab;
	    public LerpToPosition WorldCameraLerper;

        [Header("Shop")]
	    public LerpToPosition ShopperWalk;
	    public ShopGiverDisplay Shopper;
        

	    [Header("Quest Giver")]
	    public LerpToPosition QuestGiverWalk;
	    public QuestGiverDisplay QuestGiver;

	    [Header("Events")]
	    public UnityEventEnemyData FightFollower;
	    public UnityEvent ListChanged;
	    public UnityEvent CameraDoneAfterScroll;
	    public UnityEvent CameraDoneAfterScrollFighting;

	    [NonSerialized] public ObjectPrefabPool FollowerPool;

	    void Awake() {
	        FollowerPool = new ObjectPrefabPool(DisplayPrefab.gameObject, transform);
	    }

        public void AfterLoadEvent() {

	        ResetFollowers();

            // Remove listeneres
            ListOfCurrentFollowers.OnAdd.RemoveListener(ResetFollowersWithDummyParam);
			ListOfCurrentFollowers.OnRemove.RemoveListener(ResetFollowersWithDummyParam);
			ListOfCurrentFollowers.OnChange.RemoveListener(ResetFollowersWithDummyParam);

            // Add listeners
			ListOfCurrentFollowers.OnAdd.AddListener(ResetFollowersWithDummyParam);
			ListOfCurrentFollowers.OnRemove.AddListener(ResetFollowersWithDummyParam);
			ListOfCurrentFollowers.OnChange.AddListener(ResetFollowersWithDummyParam);
		}

	    private void ResetFollowersWithDummyParam(FollowerData d) {
	        ResetFollowers();
	    }

		private void ResetFollowers() {
			FollowerPool.Reset();

			for (int i = 0; i < ListOfCurrentFollowers.Value.Count; i++) {
				FollowerData fd = ListOfCurrentFollowers[i];
			    GameObject go = FollowerPool.GetObject();
			    FollowerDisplay d = go.GetComponent<FollowerDisplay>();
				AddDisplay(d, fd);
			}

            ListChanged.Invoke();
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

		public void FollowerClicked(FollowerDisplay fd) {
			// Show some info on them.
			
			if (ListOfCurrentFollowers[0] == fd.Data) {
				if (fd.Data is EnemyData) {
					//EnemyData ed = (EnemyData) fd;
					// TODO: Include stats
					PopUp.SetPopUp("Fight follower?", new[] {"Yes", "No"}, new Action[] {
					    () => {
                            
					        FightFollower.Invoke((EnemyData)fd.Data);
                            WorldCameraLerper.StartLerping(() => {
                                CameraDoneAfterScrollFighting.Invoke();
                            });
					    },
                        () => { }});
				} else if (fd.Data is QuestGiver) {

				    WorldCameraLerper.StartLerping(() => {
				        CameraDoneAfterScroll.Invoke();
                        QuestGiver.SetData(fd.Data);
				        QuestGiver.StartWalking();
				        QuestGiverWalk.StartLerping(() => {
				            QuestGiver.StopWalking();
				            QuestGiver.OnPointerClick(null);
				        });
				    });

				} else if (fd.Data is ShopFollower) {

				    WorldCameraLerper.StartLerping(() => {
				        CameraDoneAfterScroll.Invoke();
				        Shopper.SetData(fd.Data);
                        Shopper.StartWalking();
				        ShopperWalk.StartLerping(() => {
				            Shopper.StopWalking();
                            Shopper.OnPointerClick(null);
				        });
                    });
                }
			} 
		}
	}
}

