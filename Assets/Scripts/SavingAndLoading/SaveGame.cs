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
        
        private Dictionary<string, Func<object>> GetSaveDataFunctions = new Dictionary<string, Func<object>>();

        public void RegisterSaveFunction(string saveFile, Func<object> getSaveDataFunction)
        {
            GetSaveDataFunctions[saveFile] = getSaveDataFunction;
        }

        public void Save(string savePath)
        {
            Dictionary<string, object> saveData = new Dictionary<string, object>();
            foreach(var kvp in GetSaveDataFunctions)
            {
                saveData.Add(kvp.Key, kvp.Value());
            }

            var bytes = Serialization.SerializeValue(saveData, DataFormat.JSON);
            File.WriteAllBytes(savePath, bytes);
        }

        public Dictionary<string, object> Load(string savePath)
        {
            if (!File.Exists(savePath)) return null;
            var bytes = File.ReadAllBytes(savePath);
            var saveDict = Serialization.DeserializeValue<Dictionary<string, object>>(bytes, DataFormat.JSON);
            return saveDict;
        }

#if UNITY_EDITOR
        [Serializable]
        private class SaveDataTest
        {
            public bool Field0 = false;
            public int Field1 = 1;
            public string Field2 = "2";
            public NestedTest Nest = new NestedTest();
        }

        [Serializable]
        private class NestedTest
        {
            public string NestedValue3 = "3";
        }

        [ContextMenu("Run Test")]
        private void RunTest()
        {
            GetSaveDataFunctions.Clear();
            var sd = new SaveDataTest();
            sd.Field0 = true;
            sd.Field1 = 10;
            sd.Field2 = "not 2 lol";
            sd.Nest.NestedValue3 = "4";
            RegisterSaveFunction("Hello", () => sd);
            string saveFilePath = Application.persistentDataPath + "Test.json";
            Debug.Log(saveFilePath);
            Save(saveFilePath);
            GetSaveDataFunctions.Clear();
            var saveDict = Load(saveFilePath);
            var sd2 = (SaveDataTest) saveDict["Hello"];
            Debug.Assert(sd.Field0 == sd2.Field0);
            Debug.Assert(sd.Field1 == sd2.Field1);
            Debug.Assert(sd.Field2 == sd2.Field2);
            Debug.Assert(sd.Nest.NestedValue3 == sd2.Nest.NestedValue3);
            Debug.Log("Pass!");
        }
#endif

        public static T GetObject<T>(Dictionary<string, object> dict, string key)
        {
            if (dict == null) return default(T);
            return dict.ContainsKey(key) ? (T) dict[key] : default(T);
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


