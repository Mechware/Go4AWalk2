using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using G4AW2.Saving;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StatTracker : MonoBehaviour {

    public static StatTracker Instance; 
    
    [Serializable]
    public class EnemyCounter {
        public EnemyConfig Enemy;
        public IntVariable Count;
    }

    public List<EnemyCounter> EnemyKillCount;
    private Dictionary<int, IntVariable> enemyDictionary;

    public RuntimeSetQuest CompletedQuests;
    

    [Serializable]
    public class ItemCounter {
        public ItemConfig ItemConfig;
        public IntVariable Count;
    }

    public List<ItemCounter> ItemObtainedCount;
    private Dictionary<int, IntVariable> itemDictionary;

    void Awake() {
        Instance = this;
        
        GameEventHandler.EnemyKilled += EnemyKilled;
        GameEventHandler.LootObtained += LootObtained;

        itemDictionary = new Dictionary<int, IntVariable>();
        foreach(ItemCounter item in ItemObtainedCount) {
            itemDictionary.Add(item.ItemConfig.Id, item.Count);
        }

        enemyDictionary = new Dictionary<int, IntVariable>();
        foreach(EnemyCounter monster in EnemyKillCount) {
            enemyDictionary.Add(monster.Enemy.ID, monster.Count);
        }
    }

    private void LootObtained(ItemConfig itemConfig) {
        if(itemDictionary.ContainsKey(itemConfig.Id)) {
            itemDictionary[itemConfig.Id].Value++;
        }
    }

    private void EnemyKilled(EnemyInstance enemy) {
        if(enemyDictionary.ContainsKey(enemy.Config.ID)) {
            enemyDictionary[enemy.Config.ID].Value++;
        }
    }

    public void CompleteQuest(ActiveQuestBase quest) {
        CompletedQuests.Add(quest);
    }
    
#if UNITY_EDITOR
    [ContextMenu("Create Variables For Enemies")]
    public void CreateVariablesToTrackEnemies() {
        string[] paths = AssetDatabase.FindAssets("t:" + typeof(EnemyConfig).Name);
        for(int i = 0; i < paths.Length; i++) {
            paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
        }

        List<EnemyConfig> enemies = paths.Select(AssetDatabase.LoadAssetAtPath<EnemyConfig>).ToList();

        foreach (var enemyData in enemies) {
            string variableTitle = $"{enemyData.DisplayName}sKilled";
            if (AssetDatabase.FindAssets(variableTitle).Length != 0) {
                continue;
            }
            IntVariable variable = ScriptableObject.CreateInstance<IntVariable>();
            variable.Value = 0;
            
            AssetDatabase.CreateAsset(variable, $"Assets/Data/StatVariables/Killed/{variableTitle}.asset");
            EnemyKillCount.Add(new EnemyCounter() {
                Enemy = enemyData,
                Count = variable
            });
        }
    }

    [ContextMenu("Create Variables For Items")]
    public void CreateVariablesToTrackItems() {
        string[] paths = AssetDatabase.FindAssets("t:" + typeof(ItemConfig).Name);
        for(int i = 0; i < paths.Length; i++) {
            paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
        }

        List<ItemConfig> items = paths.Select(AssetDatabase.LoadAssetAtPath<ItemConfig>).ToList();

        foreach(var item in items) {
            string variableTitle = $"{item.Name}sCollected";
            if(AssetDatabase.FindAssets(variableTitle).Length != 0) {
                continue;
            }
            IntVariable variable = ScriptableObject.CreateInstance<IntVariable>();
            variable.Value = 0;

            AssetDatabase.CreateAsset(variable, $"Assets/Data/StatVariables/Collected/{variableTitle}.asset");
            ItemObtainedCount.Add(new ItemCounter() {
                ItemConfig = item,
                Count = variable
            });
        }
    }
#endif

}
