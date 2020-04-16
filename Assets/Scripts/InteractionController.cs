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

    public bool Fighting = false;

    public DragObject World;

    public LerpToPosition EnemyPositionLerper;
    public GameObject AttackArea;
    public RobustLerperSerialized BattleUiLerper;
    public AlphaOfAllChildren BattleUi;
    public RobustLerper DeathCover;

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
            FollowerInstance fi = QuestManager.Instance.CurrentQuest.Config.Enemies.GetRandomFollower(true);

            if (fi is EnemyInstance ei) {
                EnemyFight(ei);
                while (Fighting) {
                    yield return null;
                }    
            }
            // else if (fi is ShopFollowerInstance sfi) {
            //    ShopFollowerInstance
            //} else if (fi is QuestGiverInstance qgi) {

            //}
            
            Player.Instance.Health = Player.Instance.MaxHealth;

            
        }
    }

    void Update() {
        BattleUiLerper.Update(Time.deltaTime);
    }
    
    public void EnemyFight(EnemyInstance enemy) {
        Fighting = true;
        PlayerClickController.Instance.SetEnabled(false);
        QuickPopUp.QuickPopUpAllowed = false;
        World.Disable();
        BattleUi.gameObject.SetActive(true);
        BattleUi.SetAlphaOfAllChildren(0);
        
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
    }

    public Action<(EnemyInstance enemy, bool isSuicide)> OnEnemyDeath;
    
    public void EnemyDeath(EnemyInstance instance, bool suicide = false) {

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
            
            bool celebrateDone = false;
            bool bubblesDone = false;
        
            List<ItemInstance> items = instance.Config.Drops.GetItems(true, instance.SaveData.Level);

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
        

        void _Done(List<ItemInstance> items) {
            
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
            
            Fighting = false;
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
                Player.Instance.OnDeathFinished();
                ProcessingDeath = false;
                Fighting = false;
            });
        });
    }
}
