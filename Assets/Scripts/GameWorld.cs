using CustomEvents;
using G4AW2.Combat;
using G4AW2.Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for everything that appears in the game world
/// </summary>
public class GameWorld : MonoBehaviour
{
    [SerializeField] private DragObject _dragger;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerAnimations _playerAnims;
    [SerializeField] private EnemyArrowIndicator _arrow;
    [SerializeField] private ScrollingImages _background;

    [SerializeField] private DeadEnemyController _deadEnemies;
    [SerializeField] private MiningPoints _miningPoints;

    [SerializeField] private ShopGiverDisplay _shop;
    [SerializeField] private QuestGiverDisplay _questGiver;
    [SerializeField] private GameObject _questGiverDismissUI;

    [SerializeField] private EnemyDisplay _enemy;

    [SerializeField] private InteractionController _interactionController;

    [SerializeField] private PlayerFighterDisplay _playerDisplay;

    [SerializeField] private GameObject _attackArea;

    [SerializeField] private RuntimeSetFollowerData _followers;



    private Inventory _inventory;

    public void Initialize(Inventory i)
    {
        _inventory = i;
        
        // Background events
        {
            _background.OnScrolled += _miningPoints.Scroll;
            _background.OnScrolled += _deadEnemies.Scroll;
            _background.OnScrolled += _shop.OnScroll;

            _dragger.OnDragEvent += _background.Pause;
            _dragger.OnDragEvent += _playerAnims.StopWalking;
            _dragger.OnDragEvent += () => _arrow.SetOnMainScreen(false);

            _dragger.OnReset += _background.Play;
            _dragger.OnReset += _playerAnims.StartWalking;
            _dragger.OnReset += () => _arrow.SetOnMainScreen(true);

            _miningPoints.OnItemReceived += _inventory.Add;
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
                    _followers.Remove(enemy);
                    _deadEnemies.AddDeadEnemy(
                        _enemy.RectTransform.anchoredPosition.x,
                        _enemy.RectTransform.anchoredPosition.y,
                        enemy);
                    _dragger.Enable();
                };

                _interactionController.OnPlayerDeath += () =>_attackArea.SetActive(false);

                _interactionController.OnPlayerDeathReset += _dragger.ResetScrolling;
                _interactionController.OnPlayerDeathDone += f => _dragger.Enable();


                _enemy.OnAttack += _playerDisplay.Hit;
                _playerDisplay.OnHit += _enemy.OnHit;

                _playerDisplay.OnSuccessfulParry.AddListener(_enemy.Stun);
                _playerDisplay.OnHit += (a, b) => _playerAnims.Attack();

                _player.OnDeath += _interactionController.PlayerDied;
            }
        }

        // Initialization
        {
            _playerDisplay.Initialize(_enemy, _player);
            _playerAnims.Initialize(_player);
        }
        
    }
}
