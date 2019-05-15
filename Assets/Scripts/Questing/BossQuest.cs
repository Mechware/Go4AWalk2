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

    public RuntimeSetFollowerData EnemyList;
    public EnemyData Enemy;
    public int Level;

    public override void StartQuest(Action<ActiveQuestBase> onFinish) {
        base.StartQuest(onFinish);
        var enemy = Instantiate(Enemy);
        enemy.Level = Level;
        EnemyList.Add(enemy);
        EnemyList.OnChange.AddListener(ListChanged);
    }

    private void ListChanged(FollowerData data) {
        if(EnemyList.FirstOrDefault(e => e.ID == Enemy.ID) == null) {
            fininshed?.Invoke(this);
            EnemyList.OnChange.RemoveListener(ListChanged);
        }
    }

    public override void ResumeQuest(Action<ActiveQuestBase> onFinish) {
        base.ResumeQuest(onFinish);
        if(EnemyList.FirstOrDefault(e => e.ID == Enemy.ID) == null) {
            var enemy = Instantiate(Enemy);
            enemy.Level = Level;
            EnemyList.Add(enemy);
        }
        EnemyList.OnChange.AddListener(ListChanged);
    }

#if UNITY_EDITOR
    [ContextMenu("Pick ID")]
    public new void PickID() {
        ID = IDUtils.PickID<Quest>();
    }
#endif
}
