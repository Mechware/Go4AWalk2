using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Items/Enchanting Type")]
public class EnchantingType : ScriptableObject {

    [System.Serializable]
    public class NamePrefix {
        public string Name;
        public int RandomValueMin;
        public int BaseDamage;
    }

    public Color DamageColor;
    public List<NamePrefix> NamePrefixes;

    public string GetPrefix(int random) {
        foreach(var prefix in NamePrefixes) {
            if(random >= prefix.RandomValueMin) {
                return prefix.Name;
            }
        }

        return NamePrefixes[NamePrefixes.Count - 1].Name;
    }

    public int GetDamage(int random) {
        foreach(var prefix in NamePrefixes) {
            if(random >= prefix.RandomValueMin) {
                return prefix.BaseDamage;
            }
        }

        return NamePrefixes[NamePrefixes.Count - 1].BaseDamage;
    }
}
