using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

public class Configs : MonoBehaviour {

    public static Configs Instance;
    
    public void Awake() {
        Instance = this;
        
        #if UNITY_EDITOR
        RefreshConfigs();
        #endif
    }

    public List<ItemConfig> Items; 
    public List<FollowerConfig> Followers;
    public List<CraftingRecipe> Recipes;
    public List<QuestConfig> Quests;

    public RarityDefines Rarities;

    
    
    #if UNITY_EDITOR
    [Button("Refresh")]
    public void RefreshConfigs() {
        Debug.Log("Refreshing Configs!");
        AddAllOfType(Items);
        AddAllOfType(Followers);
        AddAllOfType(Quests);
    }

    [Button("Ensure No Duplicate Ids")]
    public void EnsureNoDups() {
        Dictionary<int, Object> ids = new Dictionary<int, Object>();
        foreach (var thing in Items) {
            if (ids.ContainsKey(thing.Id)) {
                Debug.LogWarning($"Items have same id as each other: {thing.name} and {ids[thing.Id].name}");
                continue;
            }
            ids.Add(thing.Id, thing);
        }

        ids.Clear();
        foreach(var thing in Followers) {
            if (ids.ContainsKey(thing.Id)) {
                Debug.LogWarning($"Followers have same id as each other: {thing.name} and {ids[thing.Id].name}");
                continue;
            }
            ids.Add(thing.Id, thing);
        }

        ids.Clear();
        foreach(var thing in Recipes) {
            if (ids.ContainsKey(thing.Id)) {
                Debug.LogWarning($"Recipes have same id as each other: {thing.name} and {ids[thing.Id].name}");
                continue;
            }
            ids.Add(thing.Id, thing);
        }

        ids.Clear();
        foreach(var thing in Quests) {
            if (ids.ContainsKey(thing.Id)) {
                Debug.LogWarning($"Quests have same id as each other: {thing.name} and {ids[thing.Id].name}");
                continue;
            }
            ids.Add(thing.Id, thing);
        }
        
        AssetDatabase.SaveAssets();
    }
    
    
    public void AddAllOfType<T>(List<T> list) where T : Object {
        list.Clear();
        string[] paths = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        for (int i = 0; i < paths.Length; i++) {
            paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
        }

        paths.Select(AssetDatabase.LoadAssetAtPath<T>).ForEach(list.Add);

        while (list.Contains(null)) {
            list.Remove(null);
            Debug.LogError("Null!");
        }
    }
    #endif
}

public enum Rarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    VeryRare = 3,
    Legendary = 4,
    Mythical = 5
}

