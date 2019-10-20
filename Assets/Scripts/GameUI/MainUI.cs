using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour {
    public RectTransform MasteryFill;
    public TextMeshProUGUI MasteryBeginText;
    public TextMeshProUGUI MasteryEndText;

    public ItemViewer ItemViewer;
    public WeaponUI WeaponViewer;

    public InventoryItemDisplay Weapon;
    public InventoryItemDisplay Armor;
    public InventoryItemDisplay Headgear;
    
    void Awake() {
        ItemViewer.Init();
        WeaponViewer.Init();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        var weapon = DataManager.Instance.Player.Weapon.Value;
        
        float currentDamage = weapon.RawDamage;
        
        if(weapon.Mastery == 99) {
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(1);
            MasteryBeginText.text = $"{weapon.Mastery} ({currentDamage})";
            MasteryEndText.text = $"";
        }
        else {
            float masteryProgress = weapon.RawMastery - Mathf.Floor(weapon.RawMastery);
            float nextLevelDamage = weapon.GetDamage(mastery: weapon.Mastery+1);
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(masteryProgress);
            MasteryBeginText.text = $"{weapon.Mastery} ({currentDamage})";
            MasteryEndText.text = $"{weapon.Mastery+1} ({nextLevelDamage})";
        }
        
        Weapon.SetData(DataManager.Instance.Player.Weapon.Value, 0, ChangeWeapon, null, true);
        Armor.SetData(DataManager.Instance.Player.Armor.Value, 0, ChangeArmor, null, true);
        Headgear.SetData(DataManager.Instance.Player.Headgear.Value, 0, ChangeHeadgear, null, true);
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
}
