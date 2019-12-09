using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using UnityEditor;
using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    [CreateAssetMenu(menuName = "Data/Items/Item")]
    public class ItemConfig : ScriptableObject {

        public int Id;
        public string Name = "";
        public Sprite Image;
        public int Value;
        public string Description;
        public Rarity Rarity;
        public bool SellWithTrash;
    }
}

