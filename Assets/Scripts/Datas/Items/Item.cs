using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.DropSystem
{
    public enum Rarity
    {
        Shit = 0,
        PolishedShit = 1,
        Rarish = 2,
        Rare = 3,
        Legendary = 4,
        AMAZING = 5
    }

    [CreateAssetMenu(fileName = "DropSystem/Item")]
    public class Item : ScriptableObject {
        public Sprite image;
        public float value;
        public string description;
        public Rarity rarity;

    }
}

