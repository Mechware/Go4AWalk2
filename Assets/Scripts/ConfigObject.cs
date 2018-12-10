using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.DropSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Config Settings")]
public class ConfigObject : ScriptableObject {

	[System.Serializable]
	public class RarityColorDefines {
		public Rarity Rarity;
		public Color Color;
	}

	public List<RarityColorDefines> RarityToColor;
	private static List<RarityColorDefines> rarityToColor;

	public static Color GetColorFromRarity(Rarity r) {
		var thing = rarityToColor.FirstOrDefault(rcd => rcd.Rarity == r);
		if (thing == null) {
			throw new Exception("No color define for rarity");
		}
		return thing.Color;
	}

	void OnEnable() {
		RegisterChanges();
	}

	void Awake() {
		RegisterChanges();
	}

	[ContextMenu("Register Changes")]
	public void RegisterChanges() {
		rarityToColor = RarityToColor;
	}
}
