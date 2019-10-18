using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using UnityEngine;

public class InteractionController : MonoBehaviour {

    public static InteractionController Instance;

    public DragObject World;

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

    
    private void Awake() {
        Instance = this;
    }

    void Update() {
        BattleUiLerper.Update(Time.deltaTime);
    }
    
    public void StartBossFight() {
        //
    }
    
    public void EnemyFight(EnemyData enemy) {

        QuickPopUp.QuickPopUpAllowed = false;
        World.Enable();
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
                QuickPopUp.QuickPopUpAllowed = true;
                World.Enable();


                if (CurrentQuest is BossQuest) {
                    BossQuest quest = (BossQuest) CurrentQuest.Value;
                    quest.Finish();
                }
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
