using G4AW2.Component.UI;
using G4AW2.Component.World;
using G4AW2.Managers;
using System;
using System.Linq;
using UnityEngine;

namespace G4AW2.Followers
{
    public class FollowersDisplay : MonoBehaviour {

        [Header("Misc References")]
	    public FollowerDisplay DisplayPrefab;

		[Obsolete("Move to interaction controller")]
		public LerpToPosition WorldCameraLerper;
	    
		public Transform EnemyArrowPosition;

        [Header("Shop")]
	    public LerpToPosition ShopperWalk;
	    public ShopKeeperDisplay Shopper;
        

	    [Header("Quest Giver")]
	    public LerpToPosition QuestGiverWalk;
	    public QuestGiverDisplay QuestGiver;

	    private ObjectPrefabPool _followerPool;
		[SerializeField] private SmoothPopUpManager _smoothPopUp;
		[SerializeField] private FollowerLayoutController _layout;
		[SerializeField] private PopUp _popUp;
		[SerializeField] private FollowerManager _followerManager;

		public void Initialize(FollowerManager followerManager) {
			_followerManager = followerManager;
			_followerPool = new ObjectPrefabPool(DisplayPrefab.gameObject, transform);
			ResetFollowers();
        }

	    public void OnFollowerAdded(FollowerInstance d) {
			_smoothPopUp.ShowPopUpNew(EnemyArrowPosition.position, "<color=green>+1</color> " + d.Config.DisplayName, Color.white, true);
            ResetFollowers();
        }

		private void ResetFollowers() {
			_followerPool.Reset();

			for (int i = 0; i < _followerManager.Followers.Count; i++) {
				FollowerInstance fd = _followerManager.Followers[i];
			    GameObject go = _followerPool.GetObject();
			    FollowerDisplay d = go.GetComponent<FollowerDisplay>();
				AddDisplay(d, fd);
			}

			_layout.ChangeLayout(_followerPool.InUse.Select(g => g.GetComponent<FollowerDisplay>()));
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

					_popUp.SetPopUpNew(description, new[] {"Yes", "No"}, new Action[] {
					    () => {
						    InteractionCoordinator.Instance.EnemyFight(ed);
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

