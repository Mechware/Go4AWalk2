using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class Game : MonoBehaviour {
    public WeaponConfig StartWeapon;
    public ArmorConfig StartArmor;
    
    void Start() {
        bool newGame = SaveGame.Instance.Load();

        if (newGame) {
            Player.Instance.Weapon = new WeaponInstance(StartWeapon, 1);
            Player.Instance.Weapon.SaveData.Random = 30;

            Player.Instance.Armor = new ArmorInstance(StartArmor, 1);
            Player.Instance.Armor.SaveData.Random = 30;
        }
    }

    void Update()
    {
        
    }
}
