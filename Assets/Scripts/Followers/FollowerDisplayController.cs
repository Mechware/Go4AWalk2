using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using G4AW2.Dialogue;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Followers {

	public class FollowerDisplayController : MonoBehaviour {

		public static FollowerDisplayController Instance;
		
		
        [Header("Misc References")]
	    public FollowerDisplay DisplayPrefab;
	    public LerpToPosition WorldCameraLerper;
	    public Transform EnemyArrowPosition;

        [Header("Shop")]
	    public LerpToPosition ShopperWalk;
	    public ShopKeeperDisplay Shopper;
        

	    [Header("Quest Giver")]
	    public LerpToPosition QuestGiverWalk;
	    public QuestGiverDisplay QuestGiver;

	    [NonSerialized] public ObjectPrefabPool FollowerPool;

	    void Awake() {
		    Instance = this;
	        FollowerPool = new ObjectPrefabPool(DisplayPrefab.gameObject, transform);
	    }

	    void Start() {
		    FollowerManager.Instance.FollowerAdded += OnAdd;
		    FollowerManager.Instance.FollowerRemoved += ResetFollowersWithDummyParam;
		    
	    }
	    
        public void Initialize() {
	        ResetFollowers();
        }

	    private void ResetFollowersWithDummyParam(FollowerInstance d) {
	        ResetFollowers();
	    }

	    private void OnAdd(FollowerInstance d) {
            SmoothPopUpManager.ShowPopUp(EnemyArrowPosition.position, "<color=green>+1</color> " + d.Config.DisplayName, Color.white, true);
            ResetFollowers();
        }

		private void ResetFollowers() {
			FollowerPool.Reset();

			for (int i = 0; i < FollowerManager.Instance.Followers.Count; i++) {
				FollowerInstance fd = FollowerManager.Instance.Followers[i];
			    GameObject go = FollowerPool.GetObject();
			    FollowerDisplay d = go.GetComponent<FollowerDisplay>();
				AddDisplay(d, fd);
			}

            FollowerLayoutController.Instance.ChangeLayout();
		}

        private void AddDisplay(FollowerDisplay display, FollowerInstance d) {
			display.transform.SetAsFirstSibling();
		    Vector2 r = ((RectTransform) display.transform).sizeDelta;
		    r.x = d.Config.SizeOfSprite.x;
		    r.y = d.Config.SizeOfSprite.y;
		    ((RectTransform) display.transform).sizeDelta = r;

            display.SetData(d);
			display.FollowerClicked -= FollowerClicked;
			display.FollowerClicked += FollowerClicked;
		}

		public void FollowerClicked(FollowerDisplay fd) {
			// Show some info on them.
			
			if (FollowerManager.Instance.Followers[0] == fd.Instance) {
				if (fd.Instance is EnemyInstance) {
					EnemyInstance ed = (EnemyInstance) fd.Instance;

				    string elemDmg = ed.Config.HasElementalDamage ? ed.ElementalDamage.ToString() : "0";
				    string elemColor = ed.Config.HasElementalDamage ? "#" + ColorUtility.ToHtmlStringRGB(ed.Config.ElementalDamageType.DamageColor) : "black";


                    string description = $@"Fight a level {ed.SaveData.Level} {ed.Config.DisplayName}?
<color=green>Health: {ed.MaxHealth}</color>
<color=red>Damage: {ed.Damage}</color>
Elemental Damage: <color={elemColor}>{elemDmg}</color>";

					PopUp.SetPopUp(description, new[] {"Yes", "No"}, new Action[] {
					    () => {
						    InteractionController.Instance.EnemyFight(ed);
					    },
                        () => { }});
				} else if (fd.Instance is QuestGiverInstance q) {

				    WorldCameraLerper.StartLerping(() => {
					    PlayerAnimations.Instance.Spin();
                        QuestGiver.SetData(q);
				        QuestGiver.StartWalking();
				        QuestGiverWalk.StartLerping(() => {
				            QuestGiver.StopWalking();
				            QuestGiver.OnPointerClick(null);
				        });
				    });

				} else if (fd.Instance is ShopFollowerInstance shop) {

				    WorldCameraLerper.StartLerping(() => {
					    PlayerAnimations.Instance.Spin();
				        Shopper.SetData(shop);
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

