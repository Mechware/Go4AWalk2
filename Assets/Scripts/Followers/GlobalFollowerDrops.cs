using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;

[Obsolete("Not used, plz delete")]
public static class GlobalFollowerDrops {
    public static List<FollowerDrop> GlobalDrops = new List<FollowerDrop>();
}

[System.Serializable]
public class FollowerDrop {
    public FollowerData Follower;
    public int MinLevel;
    public int MaxLevel;
    public int DropChance;
}