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

        [SerializeField] private DeadEnemyController _deadEnemies;

        [SerializeField] private QuestGiverDisplay _questGiver;
        [SerializeField] private GameObject _questGiverDismissUI;

        [SerializeField] private EnemyDisplay _enemy;

        [SerializeField] private InteractionCoordinator _interactionController;

        [SerializeField] private PlayerFighterDisplay _playerDisplay;

        [SerializeField] private GameObject _attackArea;

        [SerializeField] private PlayerAnimations _playerAnims;
        [SerializeField] private ShopKeeperDisplay _shop;

        [SerializeField] private AreaDisplay _areaManager;
        [SerializeField] private MiningPoints _mining;

        [SerializeField] private FollowersDisplay _followerDisplay;

        public bool CanScroll => _dragger.IsAtEnd() && _dragger.ScrollingEnabled;

        public void Initialize(ItemManager _inventory, PlayerManager _player, FollowerManager fm, GameEvents _events)
        {
            _events.OnQuestSet += q => _areaManager.SetArea(q.Config.Area);
            _events.OnQuestSet += q => _mining.SetQuest(q.Config);
            // Background events
            {
                _background.OnScrolled += _mining.Scroll;
                _background.OnScrolled += _deadEnemies.Scroll;
                //_background.OnScrolled += _shop.OnScroll;

                _dragger.OnDragEvent += _background.Pause;
                _dragger.OnDragEvent += _playerAnims.StopWalking;
                //_dragger.OnDragEvent += () => _arrow.SetOnMainScreen(false);

                _dragger.OnReset += _background.Play;
                _dragger.OnReset += _playerAnims.StartWalking;
                //_dragger.OnReset += () => _arrow.SetOnMainScreen(true);

                _mining.OnItemReceived += _inventory.Add;
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
                    _interactionController.OnBossFightStarted += _deadEnemies.ClearEnemies;
                    _interactionController.OnBossFightStarted += _dragger.Disable;
                    _interactionController.OnBossFightStarted += _playerAnims.StopWalking;

                    _interactionController.OnFightEnter += _dragger.Disable;
                    _interactionController.OnFightEnter += _playerAnims.StopWalking;

                    _interactionController.OnFightStart += () => _attackArea.SetActive(true);

                    _interactionController.OnEnemyDeath += (it) => _attackArea.SetActive(false);

                    _interactionController.OnEnemyDeathFinished += (enemy) =>
                    {
                        SaveGame.SaveData.CurrentFollowers.Remove(enemy.SaveData);
                        _deadEnemies.AddDeadEnemy(
                            _enemy.RectTransform.anchoredPosition.x,
                            _enemy.RectTransform.anchoredPosition.y,
                            enemy);
                        _dragger.Enable();
                    };

                    _interactionController.OnPlayerDeath += () => _attackArea.SetActive(false);

                    _interactionController.OnPlayerDeathReset += _dragger.ResetScrolling;
                    _interactionController.OnPlayerDeathDone += f => _dragger.Enable();


                    _enemy.OnAttack += _playerDisplay.Hit;
                    _playerDisplay.OnHit += _enemy.OnHit;

                    _playerDisplay.OnSuccessfulParry.AddListener(_enemy.Stun);
                    _playerDisplay.OnHit += (a, b) => _playerAnims.Attack();

                    _player.OnDeath += _interactionController.PlayerDied;
                }
            }

            _events.FollowerAdded += _followerDisplay.OnFollowerAdded;

            // Initialization
            {
                _playerDisplay.Initialize(_enemy, _player);
                _playerAnims.Initialize(_player);
                _followerDisplay.Initialize(fm);
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

        public void GameUpdate(float gameUpdate)
        {

        }
    }

}

