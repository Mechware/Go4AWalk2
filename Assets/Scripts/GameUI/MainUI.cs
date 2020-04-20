using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Followers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    public static MainUI Instance;
    
    public RectTransform PlayerHealthFill;
    public TextMeshProUGUI PlayerHealthText;
    
    public RectTransform MasteryFill;
    public TextMeshProUGUI MasteryText;

    public ItemViewer ItemViewer;
    public WeaponUI WeaponViewer;

    public InventoryItemDisplay Weapon;
    public InventoryItemDisplay Armor;
    public InventoryItemDisplay Headgear;

    public DragObject WorldView;

    public RectTransform EnemyHealth;
    public TextMeshProUGUI EnemyHealthText;

    public void Awake() {
        Instance = this;
    }
    
    public void Initialize() {
        ItemViewer.Init();
        WeaponViewer.Init();
    }

    public void MyUpdate() {
        var weapon = Player.Instance.Weapon;
        
        //float currentDamage = weapon.RawDamage;
        
        MasteryText.text = $"Weapon Mastery {weapon.MasteryLevel}";
        
        if(weapon.MasteryLevel == 99) {
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(1);
        }
        else {
            float masteryProgress = weapon.RawMasteryLevel - Mathf.Floor(weapon.RawMasteryLevel);
            //float nextLevelDamage = weapon.GetDamage(mastery: weapon.Mastery+1);
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(masteryProgress);
        }

        float playerHealth = Mathf.Clamp01((float)(Player.Instance.SaveData.Health / Player.Instance.MaxHealth));
        PlayerHealthFill.anchorMax = PlayerHealthFill.anchorMax.SetX(playerHealth);
        PlayerHealthText.text = $"{Player.Instance.SaveData.Health} / {Player.Instance.MaxHealth}";        
        
        Weapon.SetDataInstance(Player.Instance.Weapon, 0, ChangeWeapon, null, true);
        Armor.SetDataInstance(Player.Instance.Armor, 0, ChangeArmor, null, true);
        if(Player.Instance.Headgear != null) Headgear.SetDataInstance(Player.Instance.Headgear, 0, ChangeHeadgear, null, true);
        
        
        EnemyHealth.anchorMax =
            EnemyHealth.anchorMax.SetX((float)(EnemyDisplay.Instance.CurrentHealth / EnemyDisplay.Instance.MaxHealth));
        
        EnemyHealthText.Replace(("{current}", EnemyDisplay.Instance.CurrentHealth.ToString()), ("{max}", EnemyDisplay.Instance.MaxHealth.ToString()));
    }

    public void ChangeWeapon(InventoryItemDisplay it) {
        PlayerClickController.ChangePlayerWeapon();
    }
    
    public void ChangeArmor(InventoryItemDisplay it) {
        PlayerClickController.ChangePlayerArmor();
    }
    
    public void ChangeHeadgear(InventoryItemDisplay it) {
        PlayerClickController.ChangePlayerHeadgear();
    }

    public RobustLerper MasteryPopUpLerp;
    public TextMeshProUGUI MasteryPopUpText;
    public void MasteryPopUp(string popUp) {
        MasteryPopUpText.text = popUp;
        MasteryPopUpLerp.StartLerping();
    }
}
