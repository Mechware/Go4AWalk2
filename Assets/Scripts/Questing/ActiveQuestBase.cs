using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G4AW2.Data.Area;
using G4AW2.Dialogue;
using G4AW2.Followers;

namespace G4AW2.Questing {
    public class ActiveQuestBase : Quest {
        public ActiveQuestBase NextQuest;
        public Area Area;
        public List<MiningPoint> MiningPoints;
        public FollowerDropData Enemies;

        public Conversation StartConversation;
        public Conversation EndConversation;

        public virtual void StartQuest(Action<ActiveQuestBase> onFinish) { }

        public virtual void ResumeQuest(Action<ActiveQuestBase> onFinish) { }

        public virtual bool IsFinished() { throw new NotImplementedException();}
    }
}
