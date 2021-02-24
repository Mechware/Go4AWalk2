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

        public float TapSpeed = 2f;
        public float DamageAtLevel0;
    }
}


