using DG.Tweening;
using G4AW2.Combat;
using G4AW2.Followers;
using G4AW2.Managers;
using G4AW2.UI.Areas;
using UnityEngine;

namespace G4AW2.Controller
{
    public class WorldController : MonoBehaviour
    {
        [SerializeField] private DragObject _dragger;

        [SerializeField] private ScrollingImages _background;

        [SerializeField] private QuestGiverDisplay _questGiver;
        [SerializeField] private GameObject _questGiverDismissUI;

        [SerializeField] private EnemyDisplay _enemy;


        [SerializeField] private PlayerFighterDisplay _playerDisplay;

        [SerializeField] private GameObject _attackArea;

        [SerializeField] private PlayerAnimations _playerAnims;
        [SerializeField] private ShopKeeperDisplay _shop;

        [SerializeField] private FollowersDisplay _followerDisplay;

        [SerializeField] private InteractionCoordinator _interactionController;

        [SerializeField] private QuestManager _quests;
        [SerializeField] private FollowerManager _followers;

        public bool CanScroll => _dragger.IsAtEnd() && _dragger.ScrollingEnabled;

        public void Awake()
        {
            // Background events
            {
                _dragger.OnDragEvent += _background.Pause;
                _dragger.OnDragEvent += _playerAnims.StopWalking;

                _dragger.OnReset += _background.Play;
                _dragger.OnReset += _playerAnims.StartWalking;
            }


            // Interactions
            {

                _shop.StartedWalking += _dragger.Disable;
                _shop.FinishInteraction += _dragger.Enable;
                _shop.FinishInteraction += _playerAnims.Spin;

                _questGiver.StartedWalking += _dragger.Disable;
                _questGiver.FinishInteraction += _dragger.Enable;
                _questGiver.FinishInteraction += _playerAnims.Spin;
                _questGiver.FinishInteraction += () => _questGiverDismissUI.SetActive(false);

                // Fighting
                {
                    _enemy.OnAttack += _playerDisplay.Hit;
                    _playerDisplay.OnHit += _enemy.OnHit;

                    _playerDisplay.OnSuccessfulParry.AddListener(_enemy.Stun);
                    _playerDisplay.OnHit += (a, b) => _playerAnims.Attack();
                }
            }
        }
        public void ScrollToEnemies()
        {
            _dragger.InvokeDragEvent();
            _dragger.Disable();
            var rt = ((RectTransform)_dragger.transform);
            DOTween.Sequence()
                .Append(rt.DOAnchorPosX(75, 1))
                .AppendCallback(() => {
                    _dragger.Enable();
                });
        }
    }

}

