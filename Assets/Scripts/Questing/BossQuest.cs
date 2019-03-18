using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quests/BossQuest")]
public class BossQuest : ActiveQuest<int, IntVariable, UnityEventInt> {

    public RuntimeSetFollowerData EnemyList;
    public EnemyData Enemy;

    public override void StartQuest(Action<ActiveQuestBase> onFinish) {
        base.StartQuest(onFinish);
        EnemyList.Add(Enemy);
    }

    public override void ResumeQuest(Action<ActiveQuestBase> onFinish) {
        base.ResumeQuest(onFinish);
        if(EnemyList.FirstOrDefault(e => e.ID == Enemy.ID) == null) {
            EnemyList.Add(Enemy);
        }
    }

    protected override void OnTotalChanged(int totalAmount) {
        AmountSoFar.Value = totalAmount - amountWhenStarted;
        if(IsFinished()) {
            FinishQuest();
        }
    }

    protected override void UpdateAmountOnStart() {
        amountWhenStarted = TotalAmount - AmountSoFar;
    }

    public override bool IsFinished() {
        return AmountSoFar.Value >= AmountToReach;
    }

#if UNITY_EDITOR
    [ContextMenu("Pick ID")]
    public new void PickID() {
        ID = IDUtils.PickID<Quest>();
    }
#endif
}
