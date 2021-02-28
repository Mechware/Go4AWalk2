using System;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using G4AW2.Followers;
using UnityEngine;

namespace G4AW2.Data {
    public class QuestConfig : ScriptableObject {

        public int Id;
        public string DisplayName;
        public string Description;
        
        [Header("Progress")]
        public QuestConfig NextQuestConfig;
        public Conversation StartConversation;
        public Conversation EndConversation;

        [Header("Area Definitions")]
        public float MinEnemyDropTime = 240;
        public float MaxEnemyDropTime = 480;
        public Area Area;
        public List<MiningPoint> MiningPoints;
        public FollowerDropData Enemies;
        public List<Reward> QuestRewards;

        public QuestType QuestType;
        public ScriptableObject QuestParam;
        
        public int ValueToReach;

        [Tooltip("This is the level that enemys are based off of")]
        public int Level;

        [Serializable]
        public class Reward {
            public ItemConfig it;
            public int Level = -1;
            [Tooltip("-1 means roll it on creation")]
            public int RandomRoll = -1; 
        }


    }

    public enum QuestType {
        EnemySlaying,
        ItemCollecting,
        ItemGiving,
        Boss
    }
}