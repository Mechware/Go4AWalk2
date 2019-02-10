
using G4AW2.Data.DropSystem;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Events/Specific/Weapon")]
	public class GameEventWeapon : GameEventGeneric<Weapon, GameEventWeapon, UnityEventWeapon> {
	}
}
