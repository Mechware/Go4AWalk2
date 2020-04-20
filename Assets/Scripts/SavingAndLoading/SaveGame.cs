﻿using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class SaveGame : MonoBehaviour {
    public static SaveGame Instance;
    public static SaveData SaveData = new SaveData();

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
    public Dictionary<int, double> IdsToNumberOfTaps = new Dictionary<int, double>();
    public List<int> CraftingRecipesMade = new List<int>();
    public List<ConsumableSaveData> Consumables = new List<ConsumableSaveData>();
    
    // Questing
    public List<QuestSaveData> CurrentQuests = new List<QuestSaveData>();
    public List<QuestSaveData> CompletedQuests = new List<QuestSaveData>();


    public PlayerSaveData Player = new PlayerSaveData();

    public bool ShowTrashChecked;

    public Dictionary<int, int> EnemyKills = new Dictionary<int, int>();
    public Dictionary<int, int> ItemsCollected = new Dictionary<int, int>();

    public List<int> LockedSongs = new List<int>();
}

public class PlayerSaveData {
    public double Health;
    public int Gold;
    public int Level;
    public double XP;

    public int PerkHealth = 0;
    public int PerkMasteryGain = 0;
    public int PerkMasteryStart = 0;
    public int PerkSpeed = 0;
    public int PerkDodge = 0;
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

public class QuestGiverSaveData : FollowerSaveData {
    public int QuestToGiveId;
}