using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using System;
using System.ComponentModel;
using System.Linq;
using G4AW2.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quests/Active/BossQuest")]
public class BossQuest : ActiveQuestBase {

    [Header("Boss Objective")]
    public EnemyConfig Enemy;

    public override void StartQuest(Action<ActiveQuestBase> onFinish) {
        base.StartQuest(onFinish);
    }

    public void Finish() {
        finished(this);
    }

    public override (double current, double max) GetProgress() {
        return (0, 1);
    }

    public override bool IsFinished() {
        return false;
    }

#if UNITY_EDITOR
    [ContextMenu("Pick ID")]
    public new void PickID() {
        ID = IDUtils.PickID<Quest>();
    }
#endif
}
