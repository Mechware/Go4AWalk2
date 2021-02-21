using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using UnityEngine;

public class InteractionController : MonoBehaviour {

    [Obsolete("Singleton")] public static InteractionController Instance;

    public bool Fighting { private set; get; }

    public ActiveQuestBaseVariable CurrentQuest;
    public RuntimeSetFollowerData Followers;
    public Player Player;
    public Inventory Inventory;
    public LerpToPosition LerperToBattleArea;
    public LerpToPosition EnemyPositionLerper;
    public RobustLerper DeathCover;

    [SerializeField] private EnemyDisplay _enemy;
    [SerializeField] private ScrollingImages _backgroundImages;
    [SerializeField] private PlayerAnimations _playerAnims;
    [SerializeField] private ItemDropBubbleManager _itemDropper;

    [Header("Boss References")]
    public Transform BossStartPosition;
    public Transform BossEndPosition;

    public int ScrollSpeed;
    public RectTransform PlayerRT;

    public MyButton FightButton;

    public Transform PlayerFightPosition;
    public Transform EnemyFightPosition;

    public float EnemyAndPlayerWalkSpeed;

    public Action<EnemyData> OnEnemyDeathFinished;
    public Action OnBossFightStarted;
    public Action OnFightEnter;
    public Action OnFightStart;
    public Action<(EnemyData enemy, bool suicide)> OnEnemyDeath;
    public Action OnPlayerDeath;
    public Action OnPlayerDeathReset;
    public Action<float> OnPlayerDeathDone;

    private void Awake() {
        Instance = this;
    }

    void Update() {
    }
    
    public void Initialize()
    {
        _enemy.OnDeath += EnemyDeath;
    }

    public void StartBossFight(BossQuest bossQuest) {

        StartCoroutine(_StartBossFight());

        IEnumerator _StartBossFight() {
            Fighting = true;
            OnFightEnter?.Invoke();
            OnBossFightStarted?.Invoke();
            
            // Put boss where they should be
            EnemyData boss = (EnemyData) Followers.FirstOrDefault(f => f.ID == bossQuest.Enemy.ID);
            if (boss == null) {
                boss = Instantiate(bossQuest.Enemy);
                boss.Level = bossQuest.Level;
                boss.AfterCreated();
                Followers.Add(boss);
            }

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
            OnFightStart?.Invoke();
        }
    }
    
    public void EnemyFight(EnemyData enemy) {
        OnFightEnter?.Invoke();
        
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
                OnFightStart?.Invoke();
            });
        });
    }


    public void EnemyDeath(EnemyData data, bool suicide = false) {

        StartCoroutine(_EnemyDeath());
        
        IEnumerator _EnemyDeath() {

            OnEnemyDeath?.Invoke((data, suicide));

            if (Player.Health.Value <= 0) {
                yield break;    
            }
            
            if (data.OneAndDoneAttacker && suicide) {
                _playerAnims.ResetAttack();
                
                yield return new WaitForSeconds(1); // Wait for suicide animation to complete
                _playerAnims.Spin(() => {
                    _Done(new List<Item>());
                });
                yield break;
            }
        
            SoundManager.Instance.PlaySound(SoundManager.Instance.Celebrate, 0.8f);
            
            bool celebrateDone = false;
            bool bubblesDone = false;
        
            List<Item> items = data.Drops.GetItems(true);
            foreach(Item item in items) {
                if(item is Weapon) {
                    Weapon weapon = item as Weapon;
                    weapon.Level = data.Level;
                }
                if(item is Armor) {
                    Armor armor = item as Armor;
                    armor.Level = data.Level;
                }
                if(item is Headgear) {
                    Headgear hg = item as Headgear;
                    hg.Level = data.Level;
                }
            }

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
        

        void _Done(List<Item> items) {
            _playerAnims.Spin(() => {
            
                _enemy.gameObject.SetActive(false);
            
                OnEnemyDeathFinished?.Invoke(data);
                Inventory.AddItems(items);

                // Note: If the boss quest is to fight a chicken and you kill any chicken (not just the boss) then the quest gets completed
                if (CurrentQuest.Value is BossQuest quest && quest.Enemy.ID == data.ID) {
                    quest.Finish();
                }

                Fighting = false;
            });
        }
    }

    public void PlayerDied(float goldLoss) {
        
        // Set death thing to active
        DeathCover.gameObject.SetActive(true);
        _enemy.Stop(); 
        OnPlayerDeath?.Invoke();

        DeathCover.StartLerping(() => {
            Fighting = false;
            _enemy.gameObject.SetActive(false);
            OnPlayerDeathReset?.Invoke();

            // Disable enemy fighter
            _playerAnims.SpinDone(1);
            _playerAnims.StopAllCoroutines();

            // Reverse this lerp
            DeathCover.StartReverseLerp(() => {
                // Player.DeathFinished
                OnPlayerDeathDone?.Invoke(goldLoss);
            });
        });
    }
}
