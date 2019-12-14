using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using G4AW2.Data.Combat;
using G4AW2.Followers;
using UnityEngine;

public class FollowerFactory : MonoBehaviour
{
    public static FollowerInstance GetInstance(FollowerConfig config, int level) {
        if (config is EnemyConfig e) {
            return new EnemyInstance(e, level);    
        }

        if (config is QuestGiverConfig q) {
            return new QuestGiverInstance(q);    
        }

        if (config is ShopFollowerConfig s) {
            return new ShopFollowerInstance(s);
        }

        Debug.LogError("Tried to create a follower instance from config: " + config.Id);
        return new FollowerInstance();
    }
}
