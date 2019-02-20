
using G4AW2.Data.DropSystem;

namespace CustomEvents {
    [UnityEngine.CreateAssetMenu(menuName = "Variable/Specific/Weapon")]
	public class WeaponVariable : SaveableVariableWithSaveable<Weapon, UnityEventWeapon> {
	}
}
