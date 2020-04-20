using System;
using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    [CreateAssetMenu(menuName = "Data/Items/Weapon")]
    public class WeaponConfig : ItemConfig {

        /// <summary>
        /// The damage at level 1
        /// </summary>
        public double BaseDamage;
        /// <summary>
        /// How much damage is added per level increase.
        /// </summary>
        public double DamageScaling;
        /// <summary>
        /// Minimum attacks per second
        /// </summary>
        public float SpeedMin;
        /// <summary>
        /// Maximum attacks per second
        /// </summary>
        public float SpeedMax;
    }
}


