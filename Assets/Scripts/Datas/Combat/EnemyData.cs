using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;

namespace Combat
{
    public enum Element
    {
        Fire = 0,
        Earth = 1,
        Water = 2,
        Wind = 3
    }

    [CreateAssetMenu(fileName = "Enemy")]
    public class EnemyData : ScriptableObject
    {
        public int baseHealth;
        public int armor;
        public Element element;
        public List<SpellUnlock> spells;
    }


    [Serializable]
    public class SpellUnlock
    {
        public SpellData spell;
        public int unlockLevel;
    }
}


