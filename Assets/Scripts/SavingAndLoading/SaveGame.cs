using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour {
    public static SaveGame Instance;

    private void Awake() {
        Instance = this;
    }


    public List<ItemSaveData> Items = new List<ItemSaveData>();
    public List<FollowerSaveData> Followers = new List<FollowerSaveData>();

    public bool Load() {

        return true;
    }
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
