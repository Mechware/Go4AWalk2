using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Followers;
using G4AW2.Questing;
using UnityEngine;

public class InteractionController : MonoBehaviour {

    public static InteractionController Instance;

    public bool InteractionActive = false;
    public bool Fighting = false;

    public DragObject World;

    public LerpToPosition EnemyPositionLerper;
    public GameObject AttackArea;
    public RobustLerperSerialized BattleUiLerper;
    public AlphaOfAllChildren BattleUi;
    public RobustLerper DeathCover;
    public ScrollingImages BG;
    
    
    private void Awake() {
        Instance = this;
    }
    
    public void InitializeAndStart() {
        StartCoroutine(RunGame());
    }

    // Update is called once per frame
    IEnumerator RunGame()
    {
        while (true) {
            // Load next thing to appear
            EnemyInstance fi = QuestManager.Instance.CurrentQuest.Config.Enemies.GetRandomFollower(true);

            EnemyFight(fi);
            while (InteractionActive) {
                if(Fighting)
                    PlayerFightingLogic.Instance.PlayerAttemptToHitEnemy();
                yield return null;
            }    
        }
    }

    void Update() {
        BattleUiLerper.Update(Time.deltaTime);
    }
    
    public void EnemyFight(EnemyInstance enemy) {
        InteractionActive = true;
        PlayerClickController.Instance.SetEnabled(false);
        QuickPopUp.QuickPopUpAllowed = false;
        World.Disable();
        BattleUi.gameObject.SetActive(true);
        BattleUi.SetAlphaOfAllChildren(0);
        BG.ScrollSpeed = 0;
        PlayerAnimations.Instance.StopWalking();
        
        
        EnemyDisplay.Instance.gameObject.SetActive(true);
        EnemyDisplay.Instance.SetEnemy(enemy);
        EnemyDisplay.Instance.StartWalkingAnimation();
        EnemyPositionLerper.StartLerping(() => {
            
            // Start Combat
            Fighting = true;
            EnemyDisplay.Instance.StopWalking();
            EnemyDisplay.Instance.StartAttacking();
            BattleUiLerper.StartLerping();
            AttackArea.SetActive(true);
            // Update enemy health bar?
        });
    }

    public Action<(EnemyInstance enemy, bool isSuicide)> OnEnemyDeath;
    
    public void EnemyDeath(EnemyInstance instance, bool suicide = false) {
        Fighting = false;
        QuickPopUp.QuickPopUpAllowed = true;
        BattleUiLerper.StartReverseLerp();
        OnEnemyDeath?.Invoke((instance, suicide));
        
        StartCoroutine(_EnemyDeath());
        
        IEnumerator _EnemyDeath() {
            
            AttackArea.SetActive(false);
        
            if (Player.Instance.Health <= 0) {
                OnPlayerDeath();
                yield break;    
            }
        
            SoundManager.Instance.PlaySound(SoundManager.Instance.Celebrate, 0.8f);
            
            List<ItemInstance> items = instance.Config.Drops.GetItems(true, instance.SaveData.Level);

            int count = 3;
            
            // Wait for player celebration to be done and all items to be picked up
            PlayerAnimations.Instance.ResetAttack();
            PlayerAnimations.Instance.Celebrate(_TaskDone);
            
            ItemDropBubbleManager.Instance.AddItems(items, null, _TaskDone);
            
            yield return new WaitForSeconds(2);
            _TaskDone();

            void _TaskDone() {
                count--;
                if (count != 0) {
                    return;
                }
                
                // Enable world to be interactable again
                // Turn off enemy fight.
                EnemyDisplay.Instance.gameObject.SetActive(false);
                // Call combat end event (nah)
        
                DeadEnemyController.Instance.AddDeadEnemy(
                    EnemyDisplay.Instance.RectTransform.anchoredPosition.x, 
                    EnemyDisplay.Instance.RectTransform.anchoredPosition.y, 
                    instance);
            
                GameEventHandler.Instance.OnEnemyKilled(instance);
                items.ForEach(it => Inventory.Instance.Add(it));
                World.Enable();

                PlayerClickController.Instance.SetEnabled(true);
            
                InteractionActive = false;
            }
        }
        

        
    }


    private bool ProcessingDeath = false;
    public void OnPlayerDeath() {

        if (ProcessingDeath) return;
        ProcessingDeath = true;

        Fighting = false;
        
        // Set death thing to active
        DeathCover.gameObject.SetActive(true);
        // Disable attack area
        AttackArea.SetActive(false);
        
        //Lerp
        DeathCover.StartLerping(() => {

            // Set battle ui to inactive
            BattleUi.gameObject.SetActive(false);
            // Set windows to active (?)
            
            // Reset world position
            World.ResetScrolling();
            
            // Disable enemy fighter
            EnemyDisplay.Instance.StopAllCoroutines();
            EnemyDisplay.Instance.gameObject.SetActive(false);
            PlayerAnimations.Instance.StopWalking();
            
            // Reverse this lerp
            DeathCover.StartReverseLerp(() => {
                // Reset + Enable world scrolling
                World.Enable();
                // Player.DeathFinished
                Player.Instance.OnDeathFinished();
                ProcessingDeath = false;
                InteractionActive = false;
            });
        });
    }
}
