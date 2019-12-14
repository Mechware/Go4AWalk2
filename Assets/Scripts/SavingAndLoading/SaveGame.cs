using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class SaveGame : MonoBehaviour {
    public static SaveGame Instance;
    public static SaveData SaveData;

    private void Awake() {
        Instance = this;
    }
    
    public bool Save() {
        return true;
    }
    
    public bool Load() {
        return true;
    }

    private void OnApplicationPause(bool pauseStatus) {
        Save();
    }

    private void OnApplicationQuit() {
        Save();
    }
}

public class SaveData {
    public DateTime LastTimePlayedUTC;
    
    public List<ItemSaveData> Inventory = new List<ItemSaveData>();
    public List<FollowerSaveData> CurrentFollowers = new List<FollowerSaveData>();
    public Dictionary<int, int> IdsToNumberOfTaps = new Dictionary<int, int>();
    public List<int> CraftingRecipesMade = new List<int>();
    public List<ConsumableSaveData> Consumables = new List<ConsumableSaveData>();
    public bool ShowParryAndBlockColors;
    
    // Questing
    public List<QuestSaveData> CurrentQuests = new List<QuestSaveData>();
    public List<QuestSaveData> CompletedQuests = new List<QuestSaveData>();
    
    public int PlayerHealth;
    public int PlayerGold;



    public bool ShowTrashChecked;

    public Dictionary<int, int> EnemyKills = new Dictionary<int, int>();
    public Dictionary<int, int> ItemsCollected = new Dictionary<int, int>();

    public List<int> LockedSongs = new List<int>();
}

public class ItemSaveData {
    public int Id;
    public bool MarkedAsTrash;
}

public class WeaponSaveData : ItemSaveData{
    public int Random;
    public int Level;
    public int Taps;

    public int EnchanterId;
    public int EnchanterRandom;
}

public class ArmorSaveData : ItemSaveData {
    public int Random;
    public int Level;
}

public class EnchanterSaveData : ItemSaveData {
    public int Random;
}

public class HeadgearSaveData : ItemSaveData {
    public int Random;
    public int Level;
}

public class FollowerSaveData {
    public int Id;
}

public class ShopFollowerSaveData : FollowerSaveData {
    public List<ItemSaveData> Items;
}

public class EnemySaveData : FollowerSaveData{
    public int Level;
    
}

public class ConsumableSaveData {
    public int Id;
    public double EndTime;
}

public class QuestSaveData {
    public int Id;
    public int Progress;
    public bool Active;
    public bool Complete;
}