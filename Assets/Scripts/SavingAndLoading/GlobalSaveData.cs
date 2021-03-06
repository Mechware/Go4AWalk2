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
        public DateTime LastTimePlayedUTC;

        public Dictionary<string, int> IdsToNumberOfTaps = new Dictionary<string, int>();
        public List<string> CraftingRecipesMade = new List<string>();
        public bool ShowParryAndBlockColors;

        public bool ShowTrashChecked;
    }

    public class ItemSaveData
    {
        public string Id;
        public bool MarkedAsTrash;
    }

    public class WeaponSaveData : ItemSaveData
    {
        public int Random;
        public int Level;
        public int Taps;

        public string EnchanterId;
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
        public string Id;
    }

    public class ShopFollowerSaveData : FollowerSaveData
    {
        public List<ItemSaveData> Items;
    }

    public class EnemySaveData : FollowerSaveData
    {
        public int Level;

    }

    public class QuestSaveData
    {
        public string Id;
        public int Progress;
        public bool Active;
        public bool Complete;
        public bool GotRewards;
    }

    public class QuestGiverSaveData : FollowerSaveData
    {
        public string QuestToGiveId;
    }
}

