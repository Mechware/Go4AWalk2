using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    [CreateAssetMenu(menuName = "Data/Items/Armor")]
    public class Armor : Item
    {
        public float NoBlockModifier;
        public float PerfectBlockModifier;
        public float MistimedBlockModifier;
        public float DamageAdditiveModifier;

        public float GetDamage(int damage, bool perfectBlock, bool mistimedBlock, bool badParry)
        {
            float fdamage = damage - DamageAdditiveModifier;
            fdamage = Mathf.Max(0, fdamage);

            if (perfectBlock) return fdamage * PerfectBlockModifier;
            if (mistimedBlock) return fdamage * MistimedBlockModifier;
            if (badParry) return fdamage;
            return fdamage * NoBlockModifier;
        }
    }
}


