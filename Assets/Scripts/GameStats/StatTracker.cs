using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class StatTracker : MonoBehaviour {

    [Serializable]
    public class EnemyCounter {
        public EnemyData Enemy;
        public IntVariable Count;
    }

    public List<EnemyCounter> EnemyKillCount;
    private Dictionary<int, IntVariable> enemyDictionary;


    [Serializable]
    public class ItemCounter {
        public Item Item;
        public IntVariable Count;
    }

    public List<ItemCounter> ItemObtainedCount;
    private Dictionary<int, IntVariable> itemDictionary;

    void Awake() {
        GameEventHandler.EnemyKilled += EnemyKilled;
        GameEventHandler.LootObtained += LootObtained;

        itemDictionary = new Dictionary<int, IntVariable>();
        foreach (ItemCounter item in ItemObtainedCount) {
            itemDictionary.Add(item.Item.ID, item.Count);
        }

        enemyDictionary = new Dictionary<int, IntVariable>();
        foreach (EnemyCounter monster in EnemyKillCount) {
            enemyDictionary.Add(monster.Enemy.ID, monster.Count);
        }
    }

    private void LootObtained(Item item) {
        if (itemDictionary.ContainsKey(item.ID)) {
            itemDictionary[item.ID].Value++;
        }
    }

    private void EnemyKilled(EnemyData enemyData) {
        if (enemyDictionary.ContainsKey(enemyData.ID)) {
            enemyDictionary[enemyData.ID].Value++;
        }
    }
}
