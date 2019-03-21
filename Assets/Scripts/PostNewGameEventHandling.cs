using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class PostNewGameEventHandling : MonoBehaviour {

    public WeaponVariable PlayerWeapon;
    public ArmorVariable PlayerArmor;
    //public HeadgearVariable PlayerHeadgear;

    public void OnNewGame() {
        Weapon original = PlayerWeapon.Value;
        PlayerWeapon.Value = Instantiate(original);
        PlayerWeapon.Value.Level = 1;
        PlayerWeapon.Value.TapsWithWeapon.Value = 0;
        PlayerWeapon.Value.OnAfterObtained(original);

        Armor originalArmor = PlayerArmor;
        PlayerArmor.Value = Instantiate(originalArmor);
        PlayerArmor.Value.OnAfterObtained(originalArmor);
    }
}
