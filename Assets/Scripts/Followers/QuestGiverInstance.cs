using System.Collections;
using System.Collections.Generic;
using G4AW2.Followers;
using UnityEngine;

public class QuestGiverInstance : FollowerInstance {
    public new QuestGiverConfig Config => (QuestGiverConfig) base.Config;

    public QuestGiverInstance(QuestGiverConfig c) {
        base.Config = c;
    }
}
