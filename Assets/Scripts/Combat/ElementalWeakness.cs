using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElementalWeakness {

    [Serializable]
    public struct Weakness {
        public ElementalType ElementalType;
        public float DamageMultiplier;
    }

    public Weakness[] Weaknesses;
}
