using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using UnityEngine;

public class GameEventHandler : MonoBehaviour {

    public static GameEventHandler Singleton;

    public static Action<ActiveQuestBase> QuestChanged;
    public static Action<EnemyInstance> EnemyKilled;
    public static Action<ItemConfig> LootObtained;

    void Awake() {
        Singleton = this;
    }

    public void OnQuestChanged(ActiveQuestBase quest) {
        QuestChanged?.Invoke(quest);
    }

    public void OnEnemyKilled(EnemyInstance ed) {
        EnemyKilled?.Invoke(ed);
    }

    public void OnLootObtained(IEnumerable<ItemConfig> its) {
        foreach(var it in its) OnLootObtained(it);
    }

    public void OnLootObtained(ItemConfig it) {
        LootObtained?.Invoke(it);
    }

    public void OnLootObtained(ItemConfig it, int amount) {
        for (int i = 0; i < amount; i++) LootObtained?.Invoke(it);
    }
}
