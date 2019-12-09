using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using UnityEditor;
using UnityEngine;

public class Configs : MonoBehaviour {

    public static Configs Instance;
    
    public void Awake() {
        Instance = this;
    }

    public List<ItemConfig> ItemConfigs; 
    public List<FollowerConfig> Followers;
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

