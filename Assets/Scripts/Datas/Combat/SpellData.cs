using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Combat
{
    [CreateAssetMenu(fileName = "Spell")]
    public class SpellData : ScriptableObject
    {
        public string description;
        public string effect;
    }
}

