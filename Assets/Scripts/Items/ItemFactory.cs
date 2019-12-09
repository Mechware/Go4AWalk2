using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class ItemFactory
{
    public static ItemInstance GetInstance(ItemConfig config, int level, int random = -1) {
        if (config is WeaponConfig w) {
            var wi = new WeaponInstance(w, level);
            if (random != -1) wi.SaveData.Random = random;
            return wi;
        }
        
        if (config is ArmorConfig a) {
            var wi = new ArmorInstance(a, level);
            if (random != -1) wi.SaveData.Random = random;
            return wi;
        }
        
        if (config is HeadgearConfig hg) {
            var wi = new HeadgearInstance(hg, level);
            if (random != -1) wi.SaveData.Random = random;
            return wi;
        }

        if (config is EnchanterConfig e) {
            var wi = new EnchanterInstance(e);
            if (random != -1) wi.SaveData.Random = random;
            return wi;
        }

        return new ItemInstance(config);
    }
}
