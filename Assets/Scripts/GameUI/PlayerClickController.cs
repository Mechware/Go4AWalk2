using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using UnityEngine;

public class PlayerClickController : MonoBehaviour {
    public MyButton PlayerHeadgear;
    public MyButton PlayerArmor;
    public MyButton PlayerWeapon;

    public HeadgearReference HeadgearReference;
    public ArmorReference ArmorReference;
    public WeaponReference WeaponReference;

    public static PlayerClickController Instance;

    public InteractionController _controller;

    void Awake() {
        Instance = this;
        _controller.OnFightEnter += () => SetEnabled(false);
        _controller.OnEnemyDeathFinished += (e) => SetEnabled(true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerHeadgear.onClick.AddListener(ChangePlayerHeadgear);  
        
        PlayerArmor.onClick.AddListener(ChangePlayerArmor);

        PlayerWeapon.onClick.AddListener(ChangePlayerWeapon);
    }

    public static void ChangePlayerHeadgear() {
        ItemViewer.Instance.ShowItemsFromInventory<Headgear>("Equip Headgear", false, false, it => {
            EquipItemProcessor.Instance.ProcessItem(it, () => {
                ItemViewer.Instance.Close();
            });
        }); 
    }

    public static void ChangePlayerArmor() {
        ItemViewer.Instance.ShowItemsFromInventory<Armor>("Equip Armor", false, false, it => {
            EquipItemProcessor.Instance.ProcessItem(it, () => {
                ItemViewer.Instance.Close();
            });
        }); 
    }

    public static void ChangePlayerWeapon() {
        ItemViewer.Instance.ShowItemsFromInventory<Weapon>("Equip Weapon", false, false, it => {
            EquipItemProcessor.Instance.ProcessItem(it, () => {
                ItemViewer.Instance.Close();
            });
        });
    }
    
    
    public void SetEnabled(bool enabled) {
        PlayerWeapon.enabled = enabled;
        PlayerArmor.enabled = enabled;
        PlayerHeadgear.enabled = enabled;
    }
    
}
