using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantingType : MonoBehaviour {

    [System.Serializable]
    public class NamePrefix {
        public string Name;
        public int RandomValueMin;
    }

    public List<NamePrefix> NamePrefixes;
}
