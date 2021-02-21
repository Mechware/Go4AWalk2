using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using TMPro;
using UnityEngine;

public class WalkingUIController : MonoBehaviour {

    [Obsolete("Singleton")]
    public static WalkingUIController Instance;

    public RectTransform MasteryFill;
    public TextMeshProUGUI MasteryText;

    public ItemViewer ItemViewer;
    public WeaponUI WeaponViewer;

    public InventoryItemDisplay Weapon;
    public InventoryItemDisplay Armor;
    public InventoryItemDisplay Headgear;

    [SerializeField] private Animator animator;
    private static readonly int ShowHash = Animator.StringToHash("Showing");

    [SerializeField] private InteractionController _interactionController;
    void Awake() {
        Instance = this;
    }
    
    public void Show()
    {
        animator.SetBool(ShowHash, true);
    }

    public void Hide()
    {
        animator.SetBool(ShowHash, false);
    }

    public void Initialize()
    {
        Show();
        _interactionController.OnFightStart += () =>
        {
            Hide();
        };

        _interactionController.OnEnemyDeathFinished += e =>
        {
            Show();
        };

        _interactionController.OnPlayerDeathReset += () =>
        {
            Show();
        };

        ItemViewer.Init();
        WeaponViewer.Init();
    }

    // Update is called once per frame
    void Update() {
        var weapon = DataManager.Instance.Player.Weapon.Value;
        
        float currentDamage = weapon.RawDamage;
        
        MasteryText.text = $"Weapon Mastery {weapon.Mastery}";
        
        if(weapon.Mastery == 99) {
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(1);
        }
        else {
            float masteryProgress = weapon.RawMastery - Mathf.Floor(weapon.RawMastery);
            float nextLevelDamage = weapon.GetDamage(mastery: weapon.Mastery+1);
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(masteryProgress);
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

    public RobustLerper MasteryPopUpLerp;
    public TextMeshProUGUI MasteryPopUpText;
    public void MasteryPopUp(string popUp) {
        MasteryPopUpText.text = popUp;
        MasteryPopUpLerp.StartLerping();
    }
}
