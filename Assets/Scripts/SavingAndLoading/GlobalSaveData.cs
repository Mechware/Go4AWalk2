using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2
{
    [CreateAssetMenu(menuName = "GlobalSaveData")]
    public class GlobalSaveData : ScriptableObject
    {
        public static MiscSaveData SaveData = new MiscSaveData();
        [SerializeField] private SaveManager _saveManager;

        private void OnEnable()
        {
            _saveManager.RegisterSaveFunction("GenericSaveData", Save);
            _saveManager.RegisterLoadFunction("GenericSaveData", Load);

        }

        private object Save()
        {
            SaveData.LastTimePlayedUTC = DateTime.UtcNow.SecondsSinceEpoch();
            return SaveData;
        }

        private void Load(object o)
        {
            if (o == null) return;
            SaveData = (MiscSaveData)o;
        }
    }

    public class MiscSaveData
    {
        public long LastTimePlayedUTC;

        public Dictionary<string, int> IdsToNumberOfTaps = new Dictionary<string, int>();
        public List<string> CraftingRecipesMade = new List<string>();
        public bool ShowParryAndBlockColors;

        public bool ShowTrashChecked;

        public long GetTimeSinceLastPlayed()
        {
            return DateTime.UtcNow.SecondsSinceEpoch() - LastTimePlayedUTC;
        }
    }

    public class ItemSaveData
    {
        public string Id;
        public bool MarkedAsTrash;

        public ItemSaveData(string id)
        {
            Id = id;
        }
    }

    public class WeaponSaveData : ItemSaveData
    {
        public int Random;
        public int Level;
        public EnchanterSaveData EnchanterData;
        public WeaponSaveData(string id, int random, int level) : base(id) { Random = random; Level = level; }
    }

    public class ArmorSaveData : ItemSaveData
    {
        public int Random;
        public int Level;
        public ArmorSaveData(string id, int random, int level) : base(id) { Random = random; Level = level; }
    }

    public class EnchanterSaveData : ItemSaveData
    {
        public int Random;
        public EnchanterSaveData(string id, int random) : base(id) { Random = random; }
    }

    public class HeadgearSaveData : ItemSaveData
    {
        public int Random;
        public int Level;
        public HeadgearSaveData(string id, int random, int level) : base(id) { Random = random; Level = level; }
    }

    public class FollowerSaveData
    {
        public string Id;

        public FollowerSaveData(string id)
        {
            Id = id;
        }
    }

    public class ShopFollowerSaveData : FollowerSaveData
    {
        public List<ItemSaveData> Items;

        public ShopFollowerSaveData(string id, List<ItemSaveData> items) : base(id) { Items = items; }
    }

    public class EnemySaveData : FollowerSaveData
    {
        public int Level;
        public EnemySaveData(string id, int level) : base(id) { Level = level; }
    }

    public class QuestSaveData
    {
        public string Id;
        public int Progress;
        public bool Active;
        public bool Complete;
        public bool GotRewards;

        public QuestSaveData(string id)
        {
            Id = id;
        }
    }

    public class QuestGiverSaveData : FollowerSaveData
    {
        public string QuestToGiveId;

        public QuestGiverSaveData(string id, string questToGive) : base(id) { QuestToGiveId = questToGive; }
    }
}

