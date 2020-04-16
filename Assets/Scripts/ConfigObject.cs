using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.DropSystem;
using UnityEngine;

[Serializable]
public class RarityDefines {

	[System.Serializable]
	public class RarityColorDefines {
		public Rarity Rarity;
		public Color Color;
        public AnimationCurve WeaponLevelPerTap;
	}

	public List<RarityColorDefines> RarityToColor;
	
    public static float GetLevel(Rarity r, int taps) {
        var thing = Configs.Instance.Rarities.RarityToColor.FirstOrDefault(rcd => rcd.Rarity == r);
        if(thing == null) {
            throw new Exception("No color define for rarity");
        }
        return thing.WeaponLevelPerTap.Evaluate(taps);
    }

	public static Color GetColorFromRarity(Rarity r) {
		var thing = Configs.Instance.Rarities.RarityToColor.FirstOrDefault(rcd => rcd.Rarity == r);
		if (thing == null) {
			throw new Exception("No color define for rarity");
		}
		return thing.Color;
	}
}
