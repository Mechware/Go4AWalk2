using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.DropSystem;
using UnityEngine;

[Serializable]
public class RarityDefines : MonoBehaviour {

	[System.Serializable]
	public class RarityColorDefines {
		public Rarity Rarity;
		public Color Color;
        public AnimationCurve WeaponLevelPerTap;
	}

	public List<RarityColorDefines> RarityToColor;

	[NonSerialized] public static RarityDefines Instance = null;
    private void Awake()
    {
		if (Instance != null) Debug.LogError("More than one rarity define!");
		Instance = this;

    }

    public float GetLevel(Rarity r, int taps) {
        var thing = RarityToColor.FirstOrDefault(rcd => rcd.Rarity == r);
        if(thing == null) {
            throw new Exception("No color define for rarity");
        }
        return thing.WeaponLevelPerTap.Evaluate(taps);
    }

	public Color GetColorFromRarity(Rarity r) {
		var thing = RarityToColor.FirstOrDefault(rcd => rcd.Rarity == r);
		if (thing == null) {
			throw new Exception("No color define for rarity");
		}
		return thing.Color;
	}
}
