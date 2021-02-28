using G4AW2;
using G4AW2.Combat;
using G4AW2.Component.UI;
using G4AW2.Data;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Managers;
using G4AW2.UI.Areas;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCoordinator : MonoBehaviour {

    [Obsolete("Singleton")] public static InteractionCoordinator Instance;

    public bool Fighting { private set; get; }

    public LerpToPosition LerperToBattleArea;
    public LerpToPosition EnemyPositionLerper;
    public RobustLerper DeathCover;

    [SerializeField] private EnemyDisplay _enemy;
    [SerializeField] private ScrollingImages _backgroundImages;
    [SerializeField] private PlayerAnimations _playerAnims;
    [SerializeField] private ItemDropBubbleManager _itemDropper;
    [SerializeField] private AttackArea _attackArea;
    [SerializeField] private DragObject _dragger;
    [SerializeField] private Dialogue _questDialog;
    [SerializeField] private PopUp _popUp;

    [SerializeField] private PlayerManager _player;
    [SerializeField] private ItemManager _items;
    [SerializeField] private QuestManager _quests;
    [SerializeField] private FollowerManager _followers;

    [Header("Boss References")]
    public Transform BossStartPosition;
    public Transform BossEndPosition;

    public int ScrollSpeed;
    public RectTransform PlayerRT;

    public MyButton FightButton;

    public Transform PlayerFightPosition;
    public Transform EnemyFightPosition;

    public float EnemyAndPlayerWalkSpeed;

    public Action<EnemyInstance> OnEnemyDeathFinished;
    public Action OnFightEnter;
    public Action OnFightStart;
    public Action<EnemyInstance> OnEnemyDeath;
    public Action OnPlayerDeath;
    public Action OnPlayerDeathReset;
    public Action<float> OnPlayerDeathDone;
    public Action<Area> AreaChanged;

    public GameObject AreaChangeAndFadeObject;
    public RobustLerperSerialized AreaChangeInterpolater;

    private void Awake() {
        Instance = this;

        // TODO: Unsub
        _player.OnDeath += PlayerDied;
        _enemy.OnDeath += EnemyDeath;
        _quests.QuestFinished += FinishQuest;
        _quests.QuestStarted += (quest) =>
        {
            if (quest.Config.Area != AreaDisplay.Instance.CurrentArea)
            {
                AreaChangeAndFadeObject.SetActive(true);
                AreaChangeInterpolater.StartLerping(() => {

                    AreaChanged?.Invoke(quest.Config.Area);

                    AreaChangeInterpolater.StartReverseLerp(() => {

                        AreaChangeAndFadeObject.SetActive(false);

                        _questDialog.SetConversation(quest.Config.StartConversation, () => 
                        {
                            if (quest.Config.QuestType == QuestType.Boss)
                            {
                                StartBossFight(quest);
                            }
                        });

                    });
                });
            }
            else
            {
                _questDialog.SetConversation(quest.Config.StartConversation, () =>
                {
                    if (quest.Config.QuestType == QuestType.Boss)
                    {
                        StartBossFight(quest);
                    }
                });
            }
        };
    }

    void Update()
    {
        AreaChangeInterpolater.Update(Time.deltaTime);
    }

    public void QuestGiverEnterance()
    {

    }

    public void StartInteraction() {
        _dragger.Disable();
        _playerAnims.StopWalking();
    }


    public void StartBossFight(QuestInstance bossQuest) {

        StartCoroutine(_StartBossFight());

        IEnumerator _StartBossFight() {
            Fighting = true;
            OnFightEnter?.Invoke();
            StartInteraction();

            // Put boss where they should be
            EnemyConfig bossConfig = (EnemyConfig)bossQuest.Config.QuestParam;
            _followers.AddFollower(bossConfig, out var bossFollower);
            EnemyInstance boss = (EnemyInstance)bossFollower;

            _enemy.gameObject.SetActive(true);
            _enemy.SetEnemy(boss);
            
            var bossRT = _enemy.RectTransform; 
            bossRT.position = BossStartPosition.position;
            // Ensure boss is facing the right direction
            bossRT.localScale = bossRT.localScale.SetX(-1);

            int oldScrollSpeed = _backgroundImages.ScrollSpeed;
            _backgroundImages.ScrollSpeed = ScrollSpeed;
            
            // Wait until boss is in proper position
            while (bossRT.position.x > BossEndPosition.position.x) {
                float dist = -Time.deltaTime * ScrollSpeed;
                bossRT.localPosition = bossRT.localPosition.AddX(dist);
                PlayerRT.localPosition = PlayerRT.localPosition.AddX(dist);
                yield return null;
            }

            _backgroundImages.ScrollSpeed = oldScrollSpeed;
            _backgroundImages.Pause();

            
            // Pop up Fight button and wait for click
            FightButton.gameObject.SetActive(true);
            FightButton.onClick.RemoveAllListeners();
            bool fightClicked = false;
            
            FightButton.onClick.AddListener(() => {
                FightButton.onClick.RemoveAllListeners();
                FightButton.gameObject.SetActive(false);
                fightClicked = true;
            });

            while (!fightClicked) yield return null;

            // Make boss & player switch positions
            _playerAnims.StartWalking();
            _enemy.StartWalkingAnimation();

            bool playerReady = false;
            bool playerSpinning = false;
            bool enemyReady = false;
            while (!playerReady || !enemyReady) {
                if (!playerReady && !playerSpinning) {
                    if (PlayerRT.position.x >= PlayerFightPosition.position.x) {
                        _playerAnims.StopWalking();
                        playerSpinning = true;
                        _playerAnims.Spin(() => {
                            playerReady = true;
                            playerSpinning = false;
                        });
                    }
                    else {
                        float dist = Time.deltaTime * EnemyAndPlayerWalkSpeed;
                        PlayerRT.localPosition = PlayerRT.localPosition.AddX(dist);    
                    }
                    
                }
                
                if (!enemyReady) {
                    if (bossRT.position.x <= EnemyFightPosition.position.x) {
                        bossRT.localScale = bossRT.localScale.SetX(1);
                        enemyReady = true;
                        _enemy.StopWalking();
                    }
                    else {
                        float dist = -Time.deltaTime * EnemyAndPlayerWalkSpeed;
                        bossRT.localPosition = bossRT.localPosition.AddX(dist);    
                    }
                }

                yield return null;
            }
            
            // Once boss and player show up in proper places, start the fight like regular.
            _enemy.StartAttacking();
            _attackArea.gameObject.SetActive(true);
            OnFightStart?.Invoke();
        }
    }
    
    public void EnemyFight(EnemyInstance enemy) {
        OnFightEnter?.Invoke();
        StartInteraction();
        
        // Scroll to fight area
        LerperToBattleArea.StartLerping(() => {
            _playerAnims.Spin();
            
            _enemy.gameObject.SetActive(true);
            _enemy.SetEnemy(enemy);
            _enemy.StartWalkingAnimation();
            EnemyPositionLerper.StartLerping(() => {

                // Start Combat
                _enemy.StopWalking();
                _enemy.StartAttacking();
                _attackArea.gameObject.SetActive(true);
                OnFightStart?.Invoke();
            });
        });
    }


    public void EnemyDeath(EnemyInstance data) {

        StartCoroutine(_EnemyDeath());
        
        IEnumerator _EnemyDeath() {

            _attackArea.gameObject.SetActive(false);

            OnEnemyDeath?.Invoke(data);

            if (_player.Health <= 0) {
                yield break;    
            }
            
            if (data.Config.OneAndDoneAttacker && data.Suicide) {
                _playerAnims.ResetAttack();
                
                yield return new WaitForSeconds(1); // Wait for suicide animation to complete
                _playerAnims.Spin(() => {
                    _Done(new List<ItemInstance>());
                });
                yield break;
            }
        
            SoundManager.Instance.PlaySound(SoundManager.Instance.Celebrate, 0.8f);
            
            bool celebrateDone = false;
            bool bubblesDone = false;
        
            List<ItemInstance> items = data.Config.Drops.GetItems(data.SaveData.Level, _items);

            // Wait for player celebration to be done and all items to be picked up
            _playerAnims.ResetAttack();
            _playerAnims.Celebrate(() => {
                celebrateDone = true;
                if(bubblesDone && celebrateDone) {
                    _Done(items);
                }
            });

            _itemDropper.AddItems(items, null, () => {
                bubblesDone = true;
                if(bubblesDone && celebrateDone) {
                    _Done(items);
                }
            });
        }
        

        void _Done(List<ItemInstance> items) {
            _playerAnims.Spin(() => {
            
                _enemy.gameObject.SetActive(false);
            
                OnEnemyDeathFinished?.Invoke(data);
                _followers.Remove(data);
                _dragger.Enable();
                items.ForEach(_items.Add);
                Fighting = false;
            });
        }
    }

    public void FinishQuest(QuestInstance q)
    {
        var config = q.Config;
        _questDialog.SetConversation(config.EndConversation, () => {

            List<ItemInstance> todrops = new List<ItemInstance>();
            foreach (var reward in config.QuestRewards)
            {
                ItemConfig it = reward.it;
                var instance = _items.CreateInstance(it, reward.Level, reward.RandomRoll);
                _items.Add(instance);
                todrops.Add(instance);
            }

            if (todrops.Count == 0)
            {
                _TryAdvanceQuest();
            }
            else
            {

                _dragger.Disable2();

                ItemDropBubbleManager.Instance.AddItems(todrops, null, () => {
                    _dragger.Enable2();
                    _TryAdvanceQuest();
                });
            }
        });


        void _TryAdvanceQuest()
        {

            _quests.CurrentQuest.SaveData.Complete = true;

            if (config.NextQuestConfig == null)
            {
                _popUp.SetPopUpNew(
                    "You finished the quest! You may either continue in this area or switch quests using the quest book on your screen.",
                    new[] { "ok" }, new Action[] {
                        () => { }
                    });
            }
            else
            {
                _quests.SetCurrentQuest(new QuestInstance(_quests.CurrentQuest.Config.NextQuestConfig, true));
            }
        }
    }

    public void PlayerDied(float goldLoss) {
        
        // Set death thing to active
        DeathCover.gameObject.SetActive(true);
        _enemy.Stop();
        _attackArea.gameObject.SetActive(false);

        OnPlayerDeath?.Invoke();

        DeathCover.StartLerping(() => {
            Fighting = false;
            _enemy.gameObject.SetActive(false);
            _dragger.ResetScrolling();
            OnPlayerDeathReset?.Invoke();

            // Disable enemy fighter
            _playerAnims.SpinDone(1);
            _playerAnims.StopAllCoroutines();

            // Reverse this lerp
            DeathCover.StartReverseLerp(() => {
                _dragger.Enable();
                OnPlayerDeathDone?.Invoke(goldLoss);
            });
        });
    }
}
