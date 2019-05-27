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
    public EnemyData Enemy;
    public int Level;

    public void Finish() {
        finished(this);
    }

#if UNITY_EDITOR
    [ContextMenu("Pick ID")]
    public new void PickID() {
        ID = IDUtils.PickID<Quest>();
    }
#endif
}
