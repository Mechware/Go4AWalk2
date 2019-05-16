using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using UnityEngine;
using UnityEngine.Events;

public class BossQuestController : MonoBehaviour {

    public ActiveQuestBaseVariable CurrentQuest;

    public ScrollingImages BackgroundImages;
    public PlayerAnimations PlayerAnimations;

    public RobustLerperSerialized PlayerScrollToFightPosition;
    public RobustLerperSerialized BossScrollToFightPosition;

    public EnemyDisplay Enemy;
    public DragObject BackgroundDraggable;

    public UnityEvent OnWaitingStart;
    public GameEvent FightStarted;

    private RectTransformPositionSetter playerPositionSetter;
    private RectTransformPositionSetter enemyPositionSetter;

    void Awake() {
        playerPositionSetter = PlayerAnimations.GetComponent<RectTransformPositionSetter>();
        enemyPositionSetter = Enemy.GetComponent<RectTransformPositionSetter>();
    }

    public void StartBossQuest() {
        BossQuest quest = (BossQuest) CurrentQuest.Value;

        // Set enemy display to boss
        var enemy = Instantiate(quest.Enemy);
        enemy.Level = quest.Level;
        Enemy.SetEnemy(enemy);

        Enemy.OnDeath.RemoveListener(OnEnemyDeath);
        Enemy.OnDeath.AddListener(OnEnemyDeath);

        // Set scale of enemy display so boss is facing you
        var scale = Enemy.transform.localScale;
        scale.x = -1 * Mathf.Abs(scale.x);
        Enemy.transform.localScale = scale;

        // Disallow scrolling
        BackgroundDraggable.Disable();

        // 1. Start screen scroll if not started
        BackgroundImages.Play();

        // 2. Stop player from walking & drag player backwards
        PlayerAnimations.StopWalking();

        scrolling = true;

        float widthOfEnemy = ((RectTransform) Enemy.transform).sizeDelta.x;
        enemyPositionSetter.SetX(54 + widthOfEnemy/2);
        Enemy.gameObject.SetActive(true);
    }

    public float MinXForBoss = 33;
    private bool scrolling = false;

    void Update() {
        if (scrolling) {
            playerPositionSetter.SetX(playerPositionSetter.GetX() - Time.deltaTime * BackgroundImages.ScrollSpeed);
            enemyPositionSetter.SetX(enemyPositionSetter.GetX() - Time.deltaTime * BackgroundImages.ScrollSpeed);
            if (enemyPositionSetter.GetX() < MinXForBoss) {
                scrolling = false;
                BackgroundImages.Pause();
                OnWaitingStart.Invoke();
            }
        }

        PlayerScrollToFightPosition.Update(Time.deltaTime);
        BossScrollToFightPosition.Update(Time.deltaTime);
    }


    public void FightBoss() {

        PlayerAnimations.StartWalking();
        PlayerScrollToFightPosition.StartLerping(() => {
            PlayerAnimations.StopWalking();
            PlayerAnimations.Spin(() => {});
        });

        Enemy.StartWalking();
        BossScrollToFightPosition.StartLerping(() => {
            enemyPositionSetter.SetScaleX(1);
            FightStarted.Raise();
        });
    }

    public void OnDeath() {
        if(CurrentQuest.Value is BossQuest) 
            ResumeBossQuest();
    }

    [ContextMenu("Resume")]
    public void ResumeBossQuest() {
        // Set player into boss position
        enemyPositionSetter.SetX(MinXForBoss);
        playerPositionSetter.SetX(-10);

        BossQuest quest = (BossQuest) CurrentQuest.Value;
        var enemy = Instantiate(quest.Enemy);
        enemy.Level = quest.Level;
        Enemy.SetEnemy(enemy);

        Enemy.OnDeath.RemoveListener(OnEnemyDeath);
        Enemy.OnDeath.AddListener(OnEnemyDeath);
    }

    void OnEnemyDeath(EnemyData death) {
        FinishBossQuest();
        Enemy.OnDeath.RemoveListener(OnEnemyDeath);
    }

    private void FinishBossQuest() {
        BossQuest quest = (BossQuest) CurrentQuest.Value;
        quest.Finish();
    }
}
