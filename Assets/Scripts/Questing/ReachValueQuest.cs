using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.Area;
using G4AW2.Dialogue;
using G4AW2.Questing;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quests/Active/Reach Value")]
public class ReachValueQuest : ActiveQuest<int, IntVariable, UnityEventInt> {

    protected override void OnTotalChanged(int totalAmount) {
        AmountSoFar.Value = totalAmount;
        if(IsFinished()) {
            FinishQuest();
        }
    }

    protected override void UpdateAmountOnStart() {
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
