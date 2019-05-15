using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G4AW2.Data.Area;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using G4AW2.Followers;
using UnityEngine;

namespace G4AW2.Questing {
    public class ActiveQuestBase : Quest {
        public ActiveQuestBase NextQuest;
        public Area Area;
        public List<MiningPoint> MiningPoints;
        public FollowerDropData Enemies;

        public Conversation StartConversation;
        public Conversation EndConversation;

        public List<Reward> QuestRewards;
        protected Action<ActiveQuestBase> fininshed;

        [Serializable]
        public class Reward {
            public Item it;
            public int Level = -1;
            [Tooltip("-1 means roll it on creation")]
            public int RandomRoll = -1; 
        }

        public virtual void StartQuest(Action<ActiveQuestBase> onFinish) {
            fininshed = onFinish;
        }

        public virtual void ResumeQuest(Action<ActiveQuestBase> onFinish) {
            fininshed = onFinish;
        }

        public virtual bool IsFinished() { throw new NotImplementedException();}
    }
}
