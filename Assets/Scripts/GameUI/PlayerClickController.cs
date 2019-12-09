using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using UnityEngine;

public class PlayerClickController : MonoBehaviour {
    public MyButton PlayerHeadgear;
    public MyButton PlayerArmor;
    public MyButton PlayerWeapon;

    public static PlayerClickController Instance;
    
    void Awake() {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerHeadgear.onClick.AddListener(ChangePlayerHeadgear);  
        
        PlayerArmor.onClick.AddListener(ChangePlayerArmor);

        PlayerWeapon.onClick.AddListener(ChangePlayerWeapon);
    }

    public static void ChangePlayerHeadgear() {
        ItemViewer.Instance.ShowItemsFromInventory<HeadgearInstance>("Equip Headgear", it => {
            EquipItemProcessor.Instance.ProcessItem(it, () => {
                ItemViewer.Instance.Close();
            });
        }, false); 
    }

    public static void ChangePlayerArmor() {
        ItemViewer.Instance.ShowItemsFromInventory<ArmorInstance>("Equip Armor", it => {
            EquipItemProcessor.Instance.ProcessItem(it, () => {
                ItemViewer.Instance.Close();
            });
        }, false); 
    }

    public static void ChangePlayerWeapon() {
        ItemViewer.Instance.ShowItemsFromInventory<WeaponInstance>("Equip Weapon", it => {
            EquipItemProcessor.Instance.ProcessItem(it, () => {
                ItemViewer.Instance.Close();
            });
        }, false, false);
    }
    
    
    public void SetEnabled(bool enabled) {
        PlayerWeapon.enabled = enabled;
        PlayerArmor.enabled = enabled;
        PlayerHeadgear.enabled = enabled;
    }
    
}
