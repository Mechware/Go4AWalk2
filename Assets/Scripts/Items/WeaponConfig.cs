using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data
{
    [CreateAssetMenu(menuName = "Data/Items/Weapon")]
    public class WeaponConfig : ItemConfig {

        public float TapSpeed = 2f;
        public float DamageAtLevel0;
    }
}


