using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using UnityEngine;

public class GameEventHandler : MonoBehaviour {

    public static GameEventHandler Instance;

    public static Action<QuestConfig> QuestChanged;
    public static Action<EnemyInstance> EnemyKilled;
    public static Action<ItemInstance> LootObtained;

    void Awake() {
        Instance = this;
    }

    public void OnQuestChanged(QuestConfig questConfig) {
        QuestChanged?.Invoke(questConfig);
    }

    public void OnEnemyKilled(EnemyInstance ed) {
        EnemyKilled?.Invoke(ed);
    }

    public void OnLootObtained(IEnumerable<ItemInstance> its) {
        foreach(var it in its) OnLootObtained(it);
    }

    public void OnLootObtained(ItemInstance it) {
        LootObtained?.Invoke(it);
    }

    public void OnLootObtained(ItemInstance it, int amount) {
        for (int i = 0; i < amount; i++) LootObtained?.Invoke(it);
    }
}
