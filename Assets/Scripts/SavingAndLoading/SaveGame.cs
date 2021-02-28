using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Serialization = Sirenix.Serialization.SerializationUtility;

namespace G4AW2
{
    [CreateAssetMenu(menuName = "Managers/SaveGame")]
    public class SaveGame : ScriptableObject
    {
        public static SaveData SaveData = new SaveData();
        public Dictionary<string, string> SaveDict = new Dictionary<string, string>();

        private static string _savePath;

        private void OnEnable()
        {
            _savePath = Application.persistentDataPath + "G4AW2Save.dat";
        }

        [ContextMenu("Print Data")]
        public void PrintSaveDict()
        {
            Debug.Log("Print!");
            SaveDict.ForEach(kvp => Debug.Log(kvp));
            Debug.Log("End Print!");
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            SaveDict.Clear();
        }
        
        [ContextMenu("Save")]
        public bool Save()
        {
            var bytes = Serialization.SerializeValue(SaveDict, DataFormat.JSON);
            File.WriteAllBytes(_savePath, bytes);
            return true;
        }

        [ContextMenu("Load")]
        public bool Load()
        {
            if (!File.Exists(_savePath)) return true;
            var bytes = File.ReadAllBytes(_savePath);
            SaveDict = Serialization.DeserializeValue<Dictionary<string, string>>(bytes, DataFormat.JSON);
            return false;
        }

        [ContextMenu("Run Test")]
        private void RunTest()
        {
            Clear();
            Debug.Assert(SaveDict.Count == 0, "Could not initialize to a cleared dict");
            SaveDict.Add("Hello", "Ooogabooga");
            SaveDict.Add("Hello5", "Ooogabooga5");
            SaveDict.Add("Hello2", "Ooogabooga2");
            SaveDict.Add("Hello3", "Ooogabooga3");
            Clear();
            Debug.Assert(SaveDict.Count == 0, "Could not clear dict");
            SaveDict.Add("Hello", "Ooogabooga");
            SaveDict.Add("Hello5", "Ooogabooga5");
            SaveDict.Add("Hello2", "Ooogabooga2");
            SaveDict.Add("Hello3", "Ooogabooga3");
            Save();
            Clear();
            Load();
            Debug.Assert(SaveDict["Hello"] == "Ooogabooga", "Save dict isn't right");
            Debug.Assert(SaveDict["Hello5"] == "Ooogabooga5", "Save dict isn't right2");
            Debug.Assert(SaveDict["Hello2"] == "Ooogabooga2", "Save dict isn't right3");
            Debug.Assert(SaveDict["Hello3"] == "Ooogabooga3", "Save dict isn't right4");
            Debug.Log("Pass!");
        }
    }

    public class SaveData
    {
        public DateTime LastTimePlayedUTC;

        public List<ItemSaveData> Inventory = new List<ItemSaveData>();
        public List<FollowerSaveData> CurrentFollowers = new List<FollowerSaveData>();
        public Dictionary<int, int> IdsToNumberOfTaps = new Dictionary<int, int>();
        public List<int> CraftingRecipesMade = new List<int>();
        public List<ConsumableSaveData> Consumables = new List<ConsumableSaveData>();
        public bool ShowParryAndBlockColors;

        // Questing
        public List<QuestSaveData> CurrentQuests = new List<QuestSaveData>();
        [Obsolete("Make a large lists of quests and filter them on start up")] public List<QuestSaveData> CompletedQuests = new List<QuestSaveData>();

        public int PlayerHealth;
        public int PlayerGold;



        public bool ShowTrashChecked;

        public Dictionary<int, int> EnemyKills = new Dictionary<int, int>();
        public Dictionary<int, int> ItemsCollected = new Dictionary<int, int>();

        public List<int> LockedSongs = new List<int>();
    }

    public class ItemSaveData
    {
        public int Id;
        public bool MarkedAsTrash;
    }

    public class WeaponSaveData : ItemSaveData
    {
        public int Random;
        public int Level;
        public int Taps;

        public int EnchanterId;
        public int EnchanterRandom;
    }

    public class ArmorSaveData : ItemSaveData
    {
        public int Random;
        public int Level;
    }

    public class EnchanterSaveData : ItemSaveData
    {
        public int Random;
    }

    public class HeadgearSaveData : ItemSaveData
    {
        public int Random;
        public int Level;
    }

    public class FollowerSaveData
    {
        public int Id;
    }

    public class ShopFollowerSaveData : FollowerSaveData
    {
        public List<ItemSaveData> Items;
    }

    public class EnemySaveData : FollowerSaveData
    {
        public int Level;

    }

    public class ConsumableSaveData
    {
        public int Id;
        public double EndTime;
    }

    public class QuestSaveData
    {
        public int Id;
        public int Progress;
        public bool Active;
        public bool Complete;
        public bool GotRewards;
    }

    public class QuestGiverSaveData : FollowerSaveData
    {
        public int QuestToGiveId;
    }
}


