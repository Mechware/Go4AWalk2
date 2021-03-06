﻿using System;
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

    public static InteractionController Instance;

    public bool Fighting = false;

    public DragObject World;
    public ScrollingImages BackgroundImages;

    public ActiveQuestBaseVariable CurrentQuest;
    public RuntimeSetFollowerData Followers;
    public Player Player;
    public Inventory Inventory;
    public LerpToPosition LerperToBattleArea;
    public LerpToPosition EnemyPositionLerper;
    public GameObject AttackArea;
    public RobustLerperSerialized BattleUiLerper;
    public AlphaOfAllChildren BattleUi;
    public RobustLerper DeathCover;

    [Header("Boss References")]
    public Transform BossStartPosition;
    public Transform BossEndPosition;

    public int ScrollSpeed;
    public RectTransform PlayerRT;

    public MyButton FightButton;

    public Transform PlayerFightPosition;
    public Transform EnemyFightPosition;

    public float EnemyAndPlayerWalkSpeed;
    
    private void Awake() {
        Instance = this;
    }

    void Update() {
        BattleUiLerper.Update(Time.deltaTime);
    }
    
    public void StartBossFight() {

        StartCoroutine(_StartBossFight());

        IEnumerator _StartBossFight() {
            Fighting = true;
            PlayerClickController.Instance.SetEnabled(false);
            DeadEnemyController.Instance.ClearEnemies();
            QuickPopUp.QuickPopUpAllowed = false;
            
            // Disable being able to move the world but keep scrolling
            World.Disable();
            EnemyArrowIndicator.Instance.SetOnMainScreen(false);
        
            // Put boss where they should be
            var bossQuest = (BossQuest) CurrentQuest.Value;
            
            EnemyData boss = (EnemyData) Followers.FirstOrDefault(f => f.ID == bossQuest.Enemy.ID);
            if (boss == null) {
                boss = Instantiate(bossQuest.Enemy);
                boss.Level = bossQuest.Level;
                boss.AfterCreated();
                Followers.Add(boss);
            }

            EnemyDisplay.Instance.gameObject.SetActive(true);
            EnemyDisplay.Instance.SetEnemy(boss);
            
            var bossRT = EnemyDisplay.Instance.RectTransform; 
            bossRT.position = BossStartPosition.position;

            // Ensure player isn't moving.
            PlayerAnimations.Instance.StopWalking();
            // Ensure boss is facing the right direction
            bossRT.localScale = bossRT.localScale.SetX(-1);

            int oldScrollSpeed = BackgroundImages.ScrollSpeed;
            BackgroundImages.ScrollSpeed = ScrollSpeed;
            
            // Wait until boss is in proper position
            while (bossRT.position.x > BossEndPosition.position.x) {
                float dist = -Time.deltaTime * ScrollSpeed;
                bossRT.localPosition = bossRT.localPosition.AddX(dist);
                PlayerRT.localPosition = PlayerRT.localPosition.AddX(dist);
                yield return null;
            }

            BackgroundImages.ScrollSpeed = oldScrollSpeed;
            BackgroundImages.Pause();

            
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
            PlayerAnimations.Instance.StartWalking();
            EnemyDisplay.Instance.StartWalkingAnimation();

            bool playerReady = false;
            bool playerSpinning = false;
            bool enemyReady = false;
            while (!playerReady || !enemyReady) {
                if (!playerReady && !playerSpinning) {
                    if (PlayerRT.position.x >= PlayerFightPosition.position.x) {
                        PlayerAnimations.Instance.StopWalking();
                        playerSpinning = true;
                        PlayerAnimations.Instance.Spin(() => {
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
                        EnemyDisplay.Instance.StopWalking();
                    }
                    else {
                        float dist = -Time.deltaTime * EnemyAndPlayerWalkSpeed;
                        bossRT.localPosition = bossRT.localPosition.AddX(dist);    
                    }
                }

                yield return null;
            }
            
            
            
            // Once boss and player show up in proper places, start the fight like regular.
            EnemyDisplay.Instance.StartAttacking();
            BattleUi.gameObject.SetActive(true);
            BattleUi.SetAlphaOfAllChildren(0);
            BattleUiLerper.StartLerping();
            AttackArea.SetActive(true);
        }
    }
    
    public void EnemyFight(EnemyData enemy) {
        PlayerClickController.Instance.SetEnabled(false);
        QuickPopUp.QuickPopUpAllowed = false;
        World.Disable();
        BattleUi.gameObject.SetActive(true);
        BattleUi.SetAlphaOfAllChildren(0);
        
        // Scroll to fight area
        LerperToBattleArea.StartLerping(() => {
            PlayerAnimations.Instance.Spin();
            
            EnemyDisplay.Instance.gameObject.SetActive(true);
            EnemyDisplay.Instance.SetEnemy(enemy);
            EnemyDisplay.Instance.StartWalkingAnimation();
            EnemyPositionLerper.StartLerping(() => {
                
                // Start Combat
                EnemyDisplay.Instance.StopWalking();
                EnemyDisplay.Instance.StartAttacking();
                BattleUiLerper.StartLerping();
                AttackArea.SetActive(true);
                // Update enemy health bar?
                
            });
        });
    }

    public void EnemyDeath(EnemyData data, bool suicide = false) {

        QuickPopUp.QuickPopUpAllowed = true;
        BattleUiLerper.StartReverseLerp();

        
        StartCoroutine(_EnemyDeath());
        
        IEnumerator _EnemyDeath() {
            
            
            Followers.Remove(data);
            AttackArea.SetActive(false);
        
            if (Player.Health.Value <= 0) {
                OnPlayerDeath();
                yield break;    
            }
            
            if (data.OneAndDoneAttacker && suicide) {
                PlayerAnimations.Instance.ResetAttack();
                
                yield return new WaitForSeconds(1); // Wait for suicide animation to complete
                PlayerAnimations.Instance.Spin(() => {
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
            PlayerAnimations.Instance.ResetAttack();
            PlayerAnimations.Instance.Celebrate(() => {
                celebrateDone = true;
                if(bubblesDone && celebrateDone) {
                    _Done(items);
                }
            });
            
            ItemDropBubbleManager.Instance.AddItems(items, null, () => {
                bubblesDone = true;
                if(bubblesDone && celebrateDone) {
                    _Done(items);
                }
            });
        }
        

        void _Done(List<Item> items) {
            PlayerAnimations.Instance.Spin(() => {
            
                // Enable world to be interactable again
                // Turn off enemy fight.
                EnemyDisplay.Instance.gameObject.SetActive(false);
                // Call combat end event (nah)
            
                DeadEnemyController.Instance.AddDeadEnemy(
                    EnemyDisplay.Instance.RectTransform.anchoredPosition.x, 
                    EnemyDisplay.Instance.RectTransform.anchoredPosition.y, 
                    data);
                
                GameEventHandler.Singleton.OnEnemyKilled(data);
                Inventory.AddItems(items);
                World.Enable();

                PlayerClickController.Instance.SetEnabled(true);
                
                // Note: If the boss quest is to fight a chicken and you kill any chicken (not just the boss) then the quest gets completed
                if (CurrentQuest.Value is BossQuest quest && quest.Enemy.ID == data.ID) {
                    quest.Finish();
                }

                Fighting = false;
            });
        }
    }


    private bool ProcessingDeath = false;
    public void OnPlayerDeath() {

        if (ProcessingDeath) return;
        ProcessingDeath = true;
        
        // Set death thing to active
        DeathCover.gameObject.SetActive(true);
        // Disable attack area
        AttackArea.SetActive(false);
        
        //Lerp
        DeathCover.StartLerping(() => {
            Fighting = false;

            // Set battle ui to inactive
            BattleUi.gameObject.SetActive(false);
            // Set windows to active (?)
            
            // Call PlayerAnimation.SpinDone (to force player to spin)
            PlayerAnimations.Instance.SpinDone(1);
            // Reset world position
            World.ResetScrolling();
            // Disable enemy fighter
            EnemyDisplay.Instance.StopAllCoroutines();
            EnemyDisplay.Instance.gameObject.SetActive(false);
            // Reverse this lerp
            DeathCover.StartReverseLerp(() => {
                // Reset + Enable world scrolling
                World.Enable();
                // Player.DeathFinished
                Player.OnDeathFinished();
                ProcessingDeath = false;
            });
            
        });
            
    }
}
